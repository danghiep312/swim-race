using System;
using System.ComponentModel;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Effect.Popup
{
    public class SlidePanel : Framework.Effect.Popup.PopupEffect
    {
        [Title("Init value")] [VerticalGroup("Init value", Order = -1)]
        public RectTransform rectTransform;

        [VerticalGroup("Init value", Order = -1)] [PropertySpace(SpaceAfter = 20, SpaceBefore = 0)]
        public Vector2 startPos;

        public override void Appear()
        {
            
            rectTransform.anchoredPosition = startPos;
            var canvas = GetComponentInParent<CanvasGroup>(true);
            if (canvas)
            {
                DOTween.Kill(canvas);
                canvas.alpha = .2f;
                canvas.DOFade(1f, duration / 2f).SetDelay(duration / 2f).SetEase(Ease.Linear);
            }

            DOTween.Kill(rectTransform);
            rectTransform.anchoredPosition = startPos;
            rectTransform.DOAnchorPos(Vector2.zero, duration).SetDelay(delay).SetEase(animEase).OnComplete(() =>
            {
                onAppearComplete?.Invoke();
            });
        }

        public override void Disappear()
        {
            onDisappear?.Invoke();
            rectTransform.DOAnchorPos(startPos, disDuration).SetDelay(disDelay).SetEase(disAnimEase).OnComplete(() =>
            {
                onDisappearComplete?.Invoke();
                targetDisable.SetActive(false);
            });
        }

        public void Disappearance(bool blur = true)
        {
            onDisappear?.Invoke();
            var canvas = GetComponentInParent<CanvasGroup>();
            if (!blur)
            {
                canvas.alpha = .2f;
            }

            //canvas.DOFade(1f, duration / 2f).SetDelay(duration / 2f).SetEase(Ease.Linear);
            rectTransform.DOAnchorPos(startPos, disDuration).SetDelay(disDelay).SetEase(disAnimEase).OnComplete(() =>
            {
                targetDisable.SetActive(false);
            });
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (startPos == Vector2.zero)
            {
                startPos = Vector2.up * 2000f;
            }

            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

#endif
    }
}