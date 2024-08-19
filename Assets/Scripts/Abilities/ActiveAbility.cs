using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour
{
    public float cooldown = 1f;
    private float currentCooldown = 0f;

    UIActiveAbility uiActiveAbility;
    public Sprite spriteIcon;

    public bool IsReady => currentCooldown <= 0f;

    private bool ableToUse = false;

    private void Start()
    {
        if (TryGetComponent<CatController>(out var _))
            LockIn();
    }

    public void UseAbility()
    {
        if (IsReady)
        {
            currentCooldown = cooldown;
            ActivateAbility();
        }
        else
            uiActiveAbility.WarnOnCooldown();

    }

    public void LockIn()
    {
        AudioManager.Instance.PlayCardSelection();

        uiActiveAbility = UIActiveAbility.InstanceActive;
        ableToUse = true;
        Debug.Log(uiActiveAbility);
        uiActiveAbility.gameObject.SetActive(true);
        uiActiveAbility.abilityImage.sprite= spriteIcon;
        uiActiveAbility.activeAbility = this;
    }

    protected virtual void ActivateAbility()
    {

        // Implement the ability here
    }

    public float GetProgress()
    {
        return Mathf.Clamp01(1f - currentCooldown / cooldown);
    }

    private void Update()
    {
        if(!ableToUse)
            return;

        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseAbility();
        }
    }
}
