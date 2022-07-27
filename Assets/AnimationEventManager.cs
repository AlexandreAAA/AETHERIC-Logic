using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveController;
using System;

public class AnimationEventManager : MonoBehaviour
{
    Animator anim;

    public TransformVariable launcher;

    public AnimEvent Fireball1 = new AnimEvent();
    public AnimEvent Fireball2 = new AnimEvent();

    private void Start()
    {
        anim = GetComponent<Animator>();
        InitAnimEvent(Fireball1);
        InitAnimEvent(Fireball2);
    }

    public void LaunchFireball()
    {
        GameObject fireball = PlayerObjectPool.SharedInstance.GetPooledObject(PlayerObjectPool.SharedInstance.fireball);
        fireball.transform.SetPositionAndRotation(launcher.value.position, launcher.value.rotation);
        fireball.SetActive(true);
    }

    private void InitAnimEvent(AnimEvent ae)
    {
        AnimationEvent ev = new AnimationEvent();
        ev.time = ae.addAtTime;
        ev.functionName = ae.functionToRaise;
        ae.animToAddEvent.AddEvent(ev);
    }


}
[Serializable]
public class AnimEvent
{
    public AnimationClip animToAddEvent;
    public float addAtTime;
    public string functionToRaise;
}



