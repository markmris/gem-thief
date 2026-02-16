using System.Collections;
using UnityEngine;

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
    private Vector2 distanceFromPlayer;
    private Vector2 facingDirection;

    [Header ("---- Guard Objects ----")]
    public Sprite guardFront;
    public Sprite guardBack;
    public Sprite guardLeft;
    public Sprite guardRight;
    private Rigidbody2D rigidBody;

    private int state = 0;
    private bool canSeePlayer;
    private bool debounce = false;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        guardSpriteRenderer = transform.GetComponent<SpriteRenderer>();
        rigidBody = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (guardSpriteRenderer.sprite == guardFront)
        {
            facingDirection = Vector2.down;
        }
        else if (guardSpriteRenderer.sprite == guardBack)
        {
            facingDirection = Vector2.up;
        }
        else if (guardSpriteRenderer.sprite == guardLeft)
        {
            facingDirection = Vector2.left;
        }
        else if (guardSpriteRenderer.sprite == guardRight)
        {
            facingDirection = Vector2.right;
        }

        canSeePlayer = LookForPlayer();

        if (canSeePlayer)
        {
            if (!debounce)
            {
                debounce = true;
                StartCoroutine(LookAtPlayer());
            }
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

        else return true;
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
    }
}
