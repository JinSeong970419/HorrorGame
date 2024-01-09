using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.Universal;

namespace Horror
{
    public class SettingSystem : MonoBehaviour
    {
        [Header("Save Setting Event")]
        [SerializeField] private GameEventVoid SaveSettingsEvent;

        [Header("Current Setting")]
        [SerializeField] private SettingSO _currentSetting;
        [SerializeField] private UniversalRenderPipelineAsset _urpRenderAsset;
        [SerializeField] private SaveSystem _saveSystem;

        [Header("Volume Data Event")]
        [SerializeField] private GameEventFloat _MasterVolumeEvent;
        [SerializeField] private GameEventFloat _MusicVolumeEvent;
        [SerializeField] private GameEventFloat _SFXVolumeEvent;

        private void Awake()
        {
            _saveSystem.LoadSaveDataFromDisk();
            _currentSetting.LoadSavedSettings(_saveSystem.saveData);
            SetCurrentSettings();
        }
        private void OnEnable()
        {
            SaveSettingsEvent.AddListener(SaveSettings);
        }
        private void OnDisable()
        {
            SaveSettingsEvent.RemoveListener(SaveSettings);
        }

        /// <summary>
        /// 현재 설정 세팅
        /// </summary>
        void SetCurrentSettings()
        {
            _MasterVolumeEvent.Invoke(_currentSetting.MasterVolume);
            _MusicVolumeEvent.Invoke(_currentSetting.MusicVolume);
            _SFXVolumeEvent.Invoke(_currentSetting.SfxVolume);

            Resolution currentresolution = Screen.currentResolution; // 현재 해상도 읽기

            if (_currentSetting.ResolutionsIndex < Screen.resolutions.Length)
                currentresolution = Screen.resolutions[_currentSetting.ResolutionsIndex];

            Screen.SetResolution(currentresolution.width, currentresolution.height, _currentSetting.IsFullscreen);
            _urpRenderAsset.shadowDistance = _currentSetting.ShadowDistance;
            _urpRenderAsset.msaaSampleCount = _currentSetting.AntiAliasingIndex;
            
            LocalizationSettings.SelectedLocale = _currentSetting.CurrentLocale;
        }

        void SaveSettings()
        {
            _saveSystem.SaveDataToDisk();
        }
    }
}
