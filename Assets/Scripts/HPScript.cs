using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = CatController.Instance.life.ToString();
    }
}
