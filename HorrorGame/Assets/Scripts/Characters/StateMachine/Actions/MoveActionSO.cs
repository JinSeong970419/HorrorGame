using UnityEngine;
using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror
{
    [CreateAssetMenu(fileName = "MoveAction", menuName = "State Machines/Actions/MoveAction")]
    public class MoveActionSO : StateActionSO<MoveAction>
    {
        [Tooltip("Player Run speed")]
        public float speed = 8f;
    }

    public class MoveAction : StateAction
    {
        private Protagonist _protagonistScript;
        private MoveActionSO _originSO => (MoveActionSO)base.OriginSO;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _protagonistScript = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            _protagonistScript.movementVector.x = _protagonistScript.movementInput.x * _originSO.speed;
            _protagonistScript.movementVector.z = _protagonistScript.movementInput.z * _originSO.speed;
        }
    }
}
