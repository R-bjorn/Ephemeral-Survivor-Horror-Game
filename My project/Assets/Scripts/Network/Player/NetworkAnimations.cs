using Unity.Netcode;
using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{

    public class NetworkAnimations : NetworkBehaviour
    {
        public CharacterController controller;
        public float SpeedChangeRate = 10.0f;
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;


        // player
        private float _speed;
        private float _animationBlend;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        public Animator _animator;

        private Vector3 lastTransformPosition = Vector3.zero;

        private void Start()
        {
            AssignAnimationIDs();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);


            _animator.SetBool(_animIDGrounded, Grounded);

        }

        private void Update()
        {
            if (IsOwner) return;
            GroundedCheck();

            if (!Grounded) Jump();
            else Move();
        }


        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void SetSpeed()
        {
            var now = transform.position;
            var then = lastTransformPosition;
            var speed = (now - then).magnitude / Time.deltaTime;
            lastTransformPosition = now;
            _speed = speed;
        }

        private void Move()
        {
            SetSpeed();

            _animator.SetBool(_animIDFreeFall, false);
            _animator.SetBool(_animIDJump, false);
            // float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            float inputMagnitude = 1f;
            // Todo Get if sprint
            bool inputsprint = false;
            if (_speed > 2) inputsprint = true;

            float targetSpeed = inputsprint ? SprintSpeed : MoveSpeed;

            if (_speed < 0.1) targetSpeed = 0;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        private void Jump()
        {
            if (controller.velocity.y < 0f)
            {
                _animator.SetBool(_animIDFreeFall, true);
                _animator.SetBool(_animIDJump, false);
            }
            else
            {
                _animator.SetBool(_animIDFreeFall, false);
                _animator.SetBool(_animIDJump, true);
            }
        }

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            { 
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
                }         
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }

    }
}