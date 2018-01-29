using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool isAbleToAttack = true;
    public int startingHealth;
    public int curretHealth;

    public Slider playerHealthSlider;

    public PaintballHolder paintballHolder;
    public BossAttacker bossAttacker;

    void Start()
    {
        //set health
        curretHealth = startingHealth;
        playerHealthSlider.maxValue = startingHealth;
    }

    void Update()
    {
        playerHealthSlider.value = curretHealth;
    }

    public void OnShoot()
    {
        //replace main paintball
        paintballHolder.ReplaceMainPaintball();
        //decrease boss turn
        bossAttacker.DecreaseTurn();
    }
}
