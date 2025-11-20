using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private Rigidbody2D rb;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Get input from arrow keys or WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        // Create movement vector and normalize it
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        
        // Apply movement to Rigidbody2D
        rb.linearVelocity = movement * moveSpeed;
        
        // Flip sprite based on horizontal direction
        if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        else if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1); // Face right
    }
}