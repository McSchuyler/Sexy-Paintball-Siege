using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageUpdate : MonoBehaviour {
    public Boss boss;
    public Slider bossRelationMeter;
    public Text percentText;

    int CalculatePercentage()
    {
        return Mathf.RoundToInt(bossRelationMeter.value / bossRelationMeter.maxValue * 100);
    }

    void Update()
    {
        percentText.text = CalculatePercentage().ToString();
    }
}
