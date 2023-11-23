using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;

        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
    }
}
