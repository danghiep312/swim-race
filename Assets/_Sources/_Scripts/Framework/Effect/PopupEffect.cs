using System;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PopupEffect : MonoBehaviour
{
    public Image panelToFade;
    public float transparentValue;
    [Title("Appear Effect")]
    public float duration;
    public float delay;
    public Ease ease;
    
    [Title("Disappear Effect")]
    public float disDuration;
    public float disDelay;
    public Ease disEase;
    public GameObject targetDisable;
    
    [Title("Complete Callback")]
    public UnityEvent appearCallback;
    public UnityEvent disappearCallback;
    
    
    [Title("Init callback")]
    [Description("Invoke when start effect")]
    public UnityEvent initCallback;
    [Description("Invoke when start effect")]
    public UnityEvent disableCallback;
    
    private void OnEnable()
    {
        initCallback?.Invoke();
        panelToFade.DOFade(transparentValue, duration).SetDelay(delay).SetEase(Ease.Linear);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, duration).SetDelay(delay).SetEase(ease).OnComplete(() =>
        {
            appearCallback?.Invoke();
        });
    }

    public void Disappearance()
    {
        disableCallback?.Invoke();
        panelToFade.DOFade(0, disDuration).SetDelay(disDelay).SetEase(Ease.Linear);
        transform.DOScale(Vector3.zero, disDuration).SetDelay(disDelay).SetEase(disEase).OnComplete(() =>
        {
            disappearCallback?.Invoke();
            targetDisable.SetActive(false);
        });
    }

}
