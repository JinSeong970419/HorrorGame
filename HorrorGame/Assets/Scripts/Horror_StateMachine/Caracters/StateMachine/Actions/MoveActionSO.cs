using UnityEngine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
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
        private new MoveActionSO OriginSO => (MoveActionSO)base.OriginSO;

        public override void Awake(StateMachine stateMachine)
        {
            _protagonistScript = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            _protagonistScript.movementVector.x = _protagonistScript.movementInput.x * OriginSO.speed;
            _protagonistScript.movementVector.z = _protagonistScript.movementInput.z * OriginSO.speed;
        }
    }
}
