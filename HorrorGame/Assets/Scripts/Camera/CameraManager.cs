using Cinemachine;
using UnityEngine;

namespace Horror
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private CinemachineImpulseSource inpulseSource;

        [SerializeField][Range(.5f, 3f)] private float _speedMultiplier = 1f;
        [SerializeField] private TransformAnchor _cameraTransformAnchor;
        [SerializeField] private TransformAnchor _protagonistTransformAnchor;

        private void OnEnable()
        {
            inputReader.CameraMoveEvent += OnCameraMove;
            _protagonistTransformAnchor.onAnchorProvided += SetupProtagonistVirtualCamera;
            _cameraTransformAnchor.Provide(mainCamera.transform);
        }

        private void OnDisable()
        {
            inputReader.CameraMoveEvent -= OnCameraMove;
            _protagonistTransformAnchor.onAnchorProvided -= SetupProtagonistVirtualCamera;

            _cameraTransformAnchor.UnSet();
        }

        private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
        {
            float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;

            freeLookCamera.m_XAxis.m_InputAxisValue = cameraMovement.x * deviceMultiplier * _speedMultiplier;
            freeLookCamera.m_YAxis.m_InputAxisValue = cameraMovement.y * deviceMultiplier * _speedMultiplier;
        }

        public void SetupProtagonistVirtualCamera()
        {
            Transform target = _protagonistTransformAnchor.Value;

            freeLookCamera.Follow = target;
            freeLookCamera.LookAt = target;
            freeLookCamera.OnTargetObjectWarped(target, target.position - freeLookCamera.transform.position - Vector3.forward);
        }
    }
}
