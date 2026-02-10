using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header ("---- Player Sprites ----")]
    public Sprite playerFront;
    public Sprite playerBack;
    public Sprite playerLeft;
    public Sprite playerRight;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    [Header ("---- Movement Variables ----")]
    public InputActionReference moveAction;
    public float walkSpeed;
    private Vector2 direction;

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        direction = moveAction.action.ReadValue<Vector2>();

        if (direction == Vector2.zero || direction.y < 0)
        {
            spriteRenderer.sprite = playerFront;
        }
        else if (direction.y > 0)
        {
            spriteRenderer.sprite = playerBack;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.sprite = playerLeft;
        }
        else
        {
            spriteRenderer.sprite = playerRight;
        }
    }

    void FixedUpdate()
    {
        rigidBody.linearVelocity = direction * walkSpeed;
    }
}
