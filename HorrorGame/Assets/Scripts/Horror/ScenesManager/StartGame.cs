using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Horror
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _locationsToLoad;
        [SerializeField] private SaveSystem _saveSystem;
        [SerializeField] private bool _showLoadScreen;

        [Header("Broadcasting On")]
        [SerializeField] private LoadGameEvent _loadLocation;

        [Header("Listening to")]
        [SerializeField] private GameEventVoid _onNewGameButton;
        [SerializeField] private GameEventVoid _onContinueButton;

        private bool _hasSaveData;
        
        private void Start()
        {
            _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            _onNewGameButton?.AddListener(startNewGame);
            _onContinueButton?.AddListener(ContinuePreviousgame);
        }
        #region New Game
        private void startNewGame()
        {
            _hasSaveData = false;

            _saveSystem.WriteEmptySaveFile();
            _saveSystem.SetNewGameData();

            _loadLocation.Invoke(_locationsToLoad, _showLoadScreen);
        }
        #endregion

        #region Continue Game
        private void ContinuePreviousgame()
        {
            StartCoroutine(LoadSaveGame());
        }

        private IEnumerator LoadSaveGame()
        {
            var locationGuid = _saveSystem.saveData._locationId;
            var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

            yield return asyncOperationHandle;

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                LocationSO locationSO = asyncOperationHandle.Result;
                _loadLocation.Invoke(locationSO, _showLoadScreen);
            }
        }
        #endregion
    }
}
