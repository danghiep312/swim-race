using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.Effect
{
    public class ButtonIdle : MonoBehaviour
    {
        public Vector3 originalScale = Vector3.one;
        public Vector3 targetScale = Vector3.one * 1.1f;
        public float duration = 0.5f;
        public float revDuration = 0.5f;
        public Ease ease = Ease.Linear;
        public Ease revEase = Ease.Linear;

        private void OnEnable()
        {
            transform.localScale = originalScale;
            transform.DOScale(targetScale, duration).SetEase(ease).OnComplete(() =>
            {
                transform.DOScale(originalScale, revDuration).SetEase(revEase);
            }).OnKill(() =>
            {
                transform.localScale = originalScale;
            }).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        [Button("Test Effect")]
        public void Test()
        {
            OnDisable();
            OnEnable();
        }
    }
}