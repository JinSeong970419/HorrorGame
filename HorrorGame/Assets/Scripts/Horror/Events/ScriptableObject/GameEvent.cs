using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    public class GameEventBase : ScriptableObject
    {
        private List<UnityAction> listeners = new List<UnityAction>();
        public void AddListener(UnityAction call)
        {
            listeners.Add(call);
        }

        public void RemoveListener(UnityAction call)
        {
            listeners.Remove(call);
        }

        public void Invoke()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Invoke();
            }
        }
    }

    public class GameEvent : GameEventBase { }

    public class GameEvent<T> : GameEventBase
    {
        private List<UnityAction<T>> listeners = new List<UnityAction<T>>();
        public void AddListener(UnityAction<T> call)
        {
            listeners.Add(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            listeners.Remove(call);
        }

        public void Invoke(T arg)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].Invoke(arg);
            }
        }
    }

    public class GameEvent<T1, T2> : GameEventBase
    {
        private List<UnityAction<T1, T2>> listeners = new List<UnityAction<T1, T2>>();
        public void AddListener(UnityAction<T1, T2> call)
        {
            listeners.Add(call);
        }

        public void RemoveListener(UnityAction<T1, T2> call)
        {
            listeners.Remove(call);
        }

        public void Invoke(T1 arg1, T2 arg2)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].Invoke(arg1, arg2);
            }
        }
    }

    public class GameEvent<T1, T2, T3> : GameEventBase
    {
        private List<UnityAction<T1, T2, T3>> listeners = new List<UnityAction<T1, T2, T3>>();
        public void AddListener(UnityAction<T1, T2, T3> call)
        {
            listeners.Add(call);
        }

        public void RemoveListener(UnityAction<T1, T2, T3> call)
        {
            listeners.Remove(call);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            int count = listeners.Count;
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].Invoke(arg1, arg2, arg3);
            }
        }
    }

    public class GameEvent<T1, T2, T3, T4> : GameEventBase
    {
        private List<UnityAction<T1, T2, T3, T4>> listeners = new List<UnityAction<T1, T2, T3, T4>>();
        public void AddListener(UnityAction<T1, T2, T3, T4> call)
        {
            listeners.Add(call);
        }

        public void RemoveListener(UnityAction<T1, T2, T3, T4> call)
        {
            listeners.Remove(call);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            int count = listeners.Count;
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
}
