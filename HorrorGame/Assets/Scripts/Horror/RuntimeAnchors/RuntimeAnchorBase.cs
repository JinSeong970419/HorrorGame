using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    public class RuntimeAnchorBase<T> : DescriptionBaseSO where T : Object
    {
        public UnityAction onAnchorProvided;

        [Header("Debug")]
        [ReadOnly] public bool isSet = false;

        [ReadOnly, SerializeField] private T _value;
        public T Value { get { return _value; } }

        public void Provide(T value)
        {
            if (value == null)
            {
                Debug.LogError($"{this.name} runtime anchor Null");
                return;
            }

            _value = value;
            isSet = true;

            if (onAnchorProvided != null)
                onAnchorProvided.Invoke();
        }

        public void UnSet()
        {
            _value = null;
            isSet = false;
        }

        private void OnDisable()
        {
            UnSet();
        }
    }
}
