using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    public class UISettingsAudio : MonoBehaviour
    {
        [Header("VolumeField")]
        [SerializeField] private UISettingItemFiller _masterVolumeField;
        [SerializeField] private UISettingItemFiller _musicVolumeField;
        [SerializeField] private UISettingItemFiller _sfxVolumeField;

        [Header("Broadcasting")]
        [SerializeField] private GameEventFloat _masterVolumeEvent;
        [SerializeField] private GameEventFloat _musicVolumeEvent;
        [SerializeField] private GameEventFloat _SFXVolumeEvent;

        private float _masterVolume { get; set; }
        private float _musicVolume { get; set; }
        private float _sfxVolume { get; set; }
        private float _savedMasterVolume { get; set; }
        private float _savedMusicVolume { get; set; }
        private float _savedSfxVolume { get; set; }

        private int _maxVolume = 10;
        
        public event UnityAction<float, float, float> _save = delegate { };

        public void Setup(float musicVolume, float sfxVolume, float masterVolume)
        {
            _masterVolume = masterVolume;
            _musicVolume = musicVolume;
            _sfxVolume = sfxVolume;

            _savedMasterVolume = _masterVolume;
            _savedMusicVolume = _musicVolume;
            _savedSfxVolume = _sfxVolume;

            SetMasterVolumeField();
            SetMusicVolumeField();
            SetSfxVolumeField();
        }

        private void ResetVolumes()
        {
            Setup(_savedMusicVolume, _savedSfxVolume, _savedMasterVolume);
        }

        #region MasterVolume
        private void SetMasterVolumeField()
        {
            string selectedOption = Mathf.RoundToInt(_maxVolume * _masterVolume).ToString();

            SetMasterVolume();

            _masterVolumeField.FillSettingFieldSlider(selectedOption, _masterVolume);
        }

        public void MasterVolume(float value)
        {
            _masterVolume = Mathf.Round(value * 10) * 0.1f;
            SetMasterVolumeField();
            SaveVolume();
        }
        #endregion

        #region MusicVolume
        private void SetMusicVolumeField()
        {
            string selectedOption = Mathf.RoundToInt(_maxVolume * _musicVolume).ToString();

            SetMusicVolume();

            _musicVolumeField.FillSettingFieldSlider(selectedOption, _musicVolume);
        }

        public void MusicVolume(float value)
        {
            _musicVolume = Mathf.Round(value * 10) * 0.1f;
            SetMusicVolumeField();
            SaveVolume();
        }
        #endregion

        #region SFXVolume
        private void SetSfxVolumeField()
        {
            string selectedOption = Mathf.RoundToInt(_maxVolume * _sfxVolume).ToString();

            SetSfxVolume();

            _sfxVolumeField.FillSettingFieldSlider(selectedOption, _sfxVolume);
        }

        public void SfxVolume(float value)
        {
            _sfxVolume = Mathf.Round(value * 10) * 0.1f;
            SetSfxVolumeField();
            SaveVolume();
        }
        #endregion


        #region Setting Volume
        private void SetMusicVolume()
        {
            _masterVolumeEvent.Invoke(_musicVolume);
        }
        private void SetSfxVolume()
        {
            _SFXVolumeEvent.Invoke(_sfxVolume);
        }
        private void SetMasterVolume()
        {
            _musicVolumeEvent.Invoke(_masterVolume);
        }
        #endregion

        private void SaveVolume()
        {
            _savedMasterVolume = _masterVolume;
            _savedMusicVolume = _musicVolume;
            _savedSfxVolume = _sfxVolume;

            _save.Invoke(_musicVolume, _sfxVolume, _masterVolume);
        }
    }
}
