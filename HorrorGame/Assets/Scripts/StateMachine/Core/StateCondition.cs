using Horror.StateMachine.ScriptableObjects;

namespace Horror.StateMachine
{
    public abstract class Condition : IStateComponent
    {
        private bool _isCached;
        private bool _cachedStatement;
        internal StateConditionSO _originSO;

        protected StateConditionSO OriginSO => _originSO;

        /// <summary>
        /// Condition 조건 여부 확인
        /// </summary>
        protected abstract bool Statement();

        /// <summary>
        /// StateMent 캐싱 작업
        /// </summary>
        internal bool GetStatement()
        {
            if (!_isCached)
            {
                _isCached = true;
                _cachedStatement = Statement();
            }

            return _cachedStatement;
        }

        internal void ClearStatementCache()
        {
            _isCached = false;
        }

        /// <summary>
        /// 새 Instance 호출 시 Awake 작업 호출.
        /// </summary>
        public virtual void Awake(StateMachine stateMachine) { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }

    /// <summary>
    /// Action 조건 및 결과 구조체
    /// </summary>
    public readonly struct StateCondition
    {
        internal readonly StateMachine _stateMachine;
        internal readonly Condition _condition;
        internal readonly bool _expectedResult;

        public StateCondition(StateMachine stateMachine, Condition condition, bool expectedResult)
        {
            _stateMachine = stateMachine;
            _condition = condition;
            _expectedResult = expectedResult;
        }

        public bool IsMet()
        {
            bool statement = _condition.GetStatement();
            bool isMet = statement == _expectedResult;

#if UNITY_EDITOR
            _stateMachine._debugger.TransitionConditionResult(_condition._originSO.name, statement, isMet);
#endif
            return isMet;
        }
    }
}