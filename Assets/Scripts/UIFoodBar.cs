using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFoodBar : MonoBehaviour
{

    public Slider slider;
    // Update is called once per frame
    void Update()
    {
        slider.value = CatController.Instance.food / CatController.Instance.foodNeeded;
    }
}
