using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Horror
{
    public class LocationEntrance : MonoBehaviour
    {
        [SerializeField] private PathSO _entrancePath;
        [SerializeField] private PathStorageSO _pathStorage = default; //마지막으로 선택한 경로가 저장
        [SerializeField] private CinemachineVirtualCamera entranceShot;

        [Header("Scene Player Spawn")]
        [SerializeField] private GameEventVoid _onCallSpawn;

        public PathSO EntrancePath => _entrancePath;

        private void Awake()
        {
            if (_pathStorage.lastPathTaken == _entrancePath)
            {
                entranceShot.Priority = 100;
                _onCallSpawn.AddListener(PlanTransition);
            }
        }

        private void PlanTransition()
        {
            StartCoroutine(TransitionToGameCamera());
        }

        private IEnumerator TransitionToGameCamera()
        {

            yield return new WaitForSeconds(.1f);

            entranceShot.Priority = -1;
            _onCallSpawn.RemoveListener(PlanTransition);
        }
    }
}
