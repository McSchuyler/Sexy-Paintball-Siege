using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset : MonoBehaviour {

    public static Reset instance;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level_001");
        //GameObject resetButtonObj = GameObject.FindGameObjectWithTag("Reset");
        //Button resetButton = resetButtonObj.GetComponent<Button>();
        //resetButton.onClick.AddListener(Restart);
        TargetsGenerator.instance.Restart();
    }
}
