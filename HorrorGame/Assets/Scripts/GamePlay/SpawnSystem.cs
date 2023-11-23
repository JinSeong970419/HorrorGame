using System;
using UnityEngine;

namespace Horror
{
    public class SpawnSystem : MonoBehaviour
    {
        [Header("Spawn References")]
        [SerializeField] private TransformAnchor _playerTransformAnchor;
        [SerializeField] private Protagonist _playerPrefab;
        [SerializeField] private PathStorageSO _pathTaken;

        [Header("Scene Player Spawn")]
        [SerializeField] private GameEventVoid _onCallSpawn;

        private LocationEntrance[] _spawnLocations;
        private Transform _defaultSpawnPoint;

        private void Awake()
        {
            //_spawnLocations = GameObject.FindObjectsOfType<LocationEntrance>();
            _spawnLocations = GameObject.FindObjectsByType<LocationEntrance>(FindObjectsSortMode.None);
            _defaultSpawnPoint = transform.GetChild(0);
        }

        private void OnEnable()
        {
            _onCallSpawn.AddListener(SpawnPlayer);
        }

        private void OnDisable()
        {
            _onCallSpawn.RemoveListener(SpawnPlayer);
            _playerTransformAnchor.UnSet();
        }

        private void SpawnPlayer()
        {
            Transform spawnLocation = GetSpawnLocation();
            Protagonist playerInstance = Instantiate(_playerPrefab, spawnLocation.position, spawnLocation.rotation);
            Transform provideActorPosition = playerInstance.transform.Find("ActorCameraRoot") != null ? playerInstance.transform.Find("ActorCameraRoot") : playerInstance.transform;
            _playerTransformAnchor.Provide(provideActorPosition); 
        }

        private Transform GetSpawnLocation()
        {
            if (_pathTaken == null)
                return _defaultSpawnPoint;

            //사용 가능한 위치 항목에서 마지막으로 선택한 경로와 일치여부 확인
            int entranceIndex = Array.FindIndex(_spawnLocations, element => element.EntrancePath == _pathTaken.lastPathTaken);

            if (entranceIndex == -1)
            {
                Debug.LogWarning("플레이어의 위치가 지정되지 않아 Deafult Sapwn을 진행");
                return _defaultSpawnPoint;
            }
            else
                return _spawnLocations[entranceIndex].transform;
        }
    }
}
