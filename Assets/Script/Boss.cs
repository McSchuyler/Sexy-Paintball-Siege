///Fill up boss love meter depend on type and attribute, change boss stage depend on boss meter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stage { first, second, third, fourth };

public class Boss : MonoBehaviour {

    public int maxRelation;
    public int currentRelation;

    public float firstStagePercentage = 0.15f;
    public float secondStagePercentage = 0.35f;
    public float thirdStagePercentage = 0.60f;

    public Slider bossLoveMeter;

    public Stage currentStage;

    public float healPercentage;
    public float pinkMultiplier;

    public ParticleSystem healParticle;

    public Player player;

    [System.Serializable]
    public class BossAttribute
    {
        public string type;
        public float multiplier;
    }

    [Header("Boss Attribute")]
    public BossAttribute[] weakness;
    public BossAttribute[] resistance;

    void Start()
    {
        bossLoveMeter.maxValue = maxRelation; 
        currentRelation = 0;
        currentStage = Stage.first;
        bossLoveMeter.value = currentRelation;
    }

    public void ApplyPaintballEffect(int dmg, string type)
    {
        //check if is pink, if is heal player
        if(type == "Pink")
        {
            //Play particle
            healParticle.Play();
            player.curretHealth = +Mathf.RoundToInt(dmg / healPercentage);
            AudioManager.instance.PlaySound2D("Heal");
            //check if player health exceed max health
            if (player.curretHealth > player.startingHealth)
                player.curretHealth = player.startingHealth;
        }

        float multiplier = CheckDamageMultiplier(type);
        //increase lover meter
        AudioManager.instance.PlaySound2D("BossHurt");
        currentRelation += Mathf.RoundToInt(dmg * multiplier); ;
        //check health gate
        CheckHealthGate();
        //update health bar
        bossLoveMeter.value = currentRelation;
    }

    void CheckHealthGate()
    {
        int count = 0;

        if (GetCurrentrelationPercentage() >= firstStagePercentage)
        {
            currentStage = Stage.second;
            count++;
        } 
        if (GetCurrentrelationPercentage() >= secondStagePercentage)
        {
            currentStage = Stage.third;
            count++;
        }
            
        if (GetCurrentrelationPercentage() >= thirdStagePercentage)
        {
            currentStage = Stage.fourth;
            count++;
        }

        if (count > 0)
        {
            AudioManager.instance.PlaySound2D("BossChangeState");
            count = 0;
        }
            
    }

    float GetCurrentrelationPercentage()
    {
        return (float)currentRelation/maxRelation;
    }

    float CheckDamageMultiplier(string type)
    {
        //check if is pink
        if(type == "Pink")
        {
            return pinkMultiplier;
        }

        //check resistance
        for (int i = 0; i < resistance.Length; i++)
        {
            if (type == resistance[i].type)
                return resistance[i].multiplier;
        }

        //check weakness
        for(int j = 0; j < weakness.Length; j++)
        {
            if (type == weakness[j].type)
                return weakness[j].multiplier;
        }

        return 0.0f;
    }
}
