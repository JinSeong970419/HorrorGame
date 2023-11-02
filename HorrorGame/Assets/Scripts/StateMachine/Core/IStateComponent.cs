namespace Horror.StateMachine
{
    interface IStateComponent
    {
        /// <summary>
        /// 상태 진입 시 호출
        /// </summary>
        void OnStateEnter();

        /// <summary>
        /// 상태 전환 시 호출
        /// </summary>
        void OnStateExit();
    }
}
