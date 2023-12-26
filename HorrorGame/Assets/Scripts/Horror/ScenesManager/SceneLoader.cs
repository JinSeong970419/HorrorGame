using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Horror
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _gameplayScene;

        [Header("Listening to")]
        [SerializeField] private LoadGameEvent _loadLocation;
        [SerializeField] private LoadGameEvent _StartupLocation;

        [Header("BroadCasting")]
        [SerializeField] private GameEventBool _toggleLoadingScreen;
        [SerializeField] private GameEventVoid _onCallSpawn; // Spawn System용 Event
        [SerializeField] private FadeEvent _fadeRequest;

        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

        private GameSceneSO _sceneLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;

        private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
        private float _fadeDuration = .5f;
        private bool _isLoading; // Scene 로딩 중 새 로드를 방지

        private void OnEnable()
        {
            _loadLocation.OnLoadingRequested += LoadLocation;
            _StartupLocation.OnLoadingRequested += LocationStartup;
        }

        private void OnDisable()
        {
            _loadLocation.OnLoadingRequested -= LoadLocation;
            _StartupLocation.OnLoadingRequested -= LocationStartup;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 테스트용 특수 로딩, Initialize를 거치지 않고 씬을 불러오는 Editor용 함수
        /// </summary>
        private void LocationStartup(GameSceneSO currentOpenLocation, bool showLoadingScreen, bool fadeScreen)
        {
            _currentlyLoadedScene = currentOpenLocation;

            if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
            {
                //GamePlayManager 씬이 동시에 로드됨
                _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                // 씬을 로드 할 동안 대기 
                // 1. 호출 시 프레임 드랍 및 UI 응답성 저하가 발생할 수 있음
                _gameplayManagerLoadingOpHandle.WaitForCompletion();
                _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

                GameStart();
            }
        }
#endif

        /// <summary>
        /// 파라미터로 전달된 Location Scene을 로드
        /// </summary>
        private void LoadLocation(GameSceneSO locationLoad, bool showLoadingScreen, bool fadeScreen)
        {
            // 중복 로드 방지
            if (_isLoading)
                return;

            _sceneLoad = locationLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            // 게임 메뉴에서 호출 시 GamePlayManager 먼저 로드
            if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

            StartCoroutine(UnloadPreviousScene());
        }

        /// <summary>
        /// 이전 씬 제거
        /// </summary>
        private IEnumerator UnloadPreviousScene()
        {
            _fadeRequest.FadeOut(_fadeDuration);

            yield return new WaitForSeconds(_fadeDuration);

            if (_currentlyLoadedScene != null) //Initialisation에서 초기화
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    _currentlyLoadedScene.sceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            LoadNewScene();
        }

        /// <summary>
        /// 새로운 Scene을 비동기 방식으로 로딩
        /// </summary>
        private void LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.Invoke(true);
            }

            _loadingOperationHandle = _sceneLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            // 현재 씬 저장
            _currentlyLoadedScene = _sceneLoad;

            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);
            LightProbes.TetrahedralizeAsync();

            _isLoading = false;

            if (_showLoadingScreen)
                _toggleLoadingScreen.Invoke(false);

            _fadeRequest.FadeIn(_fadeDuration);

            GameStart();
        }

        private void GameStart()
        {
            _onCallSpawn.Invoke();
        }
    }
}
