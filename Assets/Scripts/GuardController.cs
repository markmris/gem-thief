using System.Collections;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public float walkSpeed;
    public float viewDistance;
    public float viewAngle;
    public Sprite questionMarkSprite;
    public Sprite exclamationMarkSprite;
    private Transform player;
    public SpriteRenderer spriteRenderer;
    private Vector2 distanceFromPlayer;
    private Vector2 facingDirection = Vector2.down;

    private int state = 0;
    private bool canSeePlayer;
    private bool debounce = false;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
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
                spriteRenderer.sprite = null;
                break;
            
            case 1:
                spriteRenderer.sprite = questionMarkSprite;
                break;
            
            case 2:
                spriteRenderer.sprite = exclamationMarkSprite;
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
