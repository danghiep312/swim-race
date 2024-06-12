using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Effect
{
    public class FadeTransition : MonoBehaviour
    {
        [ShowInInspector] private static Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            FadeOut(.5f);
        }

        public static void FadeIn(float time)
        {
            _image.DOFade(1f, time);
        }

        public static void FadeOut(float time)
        {
            _image.DOFade(0f, time);
        }
    }
}