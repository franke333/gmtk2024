using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : MonoBehaviour
{

    UIActiveAbility uiActiveAbility;

    public Sprite spriteIconSpeed,spriteIconSteering,spriteIconLifes;


    public void TakeLife()
    {
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.abilityImage.sprite = spriteIconLifes;
        CatController.Instance.life+=6;
    }

    public void TakeSpeed()
    {
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.abilityImage.sprite = spriteIconSpeed;
        PlayerMovementController.Instance.maxSpeed *= 1.33f;
        PlayerMovementController.Instance.acceleration *= 1.25f;
    }

    public void TakeSteering()
    {
        uiActiveAbility = UIActiveAbility.InstancePassive;
        uiActiveAbility.abilityImage.sprite = spriteIconSteering;
        CatController.Instance.maxMass *= 2.5f;
    }
}