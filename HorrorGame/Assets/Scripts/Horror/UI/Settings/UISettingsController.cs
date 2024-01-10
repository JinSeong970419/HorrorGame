using Horror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Horror
{
    [System.Serializable]
    public enum SettingFieldType
    {
        Language,
        Volume_SFX,
        Volume_Music,
        Resolution,
        FullScreen,
        ShadowDistance,
        AntiAliasing,
        ShadowQuality,
        Volume_Master,

    }
    [System.Serializable]
    public class SettingTab
    {
        public SettingsType settingTabsType;
        public LocalizedString title;
    }

    [System.Serializable]
    public class SettingField
    {
        public SettingsType settingTabsType;
        public SettingFieldType settingFieldType;
        public LocalizedString title;
    }

    public enum SettingsType
    {
        Language,
        Graphics,
        Audio,
    }
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField] private UISettingsLanguage _languageComponent;
        //[SerializeField] private UISettingsGraphicsComponent _graphicsComponent;
        [SerializeField] private UISettingsAudio _audioComponent;
        //[SerializeField] private UISettingTabsFiller _settingTabFiller = default;
        [SerializeField] private SettingSO _currentSetting;
        [SerializeField] private List<SettingsType> _settingTabsList = new List<SettingsType>();
        private SettingsType _selectedTab = SettingsType.Audio;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameEventVoid SaveSettingsEvent;
        public UnityAction Closed;

        private void OnEnable()
        {
            _languageComponent._save += SaveLaguageSettings;
            _audioComponent._save += SaveAudioSettings;
            //_graphicsComponent._save += SaveGraphicsSettings;

            _inputReader.MenuCloseEvent += CloseScreen;
            //_inputReader.TabSwitched += SwitchTab;

            //_settingTabFiller.FillTabs(_settingTabsList);
            //_settingTabFiller.ChooseTab += OpenSetting;

            OpenSetting(SettingsType.Audio);

        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
            //_inputReader.TabSwitched -= SwitchTab;

            _languageComponent._save -= SaveLaguageSettings;
            _audioComponent._save -= SaveAudioSettings;
            //_graphicsComponent._save -= SaveGraphicsSettings;
        }

        public void CloseScreen()
        {
            Closed.Invoke();
        }

        private void OpenSetting(SettingsType settingType)
        {
            _selectedTab = settingType;
            switch (settingType)
            {
                case SettingsType.Language:
                    _currentSetting.SaveLanguageSettings(_currentSetting.CurrentLocale);
                    break;
                //case SettingsType.Graphics:
                //    _graphicsComponent.Setup();
                    break;
                case SettingsType.Audio:
                    _audioComponent.Setup(_currentSetting.MusicVolume, _currentSetting.SfxVolume, _currentSetting.MasterVolume);
                    break;
                default:
                    break;
            }

            //_languageComponent.gameObject.SetActive(settingType == SettingsType.Language);
            //_graphicsComponent.gameObject.SetActive((settingType == SettingsType.Graphics));
            //_audioComponent.gameObject.SetActive(settingType == SettingsType.Audio);
            //_settingTabFiller.SelectTab(settingType);
        }

        private void SwitchTab(float orientation)
        {

            if (orientation != 0)
            {
                bool isLeft = orientation < 0;
                int initialIndex = _settingTabsList.FindIndex(o => o == _selectedTab);
                if (initialIndex != -1)
                {
                    if (isLeft)
                    {
                        initialIndex--;
                    }
                    else
                    {
                        initialIndex++;
                    }

                    initialIndex = Mathf.Clamp(initialIndex, 0, _settingTabsList.Count - 1);
                }
                OpenSetting(_settingTabsList[initialIndex]);
            }
        }

        public void SaveLaguageSettings(Locale local)
        {
            _currentSetting.SaveLanguageSettings(local);
            SaveSettingsEvent.Invoke();
        }

        void SaveAudioSettings(float musicVolume, float sfxVolume, float masterVolume)
        {
            _currentSetting.SaveAudioSettings(musicVolume, sfxVolume, masterVolume);
            SaveSettingsEvent.Invoke();
        }
    }
}