using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveAbility : MonoBehaviour
{

    public Image abilityImage;
    public Image background;
    public Color cooldownColor;
    public ActiveAbility activeAbility;

    public bool IsActiveAbility;
    public static UIActiveAbility InstanceActive, InstancePassive;

    private void Awake()
    {
        if(IsActiveAbility)
        {
            InstanceActive = this;
        }
        else
        {
            InstancePassive = this;
        }
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float progress = activeAbility != null ? activeAbility.GetProgress() : 1f;
        //---------

        bool onCooldown = progress <= 0.998f;

        abilityImage.color = onCooldown ? cooldownColor : Color.white;
        abilityImage.fillAmount = progress;

    }

    public void WarnOnCooldown()
    {
        background.DOColor(Color.red, 0.1f).OnComplete(() => background.DOColor(Color.white, 0.1f));
        //shake
        background.rectTransform.DOShakePosition(0.1f, 10f, 10, 90f, false, true);
    }
}
