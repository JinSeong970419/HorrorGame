using UnityEngine.AddressableAssets;

namespace Horror
{
    /// <summary>
    /// Location, Manager 등 게임 씬에 기본 ScriptableObject
    /// </summary>
    public class GameSceneSO : DescriptionBaseSO
    {
        public enum GameSceneType
        {
            //Default 씬
            Location, // 플레이 기본 씬
            Menu, // Gameplay씬 로드

            //Special 씬
            Initialisation,
            PersistentManagers,
            Gameplay,

            // 현재씬
            //Art,
        }

        public GameSceneType sceneType;
        public AssetReference sceneReference; // 호출할 씬
        public AudioSignalSO musicTrack;
    }
}
