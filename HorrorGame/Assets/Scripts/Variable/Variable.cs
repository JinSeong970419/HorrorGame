using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    public abstract class VariableBase : ScriptableObject
    {
        public abstract object BoxedValue { get; set; }
    }

    public class Variable<T> : VariableBase, ISerializationCallbackReceiver
    {
        [SerializeField] private T initialValue;
        [SerializeField] private T runtimeValue;

        private UnityEvent<T> onValueChanged = new UnityEvent<T>();
        public UnityEvent<T> OnValueChanged { get { return onValueChanged; } }

        public T Value
        {
            get { return runtimeValue; }
            set
            {
                T oldValue = runtimeValue;

                runtimeValue = value;
                if (!value.Equals(oldValue))
                {
                    onValueChanged.Invoke(value);
                }
            }
        }

        public override object BoxedValue
        {
            get { return runtimeValue; }
            set
            {
                T oldValue = runtimeValue;
                runtimeValue = (T)value;
                if (!value.Equals(oldValue))
                {
                    onValueChanged.Invoke((T)value);
                }

            }
        }

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
