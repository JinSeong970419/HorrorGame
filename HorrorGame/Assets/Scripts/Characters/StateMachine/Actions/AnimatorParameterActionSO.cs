using Horror.StateMachine;
using Horror.StateMachine.ScriptableObjects;
using UnityEngine;
using Moment = Horror.StateMachine.StateAction.SpecificMoment;

namespace Horror
{
    [CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
    public class AnimatorParameterActionSO : StateActionSO
    {
        public enum ParameterType
        {
            Bool, Int, Float, Trigger
        }

        public ParameterType parameterType;
        public string parameterName;

        public bool boolValue;
        public int intValue;
        public float floatValue;

        public Moment whenToRun;

        protected override StateAction CreateAction() => new AnimatorParameterAction(Animator.StringToHash(parameterName));
    }

    public class AnimatorParameterAction : StateAction
    {
        private Animator _animator;

        private AnimatorParameterActionSO _originSO => (AnimatorParameterActionSO)base.OriginSO;
        private int _parameterHash;

        public AnimatorParameterAction(int parameterHash)
        {
            _parameterHash = parameterHash;
        }

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _animator = stateMachine.GetComponent<Animator>();
        }

        public override void OnStateEnter()
        {
            if (_originSO.whenToRun == SpecificMoment.OnStateEnter)
                SetParameter();
        }

        public override void OnStateExit()
        {
            if (_originSO.whenToRun == SpecificMoment.OnStateExit)
                SetParameter();
        }

        private void SetParameter()
        {
            switch (_originSO.parameterType)
            {
                case AnimatorParameterActionSO.ParameterType.Bool:
                    _animator.SetBool(_parameterHash, _originSO.boolValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Int:
                    _animator.SetInteger(_parameterHash, _originSO.intValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Float:
                    _animator.SetFloat(_parameterHash, _originSO.floatValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Trigger:
                    _animator.SetTrigger(_parameterHash);
                    break;
            }
        }

        public override void OnUpdate() { }
    }
}
