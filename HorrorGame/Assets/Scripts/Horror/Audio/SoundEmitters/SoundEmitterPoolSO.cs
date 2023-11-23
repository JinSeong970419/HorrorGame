using UnityEngine;
using Horror.Pool;
using Horror.Factory;

namespace Horror
{
    [CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Pool/SoundEmitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField] private SoundEmitterFactorySO _factory;

        public override IFactory<SoundEmitter> Factory 
        {
            get => _factory;
            set => _factory = value as SoundEmitterFactorySO;
        }
    }
}
