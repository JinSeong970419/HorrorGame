#if UNITY_EDITOR

using System;
using System.Text;
using UnityEngine;

namespace Horror.StateMachine.Debugging
{
    /// <summary>
    /// Editor 환경에서 상태 전환 디버깅 진행
    /// </summary>
    [Serializable]
    internal class StateMachineDebugger
    {
        [Tooltip("Debug 로그 트리거 여부")]
        [SerializeField] internal bool debugTransitions;

        [SerializeField] internal bool appendConditionsInfo = true;
        [SerializeField] internal bool appendActionsInfo = true;

        [Tooltip("현재 상태 이름 출력")]
        [SerializeField] internal string currentState;

        private StateMachine _stateMachine;
        private StringBuilder _logBuilder;
        private string _targetState = string.Empty;

        /// <summary>
        /// Uicode Character
        /// </summary>
        private const string CHECK_MARK = "\u2714";
        private const string UNCHECK_MARK = "\u2718";
        private const string THICK_ARROW = "\u279C";
        private const string SHARP_ARROW = "\u27A4";

        /// <summary>
        /// Debug를 위한 Awake 호출
        /// </summary>
        internal void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _logBuilder = new StringBuilder();

            currentState = stateMachine._currentState._originSO.name;
        }

        internal void TransitionEvaluationBegin(string targetState)
        {
            _targetState = targetState;

            if (!debugTransitions)
                return;

            _logBuilder.Clear();
            _logBuilder.AppendLine($"{_stateMachine.name} state changed");
            _logBuilder.AppendLine($"{currentState}  {SHARP_ARROW}  {_targetState}");

            if (appendConditionsInfo)
            {
                _logBuilder.AppendLine();
                _logBuilder.AppendLine($"Transition Conditions:");
            }
        }

        internal void TransitionConditionResult(string conditionName, bool result, bool isMet)
        {
            if (!debugTransitions || _logBuilder.Length == 0 || !appendConditionsInfo)
                return;

            _logBuilder.Append($"    {THICK_ARROW} {conditionName} == {result}");

            if (isMet)
                _logBuilder.AppendLine($" [{CHECK_MARK}]");
            else
                _logBuilder.AppendLine($" [{UNCHECK_MARK}]");
        }

        internal void TransitionEvaluationEnd(bool passed, StateAction[] actions)
        {
            if (passed)
                currentState = _targetState;

            if (!debugTransitions || _logBuilder.Length == 0)
                return;

            if (passed)
            {
                LogActions(actions);
                PrintDebugLog();
            }

            _logBuilder.Clear();
        }

        private void LogActions(StateAction[] actions)
        {
            if (!appendActionsInfo)
                return;

            _logBuilder.AppendLine();
            _logBuilder.AppendLine("State Actions:");

            foreach (StateAction action in actions)
            {
                _logBuilder.AppendLine($"    {THICK_ARROW} {action._originSO.name}");
            }
        }

        private void PrintDebugLog()
        {
            _logBuilder.AppendLine();
            _logBuilder.Append("--------------------------------");

            Debug.Log(_logBuilder.ToString());
        }
    }
}
#endif