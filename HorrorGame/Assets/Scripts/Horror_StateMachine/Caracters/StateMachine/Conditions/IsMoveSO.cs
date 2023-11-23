using UnityEngine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
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
        private new IsMoveSO OriginSO => (IsMoveSO)base.OriginSO;

        public override void Awake(StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
        }

        protected override bool Statement()
        {
            Vector3 movementVector = _protagonist.movementInput;
            movementVector.y = 0f;
            return movementVector.sqrMagnitude > OriginSO.treshold;
        }
    }
}
