using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string LAST_INPUT_X = "LastInputX";
    private const string LAST_INPUT_Y = "LastInputY";
    private const string CURRENT_INPUT_X = "CurrentInputX";
    private const string CURRENT_INPUT_Y = "CurrentInputY";

    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveInput;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _rigidbody2D.linearVelocity = _moveInput * _moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _animator.SetBool(IS_WALKING, true);
        if (context.canceled)
        {

            _animator.SetBool(IS_WALKING, false);
            _animator.SetFloat(LAST_INPUT_X, _moveInput.x);
            _animator.SetFloat(LAST_INPUT_Y, _moveInput.y);
        }
        _moveInput = context.ReadValue<Vector2>();
        _animator.SetFloat(CURRENT_INPUT_X, _moveInput.x);
        _animator.SetFloat(CURRENT_INPUT_Y, _moveInput.y);
    }

}
