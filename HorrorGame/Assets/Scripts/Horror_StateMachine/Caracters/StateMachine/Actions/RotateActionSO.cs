using UnityEngine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
{
    [CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate")]
    public class RotateActionSO : StateActionSO<RotateAction>
    {
        public float turnSmoothTime = 0.2f;
    }

    public class RotateAction : StateAction
    {
        private Protagonist _protagonistScript;
        private Transform _transform;
        private Rigidbody _Rigidbody;

        private float _turnSmoothSpeed;
        private const float ROTATION_TRESHOLD = .02f;
        private new RotateActionSO OriginSO => (RotateActionSO)base.OriginSO;

        public override void Awake(StateMachine stateMachine)
        {
            _protagonistScript = stateMachine.GetComponent<Protagonist>();
            _transform = stateMachine.GetComponent<Transform>();
            _Rigidbody = stateMachine.GetComponent<Rigidbody>();
        }

        public override void OnUpdate()
        {
            Vector3 horizontalMovement = _protagonistScript.camreaVector;
            horizontalMovement.y = 0f;

            if (horizontalMovement.sqrMagnitude >= ROTATION_TRESHOLD)
            {
                float targetRotation = Mathf.Atan2(_protagonistScript.camreaVector.x, _protagonistScript.camreaVector.y) * Mathf.Rad2Deg;
                _transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                    _transform.eulerAngles.y,
                    targetRotation,
                    ref _turnSmoothSpeed,
                    OriginSO.turnSmoothTime);
                _Rigidbody.MoveRotation(_Rigidbody.rotation * Quaternion.Euler(_transform.eulerAngles));
            }
        }
    }
}
