using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Horror
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private FadeEvent _fadeEvent;
        [SerializeField] private Image _image;

        private void OnEnable()
        {
            _fadeEvent.OnEventRaised += InitiateFade;
        }

        private void OnDisable()
        {
            _fadeEvent.OnEventRaised -= InitiateFade;
        }

        private void InitiateFade(bool fadeIn, float duration, Color desiredColor)
        {
            _image.DOBlendableColor(desiredColor, duration);
        }
    }
}
