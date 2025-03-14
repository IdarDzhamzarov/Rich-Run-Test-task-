using ButchersGames;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float sensitivity;
        [SerializeField] private float maxRotationSpeed;
        [SerializeField] private float trackWidth = 3f;

        private Pathway pathWay;

        private Transform _origin;
        private float _rotation;

        private void Awake()
        {
            _origin = transform.parent;
        }

        public void ResetToStart(Level level)
        {
            pathWay = level.transform.GetComponentInChildren<Pathway>();

            _origin.position = level.PlayerSpawnPoint;
            _origin.rotation = Quaternion.identity;
            _rotation = 0;

            transform.localPosition = Vector3.zero;
            pathWay.Reset();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.isGameStart) return;

            var targetRotation = pathWay.GetTargetRotation(_origin.position);
            var targetDeltaRotation = targetRotation - _rotation;

            if (Mathf.Abs(targetDeltaRotation) < maxRotationSpeed) _rotation = targetRotation;
            else _rotation += Mathf.Sign(targetDeltaRotation) * maxRotationSpeed;

            _origin.rotation = Quaternion.AngleAxis(_rotation, Vector3.up);
            _origin.Translate(Time.fixedDeltaTime * speed * Vector3.forward);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var gameManager = GameManager.Instance;
            var value = context.ReadValue<float>();

            if (gameManager.waitingForSwipe && value > 0) gameManager.StartGame();
            if (!gameManager.isGameStart) return;

            var targetDeltaPosition = value * sensitivity;
            var targetPosition = transform.localPosition.x + targetDeltaPosition;

            if (targetPosition > trackWidth / 2f || targetPosition < -trackWidth / 2f) return;
            transform.Translate(targetDeltaPosition * Vector3.right);
        }
    }
}
