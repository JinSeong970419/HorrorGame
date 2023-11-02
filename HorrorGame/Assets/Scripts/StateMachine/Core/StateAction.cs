using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
{
    public abstract class StateAction : IStateComponent
    {
        internal StateActionSO _originSO;

        protected StateActionSO OriginSO => _originSO;

        /// <summary>
        /// <see cref="State"/>을 참조하는 모든 <see cref="StateMachine"/> 및 <see cref="StateAction"/> 호출.
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// 새 Instance 호출 시 Awake 작업 호출.
        /// </summary>
        public virtual void Awake(StateMachine stateMachine) { }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        /// <summary>
        /// StateAction이 실행 될 순간을 결정
        /// </summary>
        public enum SpecificMoment
        {
            OnStateEnter, OnStateExit, OnUpdate,
        }
    }
}
