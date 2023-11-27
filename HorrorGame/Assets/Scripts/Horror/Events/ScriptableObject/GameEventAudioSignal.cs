using UnityEngine;

namespace Horror
{
    public delegate AudioSignalKey AudioSignalPlayAction(AudioSignalSO audioCue, Vector3 positionInSpace);
    public delegate bool AudioSignalStopAction(AudioSignalKey emitterKey);
    public delegate bool AudioSignalFinishAction(AudioSignalKey emitterKey);

    /// <summary>
    /// 사운드 재생 메시지를 보내는 이벤트 AudioManager에서 이벤트를 처리 및 재생
    /// </summary>
    [CreateAssetMenu(menuName = "Game Event/Event AudioSignal", order = 12)]
    public class GameEventAudioSignal : DescriptionBaseSO
    {
        public AudioSignalPlayAction OnAudioSignalPlay;
        public AudioSignalStopAction OnAudioSignalStop;
        public AudioSignalFinishAction OnAudioSignalFinish;

        public AudioSignalKey Invoke(AudioSignalSO audioSignal,  Vector3 positionInSpace = default)
        {
            AudioSignalKey audioSignalKey = AudioSignalKey.initialize;

            if (OnAudioSignalPlay != null)
            {
                audioSignalKey = OnAudioSignalPlay.Invoke(audioSignal, positionInSpace);
            }
            else
            {
                Debug.LogWarning($"{audioSignal.name}을 AudioManager에서 확인 불가능");
            }

            return audioSignalKey;
        }

        public bool StopInvoke(AudioSignalKey audioCueKey)
        {
            bool requestSucceed = false;

            if (OnAudioSignalStop != null)
            {
                requestSucceed = OnAudioSignalStop.Invoke(audioCueKey);
            }
            else
            {
                Debug.LogWarning($"AudioManager에서 확인 불가능");
            }

            return requestSucceed;
        }

        public bool FinishInvoke(AudioSignalKey audioCueKey)
        {
            bool requestSucceed = false;

            if (OnAudioSignalFinish != null)
            {
                requestSucceed = OnAudioSignalFinish.Invoke(audioCueKey);
            }
            else
            {
                Debug.LogWarning($"AudioManager에서 확인 불가능");
            }

            return requestSucceed;
        }
    }
}
