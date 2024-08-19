using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorPartScript : MonoBehaviour
{

    private void Start()
    {
        DoSomethingCreepy();
    }

    private Ease RandomEase()
    {
        int ease = Random.Range(0, 5);
        switch (ease)
        {
            case 0:
                return Ease.InOutCirc;
            case 1:
                return Ease.InCubic;
            case 2:
                return Ease.OutCubic;
            case 3:
                return Ease.InOutSine;
            case 4:
                return Ease.InExpo;
            default:
                return Ease.InOutBack;
        }
    }
    private void DoSomethingCreepy()
    {
        float time = Random.Range(1.5f, 4f);
        int tranformation = Random.Range(0, 3);
        switch (tranformation)
        {
            case 0:
                float scale = Random.Range(0.5f, 1.5f);
                transform.DOScale(scale, time).SetRelative(false).SetEase(RandomEase()).OnComplete(() => DoSomethingCreepy());
                break;
            case 1:
                time *= 3;
                float rotation = Random.Range(0, 360);
                var rot = Quaternion.AngleAxis(rotation, Vector3.forward);
                transform.DOLocalRotate(rot.eulerAngles, time).SetEase(RandomEase()).OnComplete(() => DoSomethingCreepy());
                break;
            case 2:
                Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * 2.5f;
                transform.DOLocalMove(offset, time).SetRelative(false).SetEase(RandomEase()).OnComplete(() => DoSomethingCreepy());
                break;
        }
    }
}
