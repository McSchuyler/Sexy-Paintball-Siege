﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHolder : MonoBehaviour
{
    public int x;
    public int y;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TargetHit()
    {
        TargetsGenerator.instance.ChangeColor(x, y);
    }
}
