using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class MagnetAbility : ActiveAbility
{
    public float range = 5f;

    protected override void ActivateAbility()
    {
        CatController catController = CatController.Instance;
        foreach (var npc in LevelManager.Instance.npcsFood)
        {
            if (Vector2.Distance(npc.transform.position, catController.transform.position) < range)
            {
                Vector2 dir = (npc.transform.position - catController.transform.position).normalized;
                npc.transform.DOLocalMove(-dir, 0.4f).SetRelative(true);
            }
        }
        // move away hunters
        foreach (var npc in LevelManager.Instance.npcsHunt)
        {
            if (Vector2.Distance(npc.transform.position, catController.transform.position) < range)
            {
                Vector2 dir = (npc.transform.position - catController.transform.position).normalized;
                npc.transform.DOLocalMove(2*dir, 0.4f).SetRelative(true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
