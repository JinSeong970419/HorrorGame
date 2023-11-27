using UnityEngine;

namespace Horror
{
    public class AudioListener : MonoBehaviour
    {
        // 사운드 방향을 계산 및 반환
        [SerializeField] private TransformAnchor _cameraTransform;

        void LateUpdate()
        {
            if (_cameraTransform.isSet)
                transform.forward = _cameraTransform.Value.forward;
        }
    }
}
