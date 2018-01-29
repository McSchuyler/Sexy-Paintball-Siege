using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Unity event
[System.Serializable]
public class HitEvent : UnityEvent {}

public class BossHit : MonoBehaviour {

    public HitEvent OnHitEvent;

    public float delayToSelfDestory = 1.0f;

    //Setting Unity Event
    void Start()
    {
        if (OnHitEvent == null)
            OnHitEvent = new HitEvent();
    }

    //Called by animation event
    void PlayHitEvent()
    {
        OnHitEvent.Invoke();
        Destroy(gameObject, delayToSelfDestory);
    }
}
