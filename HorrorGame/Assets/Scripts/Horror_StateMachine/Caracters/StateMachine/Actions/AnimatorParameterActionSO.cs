using UnityEngine;
using Horror.StateMachine.ScriptableObjects;
using Moment = Horror.StateMachine.StateAction.SpecificMoment;

namespace Horror.StateMachine
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

        private new AnimatorParameterActionSO OriginSO => (AnimatorParameterActionSO)base.OriginSO;
        private int _parameterHash;

        public AnimatorParameterAction(int parameterHash)
        {
            _parameterHash = parameterHash;
        }

        public override void Awake(StateMachine stateMachine)
        {
            _animator = stateMachine.GetComponent<Animator>();
        }

        public override void OnStateEnter()
        {
            if (OriginSO.whenToRun == SpecificMoment.OnStateEnter)
                SetParameter();
        }

        public override void OnStateExit()
        {
            if (OriginSO.whenToRun == SpecificMoment.OnStateExit)
                SetParameter();
        }

        private void SetParameter()
        {
            switch (OriginSO.parameterType)
            {
                case AnimatorParameterActionSO.ParameterType.Bool:
                    _animator.SetBool(_parameterHash, OriginSO.boolValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Int:
                    _animator.SetInteger(_parameterHash, OriginSO.intValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Float:
                    _animator.SetFloat(_parameterHash, OriginSO.floatValue);
                    break;
                case AnimatorParameterActionSO.ParameterType.Trigger:
                    _animator.SetTrigger(_parameterHash);
                    break;
            }
        }

        public override void OnUpdate() { }
    }
}
