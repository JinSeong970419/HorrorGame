using System;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(fileName = "newAudioSignal", menuName = "Audio/Audio Signal")]
    public class AudioSignalSO : ScriptableObject
    {
        public bool looping = false;
        [SerializeField] private AudioClipsGroup[] _audioClipGroups;

        public AudioClip[] GetClips()
        {
            int clipsNumber = _audioClipGroups.Length;
            AudioClip[] resultClips = new AudioClip[clipsNumber];

            for (int i = 0; i < clipsNumber; i++)
            {
                resultClips[i] = _audioClipGroups[i].GetNextClip();
            }

            return resultClips;
        }
    }

    [Serializable]
    public class AudioClipsGroup
    {
        public enum SequenceMode
        {
            Random,
            RandomNoImmediateRepeat,
            Sequential,
        }

        public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] audioClips;

        private int _nextClipPlay = -1;
        private int _lastClipPlay = -1;

        /// <summary>
        /// SequenceMode에 따라 다음 클립을 선택
        /// </summary>
        public AudioClip GetNextClip()
        {
            // 재생 클립이 하나일 경우
            if (audioClips.Length == 1)
                return audioClips[0];

            if (_nextClipPlay == -1)
            {
                // 인덱스 초기화시 SequenceMode에 따라 진행
                _nextClipPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
            }
            else
            {
                // 여러개 재생 클립 중 SequenceMode에 따라 실행
                switch (sequenceMode)
                {
                    case SequenceMode.Random:
                        _nextClipPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        break;

                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            _nextClipPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } while (_nextClipPlay == _lastClipPlay);
                        break;

                    case SequenceMode.Sequential:
                        _nextClipPlay = (int)Mathf.Repeat(++_nextClipPlay, audioClips.Length);
                        break;
                }
            }

            _lastClipPlay = _nextClipPlay;

            return audioClips[_nextClipPlay];
        }
    }
}
