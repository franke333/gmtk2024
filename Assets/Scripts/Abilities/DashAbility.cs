using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : ActiveAbility
{
    protected override void ActivateAbility()
    {
       PlayerMovementController playerMovementController = PlayerMovementController.Instance;
        Vector2 dir = playerMovementController.GetVelocity();
        if(dir.magnitude ==0)
        {
            dir = transform.right;
        }
        dir.Normalize();
        playerMovementController.AddVelocity(dir * 20);
    }
}
