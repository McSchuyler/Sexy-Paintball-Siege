using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintballHolder : MonoBehaviour {

    private string mainPaintball;
    private string subPaintball;

    public Image mainImage;
    public Image subImage;

    public PaintballList paintballList;

    public string GetMainColor() {return mainPaintball;}

    void Start()
    {
        GenerateNewPaintball(out mainPaintball, ref mainImage);
        GenerateNewPaintball(out subPaintball, ref subImage);

        //add to shoot event to replace the main ball
        PaintballShooter.OnShoot += ReplaceMainPaintball;
    }

    void GenerateNewPaintball(out string paintball, ref Image paintballImage )
    {
        //Random new paintball color
        int maxNumPaintball = paintballList.allPaintballList.Length;
        int randomNumber = Random.Range(0, maxNumPaintball);

        //replace sub paint ball color
        paintball = paintballList.allPaintballList[randomNumber].name;
        paintballImage.sprite = paintballList.allPaintballList[randomNumber].paintballSprite;
    }

    public void ReplaceMainPaintball()
    {
        mainPaintball = subPaintball;
        mainImage.sprite = subImage.sprite;
        //generate new sub paintball
        GenerateNewPaintball(out subPaintball, ref subImage);
    }
}
