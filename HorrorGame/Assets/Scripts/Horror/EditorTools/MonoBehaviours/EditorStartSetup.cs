using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Horror
{
    public class EditorStartSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Scene GameSceneSO")]
        [SerializeField] private GameSceneSO _thisSceneSO;
        [SerializeField] private GameSceneSO _persistentManagersSO;

        [Header("NotifyStartupEvent")]
        [SerializeField] private AssetReference _notifyStartupEvent;

        [Header("Call Event")]
        [SerializeField] private GameEventVoid _onCallSpawn;

        [Header("Path Storage")]
        [SerializeField] private PathStorageSO _pathStorage;

        [Header("Save System")]
        [SerializeField] private SaveSystem _saveSystem;

        private bool isColdStart = false;

        private void Awake()
        {
            if (!SceneManager.GetSceneByName(_persistentManagersSO.sceneReference.editorAsset.name).isLoaded)
            {
                isColdStart = true;

                // Default Spawn지점에 캐릭터가 스폰되도록 경로 재설정
                _pathStorage.lastPathTaken = null;
            }
        }

        private void Start()
        {
            if (isColdStart)
            {
                _persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadSceneEvent;
            }
            CreateSaveFileIfNotPresent();
        }

        private void LoadSceneEvent(AsyncOperationHandle<SceneInstance> obj)
        {
            _notifyStartupEvent.LoadAssetAsync<LoadGameEvent>().Completed += OnNotifyChannelLoaded;
        }

        private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadGameEvent> obj)
        {
            if (_thisSceneSO != null)
            {
                obj.Result.Invoke(_thisSceneSO);
            }
            else
            {
                // 현재 씬 정보 없이 플레이어 호출
                // 어떤 씬에 존재하는 지 알 수 없음
                _onCallSpawn.Invoke();
            }
        }

        private void CreateSaveFileIfNotPresent()
        {
            if (_saveSystem != null && !_saveSystem.LoadSaveDataFromDisk())
            {
                _saveSystem.SetNewGameData();
            }
        }
#endif
    }
}
