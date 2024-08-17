using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool isHunter = false;
    public float speed = 5f;
    public int food = 1;

    public bool flipHorizontal = false;

    private void Update()
    {
        Vector3 ogPos = transform.position;
        Vector3 target = new Vector3(CatController.Instance.transform.position.x, CatController.Instance.transform.position.y, transform.position.z);
        if (isHunter)
        {
            transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
        }
        else if(Vector3.Distance(transform.position, target) < 8f)
        {
            transform.position += (transform.position - target).normalized * speed * Time.deltaTime;
        }

        Vector3 delta = transform.position - ogPos;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //flipping
        if (flipHorizontal)
        {
            if(delta.x > 0)
                sr.flipX = true;
            else if(delta.x < 0)
                sr.flipX = false;
        }
        else
        {
            if(delta.y > 0)
                sr.flipY = false;
            else if(delta.y < 0)
                sr.flipY = true;
        }
    }
}
