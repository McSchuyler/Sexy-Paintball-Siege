using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    public Texture2D mouseCursor;

    public Slider bossBar;
    public Slider playerBar;

    public GameObject panel;
    public GameObject winText;
    public GameObject winSprite;
    public GameObject loseText;
    public GameObject loseSprite;

    void Start()
    {
        
        Cursor.SetCursor(mouseCursor,Vector2.zero,CursorMode.Auto);
    }

    void Update()
    {
        //check if player hp less than 0 
        if(playerBar.value <= bossBar.minValue)
        {
            //exec lose window
            panel.SetActive(true);
            loseText.SetActive(true);
            loseSprite.SetActive(true);
            AssignButtonListner();
        }

        //check if boss relationship bar equals or more than 100
        if(bossBar.value >= bossBar.maxValue)
        {
            //exec win window
            panel.SetActive(true);
            winText.SetActive(true);
            winSprite.SetActive(true);
            AssignButtonListner();
        }
    }

    void AssignButtonListner()
    {
        GameObject resetButtonObj = GameObject.FindGameObjectWithTag("Reset");
        Button resetButton = resetButtonObj.GetComponent<Button>();
        resetButton.onClick.AddListener(Reset.instance.Restart);
        Debug.Log("Obj " + resetButtonObj + " button " + resetButton);
    }
}
