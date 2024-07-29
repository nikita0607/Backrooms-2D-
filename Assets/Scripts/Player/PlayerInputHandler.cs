using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BHSCamp
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _sprintSpeed;

        [Header ("Stamina settings")]
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _staminaPerSecond;
        [SerializeField] private float _staminaRegenerationTime;
        [SerializeField] private Image _staminaImage;

        private float _horizontal;
        private IMove _movable;
        private IJump _jump;
        private Health _health;
        private PlayerAnimation _animation;
        private Ground _ground;
        private bool _isDead;
        private Interaction _inreraction;

        private float _stamina;

        private void OnEnable()
        {   
            _health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= HandleDeath;
        }

        private void Awake()
        {
            _movable = GetComponent<IMove>();
            _jump = GetComponent<IJump>();
            _animation = GetComponent<PlayerAnimation>();
            _ground = GetComponent<Ground>();
            _health = GetComponent<Health>();
            _inreraction = GetComponent<Interaction>();
        }

        private void Start() {
            _stamina = _maxStamina;
        }

        private void Update()
        {
            if (_isDead || Time.timeScale == 0) {
                return;
            }

            // _staminaImage.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);
            _staminaImage.transform.eulerAngles = Vector3.zero;
            _staminaImage.fillAmount = _stamina/_maxStamina;

            _horizontal = Input.GetAxisRaw("Horizontal");
            bool isSprinting = Input.GetButton("Sprint");

            // _horizontal = _ground.OnGround ? 0 : _horizontal;

            float speed = isSprinting && _stamina>0 && _ground.OnGround ? _sprintSpeed : _speed;

            _movable.SetVelocity(new Vector2(_horizontal, 0), speed);
            _animation.SetInputX(_horizontal);
            _animation.SetIsRunning(isSprinting && _stamina>0);

            if (isSprinting && _ground.OnGround && _horizontal != 0) {
                _stamina -= Time.deltaTime * _staminaPerSecond;
                if (_stamina < 0) _stamina = 0;
            }
            else {
                _stamina += Time.deltaTime * _maxStamina / _staminaRegenerationTime;
                if (_stamina > _maxStamina) _stamina = _maxStamina;
            }

            if (Input.GetButtonDown("Jump"))
                _jump.Action();
            if (Input.GetButtonDown("Interact"))
                _inreraction.Interract();
        }

        private void HandleDeath()
        {
            _isDead = true;
        }
    }
}