using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header ("---- Player Sprites ----")]
    public Sprite playerFront;
    public Sprite playerBack;
    public Sprite playerLeft;
    public Sprite playerRight;
    public GameObject donutObject;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    [Header ("---- Movement Variables ----")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public float walkSpeed;
    public float lookMagnitude;
    public CinemachinePositionComposer positionComposer;
    private Vector2 moveDirection;

    public bool visible;
    private bool donut;
    
    private Vector2 defaultCamOffset = new Vector2(0, 0.5f);
    private Vector2 lookDirection;
    private Vector2 camOffset;

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveDirection = moveAction.action.ReadValue<Vector2>();
        lookDirection = lookAction.action.ReadValue<Vector2>();

        if (moveDirection.y < 0)
        {
            spriteRenderer.sprite = playerFront;
        }
        else if (moveDirection.y > 0)
        {
            spriteRenderer.sprite = playerBack;
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.sprite = playerLeft;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.sprite = playerRight;
        }

        if (lookDirection.x != 0)
        {
            positionComposer.TargetOffset = (lookDirection * lookMagnitude) + defaultCamOffset;
        }
        else if (lookDirection.y != 0)
        {
            positionComposer.TargetOffset = lookDirection * lookMagnitude;
        }
        else
        {
            positionComposer.TargetOffset = defaultCamOffset;
        }
    }

    void FixedUpdate()
    {
        rigidBody.linearVelocity = moveDirection * walkSpeed;
    }

    void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.transform.name == "DiningTable" && !donut)
        {
            foreach(Transform child in collisionObj.transform)
            {
                if (child.name == "Donut") Destroy(child.gameObject);
            }

            donut = true;
        }
        else if (collisionObj.transform.name == "Plate" && donut)
        {
            GameObject donutClone = Instantiate(donutObject);
            SpriteRenderer donutRenderer = donutClone.GetComponent<SpriteRenderer>();
            donutRenderer.sortingLayerName = "Objects";
            donutRenderer.sortingOrder = 1;
            donutClone.transform.position = collisionObj.transform.position;
            collisionObj.transform.name = "UsedPlate";
        }
    }
}
