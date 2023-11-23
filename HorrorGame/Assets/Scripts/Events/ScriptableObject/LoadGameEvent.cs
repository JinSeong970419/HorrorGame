using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [CreateAssetMenu(fileName = "Load Game Event", menuName = "Game Event/LoadGameEvents", order = 11)]
    public class LoadGameEvent : DescriptionBaseSO
    {
        public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;

        public void Invoke(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (OnLoadingRequested != null)
            {
                OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
            }
            else
            {
                Debug.LogWarning("이벤트 Error, Listener 확인");
            }
        }
    }
}
