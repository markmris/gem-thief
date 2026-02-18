using System.Collections;
using UnityEngine;
using Pathfinding;

public class GuardController : MonoBehaviour
{
    [Header ("---- Guard Variables ----")]
    public float walkSpeed;
    public float viewDistance;
    public float viewAngle;
    public Sprite questionMarkSprite;
    public Sprite exclamationMarkSprite;
    public SpriteRenderer statusSpriteRenderer;
    private SpriteRenderer guardSpriteRenderer;
    private Transform player;
    private PlayerController playerController;
    private GameOver gameOver;
    private Vector2 distanceFromPlayer;
    private Vector2 facingDirection;

    [Header ("---- Guard Objects ----")]
    public Sprite guardFront;
    public Sprite guardBack;
    public Sprite guardLeft;
    public Sprite guardRight;

    private Transform currentNode;
    private AIDestinationSetter aiDestinationSetter;
    private NodeController nodeController;
    private bool pathfindingDebounce = false;

    private int state = 0;
    private bool canSeePlayer;
    private bool debounce = false;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        nodeController = GameObject.Find("NodeContainer").GetComponent<NodeController>();
        gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
        guardSpriteRenderer = transform.GetComponent<SpriteRenderer>();
        aiDestinationSetter = transform.GetComponent<AIDestinationSetter>();

        currentNode = nodeController.GetNextNode(transform);
        aiDestinationSetter.target = currentNode;
    }

    void Update()
    {
        canSeePlayer = LookForPlayer();

        if (canSeePlayer)
        {
            aiDestinationSetter.target = transform;

            if (Mathf.Abs(distanceFromPlayer.normalized.y) > Mathf.Abs(distanceFromPlayer.normalized.x))
            {
                if (player.position.y > transform.position.y)
                {
                    guardSpriteRenderer.sprite = guardBack;
                    facingDirection = Vector2.up;
                }
                else
                {
                    guardSpriteRenderer.sprite = guardFront;
                    facingDirection = Vector2.down;
                }
            }
            else
            {
                if (player.position.x > transform.position.x)
                {
                    guardSpriteRenderer.sprite = guardRight;
                    facingDirection = Vector2.right;
                }
                else
                {
                    guardSpriteRenderer.sprite = guardLeft;
                    facingDirection = Vector2.left;
                }
            }

            if (!debounce)
            {
                debounce = true;
                StartCoroutine(LookAtPlayer());
            }
        }
        else
        {
            if (Mathf.Abs(aiDestinationSetter.target.position.x - transform.position.x) > .7f)
            {
                if (aiDestinationSetter.target.position.x > transform.position.x)
                {
                    guardSpriteRenderer.sprite = guardRight;
                    facingDirection = Vector2.right;
                }
                else
                {
                    guardSpriteRenderer.sprite = guardLeft;
                    facingDirection = Vector2.left;
                }
            }
            else
            {
                if (aiDestinationSetter.target.position.y > transform.position.y)
                {
                    guardSpriteRenderer.sprite = guardBack;
                    facingDirection = Vector2.up;
                }
                else
                {
                    guardSpriteRenderer.sprite = guardFront;
                    facingDirection = Vector2.down;
                }
            }
        }

        if (!pathfindingDebounce && Mathf.Abs(aiDestinationSetter.target.position.x - transform.position.x) < 0.4f && Mathf.Abs(aiDestinationSetter.target.position.y - transform.position.y) < 0.4f)
        {
            pathfindingDebounce = true;
            StartCoroutine(ContinuePatrol());
        }
    }

    bool LookForPlayer()
    {
        distanceFromPlayer = player.position - transform.position;
        if (distanceFromPlayer.magnitude > viewDistance) return false;

        float angleToPlayer = Vector2.Angle(facingDirection, distanceFromPlayer.normalized);
        if (angleToPlayer > viewAngle / 2f) return false;

        RaycastHit2D playerCast = Physics2D.Raycast(transform.position, distanceFromPlayer.normalized, viewDistance);
        if (playerCast.collider && !playerCast.collider.transform.CompareTag("Player")) return false;

        return playerController.visible;
    }

    void ChangeState()
    {
        switch (state)
        {
            case 0:
                statusSpriteRenderer.sprite = null;
                break;
            
            case 1:
                statusSpriteRenderer.sprite = questionMarkSprite;
                break;
            
            case 2:
                statusSpriteRenderer.sprite = exclamationMarkSprite;
                gameOver.EndGame();
                break;
        }
    }

    IEnumerator LookAtPlayer()
    {
        state = 1;
        float time = Time.time;

        while (canSeePlayer)
        {
            ChangeState();

            if (Time.time - time >= 1.5f)
            {
                state = 2;
                ChangeState();
                yield break;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        state = 0;
        ChangeState();
        debounce = false;
        aiDestinationSetter.target = currentNode;
    }

    IEnumerator ContinuePatrol()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));

        currentNode = nodeController.GetNextNode(currentNode);
        aiDestinationSetter.target = currentNode;
        pathfindingDebounce = false;
    }
}
