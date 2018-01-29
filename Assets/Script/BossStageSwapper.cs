using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageSwapper : MonoBehaviour {
    public Image firstStageSprite;
    public Image secondStageSprite;
    public Image thirdStageSprite;
    public Image fourthStageSprite;

    public float delayToToggleSprite = 1.0f;
    public float delayToEnablePlayerController = 1.0f;

    private Stage currentStage;
    private Boss boss;

    public Player player;

    void Start()
    {
        boss = GetComponent<Boss>();
    }

    void Update()
    {
        if(currentStage != boss.currentStage)
        {
            //play animation and change sprite
            StartCoroutine(ToggleBossState());
            currentStage = boss.currentStage;
        }
    }

    IEnumerator ToggleBossState()
    {
        //disable player control
        player.isAbleToAttack = false;
        //play animation

        //change boss sprite
        yield return new WaitForSeconds(delayToToggleSprite);

        if (boss.currentStage == Stage.second)
            firstStageSprite.gameObject.SetActive(false);

        if (boss.currentStage == Stage.third)
        {
            firstStageSprite.gameObject.SetActive(false);
            secondStageSprite.gameObject.SetActive(false);
        }
            if (boss.currentStage == Stage.fourth)
        {
            firstStageSprite.gameObject.SetActive(false);
            secondStageSprite.gameObject.SetActive(false);
            thirdStageSprite.gameObject.SetActive(false);
        }

        //Enable player control
        yield return new WaitForSeconds(delayToEnablePlayerController);
        player.isAbleToAttack = true;
    }
}
