
using UnityEngine.UI;

namespace Framework.Effect.Popup
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;


    public abstract class PopupEffect : MonoBehaviour
    {
        [FoldoutGroup("Panel Setup")] public CanvasGroup panelToFade;
        [FoldoutGroup("Panel Setup")] public float transparentValue = .8f;

        
        [FoldoutGroup("Appear Effect")]
        // [FoldoutGroup("Appear Effect")]
        public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [ShowIf("IsTwoDimension", false)]
        [Space(10)] [HideLabel] [EnumToggleButtons] [FoldoutGroup("Appear Effect")]
        public DimensionType dimensionType;
        
        [ShowIf("dimensionType", DimensionType.Together, false)] [HorizontalGroup("Appear Effect/TogetherDuration")]
        public float duration = .5f;
        
        [ShowIf("dimensionType", DimensionType.Together, false)] [HorizontalGroup("Appear Effect/TogetherDuration")]
        public float delay;
        
        [Space(20)] [HideLabel, EnumToggleButtons] [FoldoutGroup("Appear Effect")]
        public AnimationType animationType;
        
        [ShowIf("@dimensionType == DimensionType.Together && animationType == AnimationType.Ease", false)] [FoldoutGroup("Appear Effect")]
        public Ease animEase = Ease.OutBack;
        
        [ShowIf("@dimensionType == DimensionType.Together && animationType == AnimationType.AnimationCurve", false)] [FoldoutGroup("Appear Effect")]
        public AnimationCurve animCurve;

        
        
        
        [FoldoutGroup("Disappear Effect")]
        public AnimationCurve disFadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [ShowIf("IsTwoDimension", false)]
        [HideLabel] [EnumToggleButtons] [FoldoutGroup("Disappear Effect")] 
        public DimensionType disDimensionType;
        
        [ShowIf("disDimensionType", DimensionType.Together, false)][HorizontalGroup("Disappear Effect/DisTogetherDuration")]
        public float disDuration = 0.4f;
        
        [ShowIf("disDimensionType", DimensionType.Together, false)][HorizontalGroup("Disappear Effect/DisTogetherDuration")]
        public float disDelay;
        
        [Space(10)] [HideLabel, EnumToggleButtons] [FoldoutGroup("Disappear Effect")] 
        public AnimationType disAnimationType;
        
        [ShowIf("@disDimensionType == DimensionType.Together && disAnimationType == AnimationType.Ease", false)] [FoldoutGroup("Disappear Effect")]
        public Ease disAnimEase = Ease.InBack;
        
        [ShowIf("@disDimensionType == DimensionType.Together && disAnimationType == AnimationType.AnimationCurve", false)] [FoldoutGroup("Disappear Effect")]
        public AnimationCurve disAnimCurve;
        
        
        
        [PropertySpace(SpaceBefore = 20, SpaceAfter = 20)]
        public GameObject targetDisable;
    
        [FoldoutGroup("Callback")] 
        public UnityEvent onAppear;
        [FoldoutGroup("Callback")] 
        public UnityEvent onDisappear;
        [FoldoutGroup("Callback")] 
        public UnityEvent onAppearComplete;
        [FoldoutGroup("Callback")] 
        public UnityEvent onDisappearComplete;

        public abstract void Appear();
        public abstract void Disappear();

        public bool overrideAppear;
        
        protected virtual void OnEnable()
        {
            onAppear?.Invoke();
            panelToFade.GetComponent<Image>().SetAlpha(transparentValue);
            if (overrideAppear)
            {
                overrideAppear = false;
                return;
            }
            Appear();
            //panelToFade.DOFade(1, duration).SetEase(fadeCurve);
        }

        protected virtual void OnDisable()
        {
            DOTween.Kill(panelToFade);
        }
        
        protected virtual float DurationFade(DimensionType type)
        {
            return duration + delay;
            //return durationX + delayX >= durationY + delayY ? durationX + delayX : durationY + delayY;
        }
        
        public bool IsTwoDimension()
        {
            return GetComponent<PopupEffectTwoDimension>() != null;
        }



#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (panelToFade == null)
                panelToFade = transform.parent.GetComponentInChildren<CanvasGroup>(true);
            if (panelToFade == null)
                Debug.LogWarning("[CanvasGroup] not found in parent");
            if (targetDisable == null)
                targetDisable = GetComponentInParent<Panel.Panel>(true).gameObject;
        }
#endif
    }
}