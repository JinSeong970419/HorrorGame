using PlasticGui.WorkspaceWindow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(fileName = "Save", menuName = "Save/new Save")]
    public class SaveSystem : ScriptableObject
    {
        [SerializeField] private GameEventVoid _saveSettingsEvent;
        [SerializeField] private LoadGameEvent _loadLocation;
        [SerializeField] private SettingSO _currentSettings;

        public string saveFilename = "save.HorrorGame";
        public string backupSaveFilename = "save.HorrorGame.bak";
        public Save saveData = new();

        private void OnEnable()
        {
            _saveSettingsEvent?.AddListener(SaveSettings);
            _loadLocation.OnLoadingRequested += CacheLoadLocations;
        }

        private void OnDisable()
        {
            _saveSettingsEvent.RemoveListener(SaveSettings);
            _loadLocation.OnLoadingRequested -= CacheLoadLocations;
        }

        private void CacheLoadLocations(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            LocationSO locationSO = locationToLoad as LocationSO;
            if (locationSO)
            {
                saveData._locationId = locationSO.Guid;
            }

            SaveDataToDisk();
        }

        #region Save
        public void SaveDataToDisk()
        {
            if (FileManager.MoveFile(saveFilename, backupSaveFilename))
            {
                if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
                {
                    Debug.Log("Save successful " + saveFilename);
                }
            }
        }

        private void SaveSettings()
        {
            saveData.SaveSettings(_currentSettings);
        }
        #endregion

        #region Load
        public bool LoadSaveDataFromDisk()
        {
            if (FileManager.LoadFromFile(saveFilename, out var json))
            {
                saveData.LoadFromJson(json);
                return true;
            }

            return false;
        }
        #endregion

        #region Write
        public void WriteEmptySaveFile()
        {
            FileManager.WriteToFile(saveFilename, "");
        }

        public void SetNewGameData()
        {
            FileManager.WriteToFile(saveFilename, "");

            /*
             * 추 후 퀘스트 정보 등 데이터 저장
             */

            SaveDataToDisk();
        }
        #endregion
    }
}
