using UnityEngine;

namespace Horror.StateMachine.ScriptableObjects
{
    /// <summary>
	/// ScripableObject Basic 스크립트
	/// </summary>
    public class DescriptionActionBaseSO : ScriptableObject
    {
        [TextArea] public string description;
    }
}
