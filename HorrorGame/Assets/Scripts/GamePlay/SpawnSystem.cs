using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private TransformAnchor _playerTransformAnchor;
        [SerializeField] private Protagonist _playerPrefab;

        [Header("Scene Player Spawn")]
        [SerializeField] private GameEventVoid _onCallSpawn;

        [SerializeField] private Transform spawnLocation;

        private void Awake()
        {
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
            //Transform spawnLocation = GetSpawnLocation();
            Protagonist playerInstance = Instantiate(_playerPrefab, spawnLocation.position, spawnLocation.rotation);


            _playerTransformAnchor.Provide(playerInstance.transform); 
        }

        //private Transform GetSpawnLocation()
        //{
        //    if (_pathTaken == null)
        //        return _defaultSpawnPoint;

        //    //Look for the element in the available LocationEntries that matches tha last PathSO taken
        //    int entranceIndex = Array.FindIndex(_spawnLocations, element =>
        //        element.EntrancePath == _pathTaken.lastPathTaken);

        //    if (entranceIndex == -1)
        //    {
        //        Debug.LogWarning("The player tried to spawn in an LocationEntry that doesn't exist, returning the default one.");
        //        return _defaultSpawnPoint;
        //    }
        //    else
        //        return _spawnLocations[entranceIndex].transform;
        //}
    }
}
