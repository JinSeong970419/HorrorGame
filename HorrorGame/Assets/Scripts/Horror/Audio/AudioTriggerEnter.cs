using UnityEngine;

namespace Horror
{
    [RequireComponent(typeof(AudioAction))]
    public class AudioTriggerEnter : MonoBehaviour
    {
        [SerializeField] private string _tagDetect = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(_tagDetect))
                GetComponent<AudioAction>().PlayAudioSignal();
        }
    }
}
