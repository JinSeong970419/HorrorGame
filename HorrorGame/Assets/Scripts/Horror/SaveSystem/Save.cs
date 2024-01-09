using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Horror
{
    [Serializable]
    public class Save
    {
        public string _locationId;

        public float _masterVolume;
        public float _musicVolume;
        public float _sfxVolume;
        public int _resolutionsIndex;
        public int _antiAliasingIndex;
        public float _shadowDistance;
        public bool _isFullscreen;
        public Locale _currentLocale;

        public void SaveSettings(SettingSO settings)
        {
            _masterVolume = settings.MasterVolume;
            _musicVolume = settings.MusicVolume;
            _sfxVolume = settings.SfxVolume;

            _resolutionsIndex = settings.ResolutionsIndex;
            _antiAliasingIndex = settings.AntiAliasingIndex;
            _shadowDistance = settings.ShadowDistance;
            _isFullscreen = settings.IsFullscreen;
            _currentLocale = settings.CurrentLocale;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
