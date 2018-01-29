using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsingColor : MonoBehaviour {

    public Gradient grad;

    public float time;

    public Image img;

    void Update()
    {
        img.color = grad.Evaluate(Mathf.PingPong(Time.time * 1/time, 1));
    }


}
