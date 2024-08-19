using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageScript : MonoBehaviour
{
    public static List<FoliageScript> instances;
    public SpriteRenderer sr;

    public static void Respawn()
    {
        if (instances == null)
            return;
        foreach(FoliageScript foliage in instances)
        {
            if(foliage == null)
            {
                return;
            }
            foliage.sr.sprite = LevelManager.Instance.GetFoliage();
            float angle = Random.Range(0, 2 * Mathf.PI);
            float distance = Random.Range(0, LevelManager.Instance.maxRange);
            foliage.transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance + CatController.Instance.transform.position;
        }
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = -2;
        if (instances == null)
            instances = new List<FoliageScript>();
        instances.Add(this);
    }

    private void Update()
    {
        if(Vector3.Distance(CatController.Instance.transform.position,transform.position) > LevelManager.Instance.maxRange)
        {
            sr.sprite = LevelManager.Instance.GetFoliage();
            float angle = Random.Range(0, 2 * Mathf.PI);
            transform.position = new Vector3(Mathf.Cos(angle) * LevelManager.Instance.minRange, Mathf.Sin(angle) * LevelManager.Instance.minRange, 0) + CatController.Instance.transform.position;
        }
    }
}
