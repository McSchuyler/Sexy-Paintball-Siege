using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAttacker : MonoBehaviour {

    public int numberOfTurns;
    private int currentNumberOfTurns;

    public GameObject bullet;
    public GameObject buttonBlocker;
    public int attackDamage;
    public Text turnCounter;
    public float delayToResetCounter = 2.0f;

    public Player player;

    void Start()
    {
        //set turn counter
        currentNumberOfTurns = numberOfTurns;
        SetTurnCounter();
    }

    public void DecreaseTurn()
    {
        currentNumberOfTurns--;
        SetTurnCounter();
        //check if is boss turn to attack
        if (currentNumberOfTurns <= 0)
        {
            Attack();
        }
    }

    public void Attack()
    {
        //block player button
        buttonBlocker.SetActive(true);
        //spawn bullet
        GameObject hitter = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BossHit bossHit = hitter.GetComponent<BossHit>();
        bossHit.OnHitEvent.AddListener(DamagePlayer);
        bossHit.OnHitEvent.AddListener(ResetTurnCounter);
    }

    //damage player health
    void DamagePlayer()
    {
        AudioManager.instance.PlaySound2D("BossAttack");
        player.curretHealth -= attackDamage;
    }

    void ResetTurnCounter()
    {
        StartCoroutine(WaitToResetCounter());
    }

    //have a delay before reset counter
    IEnumerator WaitToResetCounter()
    {
        yield return new WaitForSeconds(delayToResetCounter);
        currentNumberOfTurns = numberOfTurns;
        SetTurnCounter();
        //Disable button blocker
        buttonBlocker.SetActive(false);
    }

    //set turn counter to current turn
    void SetTurnCounter()
    {
        turnCounter.text = currentNumberOfTurns.ToString();
    }
}
