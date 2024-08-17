using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : SingletonClass<CameraFollowScript>
{
    Vector3 shakeOffset = Vector3.zero;
    float duration = 0;
    float time = 10;

    Vector3 pos;

    public void Shake(float mass)
    {
        if (time < duration)
            return;
        shakeOffset = Random.insideUnitCircle * 0.1f * mass;
        shakeOffset.z = 0;
        duration = 0.15f + 0.05f*mass;
        time = 0;
        Debug.Log("Shake");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > duration)
        {
            shakeOffset = Vector3.zero;
        }
        Vector3 target = new Vector3(CatController.Instance.transform.position.x, CatController.Instance.transform.position.y, transform.position.z);
        pos = pos + (target - pos) * 5f * Time.deltaTime;
        pos.z = -10;
        transform.position = pos + shakeOffset * 2*(0.5f-Mathf.Abs(duration/2-time));
    }
}
