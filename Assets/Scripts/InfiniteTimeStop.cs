using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTimeStop : MonoBehaviour
{
    void Update()
    {
        Time.timeScale = 0;
    }
}
