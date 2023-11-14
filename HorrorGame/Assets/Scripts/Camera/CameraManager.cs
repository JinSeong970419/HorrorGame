using Cinemachine;
using UnityEngine;

namespace Horror
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera ActorCamera;
        [SerializeField] private CinemachineImpulseSource inpulseSource;

        [SerializeField][Range(.5f, 3f)] private float _speedMultiplier = 1f;
        [SerializeField] private TransformAnchor _cameraTransformAnchor;
        [SerializeField] private TransformAnchor _protagonistTransformAnchor;

        [SerializeField] private GameEventVoid callActor;
        bool test = false;

        private void OnEnable()
        {
            inputReader.CameraMoveEvent += OnCameraMove;
            _protagonistTransformAnchor.onAnchorProvided += SetupProtagonistVirtualCamera;
            _cameraTransformAnchor.Provide(mainCamera.transform);

            callActor.AddListener(Test);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            inputReader.CameraMoveEvent -= OnCameraMove;
            _protagonistTransformAnchor.onAnchorProvided -= SetupProtagonistVirtualCamera;

            _cameraTransformAnchor.UnSet();

            callActor.RemoveListener(Test);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Test()
        {
            test = true;
        }

        private float _rotationVelocity;

        float _cinemachineTargetPitch = 0;
        private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (!test)
                return;

            float deviceMultiplier = isDeviceMouse ? 1f : Time.deltaTime;

            _cinemachineTargetPitch += cameraMovement.y * deviceMultiplier * _speedMultiplier;

            _rotationVelocity = cameraMovement.x * deviceMultiplier * _speedMultiplier;

            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -80f, 80f);

            _protagonistTransformAnchor.Value.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            //_protagonistTransformAnchor.Value.gameObject.GetComponentInParent<Protagonist>().transform.eulerAngles += Vector3.up * _rotationVelocity;
            _protagonistTransformAnchor.Value.gameObject.GetComponentInParent<Protagonist>().transform.Rotate(0f, cameraMovement.x, 0f);

        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void SetupProtagonistVirtualCamera()
        {
            Transform target = _protagonistTransformAnchor.Value;

            ActorCamera.Follow = target;
        }
    }
}
