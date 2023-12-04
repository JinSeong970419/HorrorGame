using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [CreateAssetMenu(menuName = "Events/UI/Fade Event")]
    public class FadeEvent : DescriptionBaseSO
    {
        public UnityAction<bool, float, Color> OnEventRaised;

        /// <summary>
        /// 플레이 씬으로 페이드 전환
        /// </summary>
        public void FadeIn(float duration)
        {
            Fade(true, duration, Color.clear);
        }

        /// <summary>
        /// 검은 화면으로 페이드 전환
        /// </summary>
        public void FadeOut(float duration)
        {
            Fade(false, duration, Color.black);
        }

        /// <summary>
        /// Default Fade
        /// </summary>
        private void Fade(bool fadeIn, float duration, Color color)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(fadeIn, duration, color);
        }
    }
}
