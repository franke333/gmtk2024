using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{

    UIActiveAbility uiActiveAbility;

    public Sprite spriteIconSpeed,spriteIconSteering,spriteIconLifes;


    public void TakeLife()
    {
        AudioManager.Instance.PlayCardSelection();
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.gameObject.SetActive(true);
        uiActiveAbility.abilityImage.sprite = spriteIconLifes;
        CatController.Instance.life+=6;
    }

    public void TakeSpeed()
    {
        AudioManager.Instance.PlayCardSelection();
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.abilityImage.sprite = spriteIconSpeed;
        uiActiveAbility.gameObject.SetActive(true);
        PlayerMovementController.Instance.maxSpeed *= 1.33f;
        PlayerMovementController.Instance.acceleration *= 1.25f;
        CatController.Instance.maxMass *= 1.33f;
    }

    public void TakeSteering()
    {
        AudioManager.Instance.PlayCardSelection();
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.gameObject.SetActive(true);
        uiActiveAbility.abilityImage.sprite = spriteIconSteering;
        CatController.Instance.maxMass *= 3.5f;
    }
}