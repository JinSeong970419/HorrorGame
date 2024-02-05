using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Horror
{
    [CreateAssetMenu(fileName = "Setting", menuName = "Settings/new setting")]
    public class SettingSO : ScriptableObject
    {
        [Header("Audio")]
        [SerializeField] float _masterVolume;
        [SerializeField] float _musicVolume;
        [SerializeField] float _sfxVolume;

        [Header("Resolution")]
        [SerializeField] int _resolutionsIndex;
        [SerializeField] int _antiAliasingIndex;
        [SerializeField] float _shadowDistance;
        [SerializeField] bool _isFullscreen;
        [SerializeField] Locale _currentLocale;

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;
        public int ResolutionsIndex => _resolutionsIndex;
        public int AntiAliasingIndex => _antiAliasingIndex;
        public float ShadowDistance => _shadowDistance;
        public bool IsFullscreen => _isFullscreen;
        public Locale CurrentLocale => _currentLocale;

        public void LoadSavedSettings(Save savedFile)
        {
            _masterVolume = savedFile._masterVolume;
            _musicVolume = savedFile._musicVolume;
            _sfxVolume = savedFile._sfxVolume;
            _resolutionsIndex = savedFile._resolutionsIndex;
            _antiAliasingIndex = savedFile._antiAliasingIndex;
            _shadowDistance = savedFile._shadowDistance;
            _isFullscreen = savedFile._isFullscreen;
            _currentLocale = savedFile._currentLocale;
        }

        #region Setting Save
        public void SaveLanguageSettings(Locale local)
        {
            _currentLocale = local;
        }

        public void SaveAudioSettings(float newMusicVolume, float newSfxVolume, float newMasterVolume)
        {
            _masterVolume = newMasterVolume;
            _musicVolume = newMusicVolume;
            _sfxVolume = newSfxVolume;
        }

        public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
        {
            _resolutionsIndex = newResolutionsIndex;
            _antiAliasingIndex = newAntiAliasingIndex;
            _shadowDistance = newShadowDistance;
            _isFullscreen = fullscreenState;
        }
        #endregion
    }
}
