using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoviement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    private Rigidbody2D _rigidBody2d;
    private Vector2 _moveInput;



    void Awake()
    {
    }

    private void Start()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidBody2d.linearVelocity = _moveInput*moveSpeed;
    }

  
    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
}
