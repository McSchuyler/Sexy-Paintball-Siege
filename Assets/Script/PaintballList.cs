using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintballList : MonoBehaviour {

    [System.Serializable]
    public struct Paintball
    {
        public string name;
        public Sprite paintballSprite;
    }

    public Paintball[] allPaintballList;
}
