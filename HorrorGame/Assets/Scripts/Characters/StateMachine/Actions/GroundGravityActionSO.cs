using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(fileName = "GroundGravity", menuName = "State Machines/Actions/Ground Gravity")]
    public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
    {
        [Tooltip("땅에 고정 시키기 위한 중력 값")]
        public float verticalPull = -9.81f;
    }

    public class GroundGravityAction : StateAction
    {
        private Protagonist _protagonist;

        private GroundGravityActionSO _originSO => (GroundGravityActionSO)base.OriginSO;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            _protagonist.movementVector.y = _originSO.verticalPull;
        }
    }
}
