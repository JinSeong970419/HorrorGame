using UnityEngine;

namespace Horror
{
    public class MusicBox : MonoBehaviour
    {
        [SerializeField] private GameEventVoid _onCallSpawn;
        [SerializeField] private GameEventAudioSignal _onPlayMusic;
        [SerializeField] private GameSceneSO _thisSceneSO;
        [SerializeField] private AudioConfigurationSO _settings;

        private void OnEnable()
        {
            _onCallSpawn.AddListener(PlayMusic);
        }

        private void OnDisable()
        {
            _onCallSpawn.RemoveListener(PlayMusic);
        }

        private void PlayMusic()
        {
            _onPlayMusic.Invoke(_thisSceneSO.musicTrack, _settings);
        }
    }
}
