using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
    public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

    public class ApplyMovementVectorAction : StateAction
    {
        private Protagonist _protagonist;
        private CharacterController _characterController;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _protagonist = stateMachine.GetComponent<Protagonist>();
            _characterController = stateMachine.GetComponent<CharacterController>();
        }

        public override void OnUpdate()
        {
            _characterController.Move(_protagonist.movementVector * Time.deltaTime);
            _protagonist.movementVector = _characterController.velocity;
        }
    }
}
