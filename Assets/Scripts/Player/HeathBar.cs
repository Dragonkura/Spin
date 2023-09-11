using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeathBar : MonoBehaviour
{
    public Action OnEndEffect;
    public Image heathBar;
    public Image comsumeField;
    private Tweener transition;
    public Tweener DoEffectOnTitanMode(float value, float duration)
    {
        comsumeField.gameObject.SetActive(true);
        UpdateHeathBarSlider(0.5f);
        comsumeField.DOBlendableColor(Color.red, 0.2f).SetLoops(-1, LoopType.Yoyo);
        transition = DOTween.To(x => comsumeField.fillAmount = x, 1f, value, duration).SetEase(Ease.Linear);
        transition.onComplete += (() => {
            OnEndEffect?.Invoke();
            OnEndEffect = null;
            comsumeField.DOKill();
            comsumeField.gameObject.SetActive(false);
            transition = null;
        });
        return transition;
    }
    internal void UpdateHeathBarSlider(float value)
    {
        if (heathBar != null) heathBar.fillAmount = value;
    }
}
