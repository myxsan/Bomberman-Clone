using UnityEngine;

public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody {get; private set;}

    [Header("Movement Values")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    public float speed = 5f;

    [Header("Direction Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;

    private AnimatedSpriteRenderer currentSpriteRenderer;
    
    private Vector2 direction = Vector2.down;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentSpriteRenderer = spriteRendererDown;
    }

    private void Update() 
    {
        if(Input.GetKey(inputUp)) {
            SetDirection(Vector2.up, spriteRendererUp);
        } else if(Input.GetKey(inputDown)) {
            SetDirection(Vector2.down, spriteRendererDown);
        } else if(Input.GetKey(inputLeft)) {
            SetDirection(Vector2.left, spriteRendererLeft);
        } else if(Input.GetKey(inputRight)) {
            SetDirection(Vector2.right, spriteRendererRight);
        } else {
            SetDirection(Vector2.zero, currentSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);    
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        currentSpriteRenderer = spriteRenderer;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        currentSpriteRenderer.idle = direction == Vector2.zero;
    }
}
