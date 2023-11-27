using System.Collections;
using UnityEngine;

namespace Horror
{
    /// <summary>
    /// 재생할 Sound 요청, AudioManager에서 처리
    /// </summary>
    public class AudioAction : MonoBehaviour
    {
        [Header("Sound definition")]
        [SerializeField] private AudioSignalSO _audioSignal;
        [SerializeField] private bool _playOnStart;

        [Header("Configuration")]
        [SerializeField] private GameEventAudioSignal _audioSFXEvent;

        private AudioSignalKey controlKey = AudioSignalKey.initialize;

        private void Start()
        {
            if (_playOnStart)
                StartCoroutine(PlayDelayed());
        }

        private void OnDisable()
        {
            _playOnStart = false;
            StopAudioSignal();
        }

        private IEnumerator PlayDelayed()
        {
            // 대기 시간을 통해 AudioManager에서 재생 요청을 처리할 준비
            yield return new WaitForSeconds(1f);

            if (_playOnStart)
                PlayAudioCue();
        }

        public void PlayAudioCue()
        {
            controlKey = _audioSFXEvent.Invoke(_audioSignal, transform.position);
        }

        public void StopAudioSignal()
        {
            if (controlKey != AudioSignalKey.initialize)
            {
                if (!_audioSFXEvent.StopInvoke(controlKey))
                {
                    controlKey = AudioSignalKey.initialize;
                }
            }
        }

        public void FinishAudioCue()
        {
            if (controlKey != AudioSignalKey.initialize)
            {
                if (!_audioSFXEvent.FinishInvoke(controlKey))
                {
                    controlKey = AudioSignalKey.initialize;
                }
            }
        }
    }
}
