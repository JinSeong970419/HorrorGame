using System;
using UnityEngine;

namespace Horror
{
    /// <summary>
    /// 플레이어의 입력 처리 및 값 저장, 상태 머신에서 Data 처리 진행
    /// </summary>
    public class Protagonist : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        private Vector2 _inputVector;
        private float _previousSpeed;

        // StateMachine Action에 사용되는 값
        [NonSerialized] public Vector3 movementInput;
        [NonSerialized] public Vector3 movementVector;
        [NonSerialized] public bool isRunning;

        private void OnEnable()
        {
            _inputReader.MoveEvent += OnMove;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= OnMove;
        }

        private void Update()
        {
            RecalculateMovement();
        }

        /// <summary>
        /// Player 이동 계산
        /// </summary>
        private void RecalculateMovement()
        {
            float targetSpeed;
            Vector3 adjustedMovement;

            adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);

            targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
            if(targetSpeed > 0f)
            {
                if (isRunning)
                    targetSpeed = 1f;
            }
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

            movementInput = adjustedMovement.normalized * targetSpeed;

            _previousSpeed = targetSpeed;
        }

        #region Event PlayerInput
        // Event PlayerInput
        private void OnMove(Vector2 movement) { _inputVector = movement; }
        #endregion
    }
}
