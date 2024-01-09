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

        private float _musicVolume { get; set; }
        private float _sfxVolume { get; set; }
        private float _masterVolume { get; set; }
        private float _savedMusicVolume { get; set; }
        private float _savedSfxVolume { get; set; }
        private float _savedMasterVolume { get; set; }

        private int _maxVolume = 10;
        
        public event UnityAction<float, float, float> _save = delegate { };

        private void OnEnable()
        {
            //_saveButton.Clicked += SaveVolumes;
            //_resetButton.Clicked += ResetVolumes;
        }

        private void OnDisable()
        {
            ResetVolumes(); // 볼륨 비활성화(값 미지정 시 초기 볼륨 설정)
            //_saveButton.Clicked -= SaveVolumes;
            //_resetButton.Clicked -= ResetVolumes;
        }

        public void Setup(float musicVolume, float sfxVolume, float masterVolume)
        {
            _masterVolume = masterVolume;
            _musicVolume = sfxVolume;
            _sfxVolume = musicVolume;

            _savedMasterVolume = _masterVolume;
            _savedMusicVolume = _musicVolume;
            _savedSfxVolume = _sfxVolume;

            SetMusicVolumeField();
            SetSfxVolumeField();
            SetMasterVolumeField();
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

            _masterVolumeField.FillSettingField(selectedOption);
        }

        public void MasterVolume(float value)
        {
            _masterVolume = Mathf.Round(value * 10) * 0.1f;
            SetMasterVolumeField();
        }
        #endregion

        #region MusicVolume
        private void SetMusicVolumeField()
        {
            string selectedOption = Mathf.RoundToInt(_maxVolume * _musicVolume).ToString();

            SetMusicVolume();

            _musicVolumeField.FillSettingField(selectedOption);
        }

        public void MusicVolume(float value)
        {
            _musicVolume = Mathf.Round(value * 10) * 0.1f;
            SetMusicVolumeField();
        }
        #endregion

        #region SFXVolume
        private void SetSfxVolumeField()
        {
            string selectedOption = Mathf.RoundToInt(_maxVolume * _sfxVolume).ToString();

            SetSfxVolume();

            _sfxVolumeField.FillSettingField(selectedOption);
        }

        public void SfxVolume(float value)
        {
            _sfxVolume = Mathf.Round(value * 10) * 0.1f;
            _SFXVolumeEvent.Invoke(_sfxVolume);
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
    }
}
