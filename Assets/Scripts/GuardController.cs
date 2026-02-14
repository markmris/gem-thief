using Unity.VisualScripting;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public float walkSpeed;
    public float viewDistance;
    public float viewAngle;
    public Sprite questionMarkSprite;
    public Sprite exclamationMarkSprite;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Vector2 distanceFromPlayer;
    private Vector2 facingDirection = Vector2.down;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool canSeePlayer = LookForPlayer();

        if (canSeePlayer)
        {
            print("PLAYER SEEN");
        }
    }

    bool LookForPlayer()
    {
        distanceFromPlayer = player.position - transform.position;
        if (distanceFromPlayer.magnitude > viewDistance)return false;

        float angleToPlayer = Vector2.Angle(facingDirection, distanceFromPlayer.normalized);
        if (angleToPlayer > viewAngle / 2f) return false;

        RaycastHit2D playerCast = Physics2D.Raycast(transform.position, distanceFromPlayer.normalized, viewDistance);
        if (playerCast.collider && !playerCast.collider.transform.CompareTag("Player")) return false;

        else return true;
    }
}
