using UnityEngine;
using UnityEngine.Audio;

namespace Horror
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Sound Emitters Pool")]
        [SerializeField] private SoundEmitterPoolSO _pool;
        [SerializeField] private int _initialSize = 10;

        [Header("Audio controller")]
        [SerializeField] private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] private float _masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        private void Awake()
        {
            _pool.Prewarm(_initialSize);
            _pool.SetParent(this.transform);
        }

    }
}
