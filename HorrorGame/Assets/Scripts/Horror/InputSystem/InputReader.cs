using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Horror
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : DescriptionBaseSO, PlayerInput.IPlayerActions
    {
        // PlayerInput
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction<Vector2, bool> CameraMoveEvent = delegate { };
        public event UnityAction StartedRunning = delegate { };
        public event UnityAction StoppedRunning = delegate { };

        // Menu
        public event UnityAction MenuMouseMoveEvent = delegate { };
        public event UnityAction MoveSelectionEvent = delegate { };
        public event UnityAction MenuCloseEvent = delegate { };

        private PlayerInput _playerInput;
        private void OnEnable()
        {
            if(_playerInput == null)
            {
                _playerInput = new PlayerInput();

                _playerInput.Player.SetCallbacks(this);
            }

            _playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void DisableAllInput()
        {
            _playerInput.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    StartedRunning.Invoke(); 
                    break;

                case InputActionPhase.Canceled:
                    StoppedRunning.Invoke(); 
                    break;
            }
        }

        public void OnCameraRotate(InputAction.CallbackContext context)
        {
            CameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }
        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
    }
}
