using UnityEngine;

namespace Horror.Pool
{
    /// <summary>
    /// 컴포넌트 Pool 생성
    /// </summary>
    public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
    {
        private Transform _parent;

        private Transform _poolRoot;
        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }
                return _poolRoot;
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_poolRoot != null)
            {
#if UNITY_EDITOR
                // Editor상에서 즉시 제거
                DestroyImmediate(_poolRoot.gameObject);
#else
				Destroy(_poolRoot.gameObject);
#endif
            }
        }

        /// <summary>
        /// Pool 활성화
        /// </summary>
        public override T Request()
        {
            T member = base.Request();
            member.gameObject.SetActive(true);
            return member;
        }

        /// <summary>
        /// Pool 반납
        /// </summary>
        public override void Return(T member)
        {
            member.transform.SetParent(PoolRoot.transform);
            member.gameObject.SetActive(false);
            base.Return(member);
        }

        /// <summary>
        /// Pool 생성
        /// </summary>
        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }

        /// <summary>
        /// 부모가 될 객체 지정
        /// </summary>
        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
        }
    }
}
