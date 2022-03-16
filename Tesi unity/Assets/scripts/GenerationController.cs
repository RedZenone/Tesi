using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationController : MonoBehaviour
{
    private float time;
    void Start()
    {
        time=0f;
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
    }
}
