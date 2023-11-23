using UnityEngine;
using System.Collections.Generic;
using Horror.Factory;

namespace Horror.Pool
{
    /// <summary>
    /// 팩토리 패턴을 이용해 T 타입의 맴버를 생성하는 Pool
    /// </summary>
    public abstract class PoolSO<T> : ScriptableObject, IPool<T>
    {
        protected readonly Stack<T> Available = new Stack<T>(); // 사용 가능한 풀

        public abstract IFactory<T> Factory { get; set; }

        protected bool HasBeenPrewarmed { get; set; }

        public virtual void OnDisable()
        {
            Available.Clear();
            HasBeenPrewarmed = false;
        }

        protected virtual T Create()
        {
            return Factory.Create();
        }

        /// <summary>
        /// 초기 Pool을 생성
        /// </summary>
        /// <param name="num">생성할 풀 개수</param>
        public virtual void Prewarm(int num)
        {
            if (HasBeenPrewarmed)
            {
                Debug.LogWarning($"{name} Pool이 이미 준비됨.");
                return;
            }
            for (int i = 0; i < num; i++)
            {
                Available.Push(Create());
            }
            HasBeenPrewarmed = true;
        }

        public virtual T Request()
        {
            return Available.Count > 0 ? Available.Pop() : Create();
        }

        public virtual void Return(T member)
        {
            Available.Push(member);
        }
    }
}
