using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Started Move")]
    public class IsMoveSO : StateConditionSO<IsMoveCondition>
    {
        // 이동 반응
        public float treshold = 0.02f;
    }

    public class IsMoveCondition : Condition
    {
        private Protagonist _protagonist;
        private IsMoveSO _originSO => (IsMoveSO)OriginSO;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
        }

        protected override bool Statement()
        {
            Vector3 movementVector = _protagonist.movementInput;
            movementVector.y = 0f;
            return movementVector.sqrMagnitude > _originSO.treshold;
        }
    }
}
