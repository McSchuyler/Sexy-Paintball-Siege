using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintballShooter : MonoBehaviour {

    public delegate void ShootAction();
    public static event ShootAction OnShoot;

}
