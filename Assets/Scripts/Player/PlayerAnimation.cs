using System;
using UnityEngine;
using UnityEngine.UI;

namespace BHSCamp
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] Canvas _staminaImage;
        [SerializeField] AudioClip _footStepSound;
        private Health _healthComponent;
        private AudioSource _audioSource;
        private Animator _animator;
        private Rigidbody2D _body;
        private Ground _ground;

        private void OnEnable()
        {
            _healthComponent.OnDamageTaken += EnableHurtParameter;
            _healthComponent.OnDeath += EnableIsDeadParameter;
        }

        private void OnDisable()
        {
            _healthComponent.OnDamageTaken -= EnableHurtParameter;
            _healthComponent.OnDeath -= EnableIsDeadParameter;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
            _ground = GetComponent<Ground>();
            _healthComponent = GetComponent<Health>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _animator.SetFloat("VelocityX", Mathf.Abs(_body.velocity.x));
            _animator.SetFloat("VelocityY", _body.velocity.y);
            _animator.SetBool("IsJumping", !_ground.OnGround);
            _animator.SetFloat("BlinkingTimeScaler", UnityEngine.Random.Range(0f, 0.8f));
        }

        public void SetInputX(float inputX)
        {
            if (inputX == 0) return;
            transform.localScale = new Vector2(
                Mathf.Sign(inputX) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
            );

            Vector3 _staminaScale = _staminaImage.transform.localScale;
            _staminaScale.x = Math.Sign(inputX) * Mathf.Abs(_staminaScale.x);
            _staminaImage.transform.localScale = _staminaScale;
        }

        public void PlayFootstepSound() {
            _audioSource.PlayOneShot(_footStepSound, 1);
        }

        public void SetIsRunning(bool isRunning) {
            _animator.SetBool("IsRunning", isRunning);
        }

        private void EnableIsDeadParameter()
        {
            _animator.SetBool("IsDead", true);
        }

        private void EnableHurtParameter(int damage)
        {
            _animator.SetBool("Hurt", true);
        }

        public void DisableHurtParameter(float time)
        {
            Invoke(nameof(DisableHurtParameter), time);
        }

        private void DisableHurtParameter()
        {
            _animator.SetBool("Hurt", false);
        }
    }
}