using UnityEngine;
using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;

namespace Horror
{
    [CreateAssetMenu(fileName = "StopMovementAction", menuName = "State Machines/Actions/Stop Movement")]
    public class StopMovementActionSO : StateActionSO
    {
        [SerializeField] private StateAction.SpecificMoment _moment;
        public StateAction.SpecificMoment Moment => _moment;

        protected override StateAction CreateAction() => new StopMovement();
    }

    public class StopMovement : StateAction
    {
        private Protagonist _protagonist;
        private new StopMovementActionSO OriginSO => (StopMovementActionSO)base.OriginSO;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            if (OriginSO.Moment == SpecificMoment.OnUpdate)
                _protagonist.movementInput = Vector3.zero;
        }

        public override void OnStateEnter()
        {
            if (OriginSO.Moment == SpecificMoment.OnStateEnter)
                _protagonist.movementInput = Vector3.zero;
        }

        public override void OnStateExit()
        {
            if (OriginSO.Moment == SpecificMoment.OnStateExit)
                _protagonist.movementInput = Vector3.zero;
        }
    }
}
