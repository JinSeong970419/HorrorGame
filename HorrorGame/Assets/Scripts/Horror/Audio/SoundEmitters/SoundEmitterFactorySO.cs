using UnityEngine;
using Horror.Factory;

namespace Horror
{
    [CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Factory/SoundEmitter Factory")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        [SerializeField] private SoundEmitter prefab;
        public override SoundEmitter Create()
        {
            return Instantiate(prefab);
        }
    }
}
