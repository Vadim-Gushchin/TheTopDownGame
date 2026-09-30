using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalHelper;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveInput;
    private Animator _animator;
    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (PauseController.IsGamePaused)
        {
            if (_rigidbody2D.linearVelocity != Vector2.zero)
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
                StopMovementAnimation();
                StopFootSteps();
            }

            return;
        }
        _rigidbody2D.linearVelocity = _moveInput * _moveSpeed;
        _animator.SetBool(AnimatorConstants.IsWalking, _rigidbody2D.linearVelocity.magnitude > 0);

        if (_rigidbody2D.linearVelocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootSteps();
        }
        else if (_rigidbody2D.linearVelocity.magnitude == 0)
        {
            StopFootSteps();
        }
    }

    void StopMovementAnimation()
    {
        _animator.SetBool(AnimatorConstants.IsWalking, false);
        _animator.SetFloat(AnimatorConstants.LastInputX, _moveInput.x);
        _animator.SetFloat(AnimatorConstants.LastInputY, _moveInput.y);
    }


    public void Move(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
             StopMovementAnimation();
        }
        _moveInput = context.ReadValue<Vector2>();
        _animator.SetFloat(AnimatorConstants.CurrentInputX, _moveInput.x);
        _animator.SetFloat(AnimatorConstants.CurrentInputY, _moveInput.y);
    }

    void StartFootSteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootStep), 0f, footstepSpeed);

    }


    void StopFootSteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootStep));
    }

    void PlayFootStep()
    {
        SoundEffectManager.PlaySoundEffect(SoundEffectConstants.FootSteps, true);
    }

}
