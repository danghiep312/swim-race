using System;
using System.ComponentModel;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Effect.Popup
{
    public class PopupEffectTwoDimension : PopupEffect
    {
        [FoldoutGroup("Appear Effect")] public Vector3 scaleValue;

        [FoldoutGroup("Disappear Effect")] public Vector3 disScaleValue;


        [ShowIf("dimensionType", DimensionType.Separate, false)] [HorizontalGroup("Appear Effect/Axis X")]
        public float durationX, delayX;

        [ShowIf("dimensionType", DimensionType.Separate, false)] [HorizontalGroup("Appear Effect/Axis Y")]
        public float durationY, delayY;

        [ShowIf("@dimensionType == DimensionType.Separate && animationType == AnimationType.Ease", false)]
        [FoldoutGroup("Appear Effect")]
        public Ease animEaseX, animEaseY;

        [ShowIf("@dimensionType == DimensionType.Separate && animationType == AnimationType.AnimationCurve", false)]
        [FoldoutGroup("Appear Effect")]
        public AnimationCurve animCurveX, animCurveY;


        [ShowIf("disDimensionType", DimensionType.Separate, false)] [HorizontalGroup("Disappear Effect/Axis X")]
        public float disDurationX, disDelayX;

        [ShowIf("disDimensionType", DimensionType.Separate, false)] [HorizontalGroup("Disappear Effect/Axis Y")]
        public float disDurationY, disDelayY;

        [ShowIf("@disDimensionType == DimensionType.Separate && disAnimationType == AnimationType.Ease", false)]
        [FoldoutGroup("Disappear Effect")]
        public Ease disAnimEaseX, disAnimEaseY;

        [ShowIf("@disDimensionType == DimensionType.Separate && disAnimationType == AnimationType.AnimationCurve",
            false)]
        [FoldoutGroup("Disappear Effect")]
        public AnimationCurve disAnimCurveX, disAnimCurveY;

        public override void Appear()
        {
            DOTween.Kill(transform);
            PanelAppear();
            FadePanelAppear();
        }

        public void PanelAppear()
        {
            transform.localScale = Vector3.zero;
            if (dimensionType == DimensionType.Together)
            {
                if (animationType == AnimationType.Ease)
                    transform.DOScale(scaleValue, duration).SetEase(animEase).SetDelay(delay);
                else transform.DOScale(scaleValue, duration).SetEase(animCurve).SetDelay(delay);
            }
            else
            {
                if (animationType == AnimationType.Ease)
                {
                    DoScaleAxis(0, scaleValue.x, durationX, delayX).SetEase(animEaseX);
                    DoScaleAxis(1, scaleValue.y, durationY, delayY).SetEase(animEaseY).SetDelay(delayY);
                }
                else
                {
                    DoScaleAxis(0, scaleValue.x, durationX, delayX).SetEase(animCurveX);
                    DoScaleAxis(1, scaleValue.y, durationY, delayY).SetEase(animCurveY);
                }
            }
        }

        public void FadePanelAppear()
        {
            panelToFade.alpha = 0;
            panelToFade.DOFade(1, DurationFade(dimensionType)).SetEase(fadeCurve).OnComplete(onAppearComplete.Invoke);
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> DoScaleAxis(int axis, float value, float dur, float de)
        {
            if (axis == 0) return transform.DOScaleX(value, dur).SetDelay(de);
            return transform.DOScaleY(value, dur).SetDelay(de);
        }

        protected override float DurationFade(DimensionType type)
        {
            return durationX + delayX >= durationY + delayY ? durationX + delayX : durationY + delayY;
        }


        public override void Disappear()
        {
            onDisappear?.Invoke();
            try
            {
                if (dimensionType == DimensionType.Together)
                {
                    if (disAnimationType == AnimationType.Ease)
                        transform.DOScale(disScaleValue, disDuration).SetEase(disAnimEase).SetDelay(disDelay);
                    else transform.DOScale(disScaleValue, disDuration).SetEase(disAnimCurve).SetDelay(disDelay);
                }
                else
                {
                    if (disAnimationType == AnimationType.Ease)
                    {
                        DoScaleAxis(0, disScaleValue.x, disDurationX, disDelayX).SetEase(disAnimEaseX);
                        DoScaleAxis(1, disScaleValue.y, disDurationY, disDelayY).SetEase(disAnimEaseY);
                    }
                    else
                    {
                        DoScaleAxis(0, disScaleValue.x, disDurationX, disDelayX).SetEase(disAnimCurveX);
                        DoScaleAxis(1, disScaleValue.y, disDurationY, disDelayY).SetEase(disAnimCurveY);
                    }
                }

                panelToFade.DOFade(0, DurationFade(disDimensionType)).SetEase(disFadeCurve).OnComplete(() =>
                {
                    onDisappearComplete?.Invoke();
                    targetDisable.SetActive(false);
                });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                targetDisable.SetActive(false);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DOTween.Kill(transform);
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            try
            {
                disAnimCurve ??= AnimationCurve.Linear(0, 0, 1, 1);
                fadeCurve ??= AnimationCurve.Linear(0, 0, 1, 1);
                targetDisable = GetComponentInParent<Panel.Panel>().gameObject;
            }
            catch
            {
                // ignored
            }
        }
#endif
    }

    public enum DimensionType
    {
        Together,
        Separate
    }

    public enum AnimationType
    {
        Ease,
        AnimationCurve
    }
}