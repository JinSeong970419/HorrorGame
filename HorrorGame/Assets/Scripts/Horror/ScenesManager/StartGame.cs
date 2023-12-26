using UnityEngine;

namespace Horror
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _locationsToLoad;
        [SerializeField] private bool _showLoadScreen;

        [Header("Broadcasting On")]
        [SerializeField] private LoadGameEvent _loadLocation;

        [Header("Listening to")]
        [SerializeField] private GameEventVoid _onNewGameButton;
        [SerializeField] private GameEventVoid _onContinueButton;

        // private bool _hasSaveData;
        
        private void Start()
        {
            _onNewGameButton.AddListener(startNewGame);
            _onContinueButton.AddListener(ContinuePreviousgame);
        }

        private void startNewGame()
        {
            _loadLocation.Invoke(_locationsToLoad, _showLoadScreen);
        }

        private void ContinuePreviousgame()
        { 

        }
    }
}
