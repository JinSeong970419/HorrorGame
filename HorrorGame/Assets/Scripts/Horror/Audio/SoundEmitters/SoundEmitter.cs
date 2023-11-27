using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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

        #region Play
        /// <summary>
        /// 지정된 위치에 오디오 클립을 재생
        /// </summary>
        /// <param name="clip">재생할 오디오 클립</param>
        /// <param name="hasToLoop">반복 여부</param>
        /// <param name="position">재생 위치</param>
        public void PlayAudioClip(AudioClip clip, bool hasToLoop, Vector3 position = default)
        {
            _audioSource.clip = clip;
            _audioSource.transform.position = position;
            _audioSource.loop = hasToLoop;
            _audioSource.time = 0f; // 타임 재설정
            _audioSource.Play();

            if (!hasToLoop)
            {
                StartCoroutine(FinishedPlay(clip.length));
            }
        }

        IEnumerator FinishedPlay(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            PlayDone();
        }
        #endregion

        #region Fade
        public void FadeMusicIn(AudioClip musicClip, float duration, float startTime = 0f)
        {
            PlayAudioClip(musicClip, true);
            _audioSource.volume = 0f;

            // 음악 전환 시 Fade 효과 적용
            if (startTime <= _audioSource.clip.length)
                _audioSource.time = startTime;
            
            _audioSource.DOFade(80f, duration);
        }

        public float FadeMusicOut(float duration)
        {
            _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

            return _audioSource.time;
        }

        private void OnFadeOutComplete()
        {
            PlayDone();
        }
        #endregion

        #region PlayController
        /// <summary>
        /// 재생 중인 오디오클립 반환
        /// </summary>
        public AudioClip GetClip()
        {
            return _audioSource.clip;
        }

        /// <summary>
        /// 재실행
        /// </summary>
        public void Resume()
        {
            _audioSource.Play();
        }

        /// <summary>
        /// 재생 중인 오디오클립 일시 정지
        /// </summary>
        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// 반복 종료
        /// </summary>
        public void Finish()
        {
            if (_audioSource.loop)
            {
                _audioSource.loop = false;
                float timeRemaining = _audioSource.clip.length - _audioSource.time;
                StartCoroutine(FinishedPlay(timeRemaining));
            }
        }

        public bool IsPlaying()
        {
            return _audioSource.isPlaying;
        }

        public bool IsLooping()
        {
            return _audioSource.loop;
        }
        #endregion

        private void PlayDone()
        {
            // AudioManager에서 처리
            OnSoundFinishedPlaying.Invoke(this);
        }
    }
}
