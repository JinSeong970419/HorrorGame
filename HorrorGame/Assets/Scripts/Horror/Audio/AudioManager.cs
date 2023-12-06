using UnityEngine;
using UnityEngine.Audio;

namespace Horror
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Sound Emitters Pool Setting")]
        [SerializeField] private SoundEmitterPoolSO _pool;
        [SerializeField] private int _initialSize = 10;

        [Header("Audio Controller")]
        [SerializeField] private AudioMixer audioMixer;
        [Range(0f, 1f)]
        [SerializeField] private float _masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        [Header("Listening Event")]
        [SerializeField] private GameEventAudioSignal _SFXEventSignal;
        [SerializeField] private GameEventAudioSignal _musicEventSignal;
        [SerializeField] private GameEventFloat _SFXVolumeEventFloat;
        [SerializeField] private GameEventFloat _musicVolumeEventFloat;
        [SerializeField] private GameEventFloat _masterVolumeEventFloat;

        private SoundEmitter _musicSoundEmitter;
        private SoundEmitterVault _soundEmitterVault;

        void OnValidate()
        {
            if (Application.isPlaying)
            {
                SetGroupVolume("MasterVolume", _masterVolume);
                SetGroupVolume("MusicVolume", _musicVolume);
                SetGroupVolume("SFXVolume", _sfxVolume);
            }
        }

        private void OnEnable()
        {
            _SFXEventSignal.OnAudioSignalPlay += PlayAudioSignal;
            _SFXEventSignal.OnAudioSignalStop += StopAudioSignal;
            _SFXEventSignal.OnAudioSignalFinish += FinishAudioSignal;

            _musicEventSignal.OnAudioSignalPlay += PlayMusicTrack;
            _musicEventSignal.OnAudioSignalStop += StopMusic;

            _SFXVolumeEventFloat.AddListener(ChangeSFXVolume);
            _musicVolumeEventFloat.AddListener(ChangeMusicVolume);
            _masterVolumeEventFloat.AddListener(ChangeMasterVolume);
        }

        private void OnDisable()
        {
            _SFXEventSignal.OnAudioSignalPlay -= PlayAudioSignal;
            _SFXEventSignal.OnAudioSignalStop -= StopAudioSignal;
            _SFXEventSignal.OnAudioSignalFinish -= FinishAudioSignal;

            _musicEventSignal.OnAudioSignalPlay -= PlayMusicTrack;
            _musicEventSignal.OnAudioSignalStop -= StopMusic;

            _SFXVolumeEventFloat.RemoveListener(ChangeSFXVolume);
            _musicVolumeEventFloat.RemoveListener(ChangeMusicVolume);
            _masterVolumeEventFloat.RemoveListener(ChangeMasterVolume);
        }

        private void Awake()
        {
            // 초기 볼륨 설정
            _soundEmitterVault = new SoundEmitterVault();

            _pool.Prewarm(_initialSize);
            _pool.SetParent(this.transform);
        }

        void ChangeMasterVolume(float newVolume)
        {
            _masterVolume = newVolume;
            SetGroupVolume("MasterVolume", _masterVolume);
        }
        void ChangeMusicVolume(float newVolume)
        {
            _musicVolume = newVolume;
            SetGroupVolume("MusicVolume", _musicVolume);
        }
        void ChangeSFXVolume(float newVolume)
        {
            _sfxVolume = newVolume;
            SetGroupVolume("SFXVolume", _sfxVolume);
        }

        public void SetGroupVolume(string parameterName, float normalizedVolume)
        {
            bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
            if (!volumeSet)
                Debug.LogError("AudioMixer 확인");
        }

        public float GetGroupVolume(string parameterName)
        {
            if (audioMixer.GetFloat(parameterName, out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("The AudioMixer 확인");
                return 0f;
            }
        }

        private float MixerValueToNormalized(float mixerValue)
        {
            // -80dB to 0dB을 0 ~ 1까지 값으로 변환
            return 1f + (mixerValue / 80f);
        }
        private float NormalizedToMixerValue(float normalizedValue)
        {
            // 0 ~ 1 범위가 -80dB ~ 0dB을 초과하는 값 방지
            return (normalizedValue - 1f) * 80f;
        }

        /// <summary>
        /// Pool에서 SoundEmitter 요청 및 Audio 재생
        /// </summary>
        public AudioSignalKey PlayAudioSignal(AudioSignalSO audioSignal, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clipsPlay = audioSignal.GetClips();
            SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsPlay.Length];

            int nClisps = clipsPlay.Length;
            for (int i = 0; i < nClisps; i++)
            {
                soundEmitterArray[i] = _pool.Request();
                if (soundEmitterArray[i] != null)
                {
                    soundEmitterArray[i].PlayAudioClip(clipsPlay[i], settings, audioSignal.looping, position);
                    if (!audioSignal.looping)
                        soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlay;
                }
            }

            return _soundEmitterVault.Add(audioSignal, soundEmitterArray);
        }

        public bool StopAudioSignal(AudioSignalKey audioSignalKey)
        {
            bool isFound = _soundEmitterVault.Get(audioSignalKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0;i < soundEmitters.Length; i++)
                {
                    StopAndCleanEmitter(soundEmitters[i]);
                }

                _soundEmitterVault.Remove(audioSignalKey);
            }

            return isFound;
        }

        public bool FinishAudioSignal(AudioSignalKey audioSignalKey)
        {
            bool isFound = _soundEmitterVault.Get(audioSignalKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    soundEmitters[i].Finish();
                    soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlay;
                }
            }
            else
            {
                Debug.LogWarning("Key에 해당하는 Audio가 존재하지 않음");
            }

            return isFound;
        }

        private AudioSignalKey PlayMusicTrack(AudioSignalSO audioSignal, AudioConfigurationSO settings, Vector3 PosigionInSpace)
        {
            float fadeDuration = 2f;
            float startTime = 0f;

            if(_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
            {
                AudioClip songPlay = audioSignal.GetClips()[0];
                if (_musicSoundEmitter.GetClip() == songPlay)
                    return AudioSignalKey.initialize;

                // 재생 중인 음악 FadeOut
                startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);
            }

            _musicSoundEmitter = _pool.Request();
            _musicSoundEmitter.FadeMusicIn(audioSignal.GetClips()[0], settings, 1f, startTime);
            _musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

            return AudioSignalKey.initialize;
        }

        private bool StopMusic(AudioSignalKey key)
        {
            if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
            {
                _musicSoundEmitter.Stop();
                return true;
            }
            else
                return false;
        }

        private void OnSoundEmitterFinishedPlay(SoundEmitter soundEmitter)
        {
            StopAndCleanEmitter(soundEmitter);
        }

        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (!soundEmitter.IsLooping())
                soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlay;

            soundEmitter.Stop();
            _pool.Return(soundEmitter);
        }

        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
            _pool.Return(soundEmitter);
        }
    }
}
