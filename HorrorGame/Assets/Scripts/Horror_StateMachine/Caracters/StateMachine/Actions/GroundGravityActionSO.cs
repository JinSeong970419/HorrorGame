using UnityEngine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
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

        private new GroundGravityActionSO OriginSO => (GroundGravityActionSO)base.OriginSO;

        public override void Awake(StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            _protagonist.movementVector.y = OriginSO.verticalPull;
        }
    }
}
