using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Weapon : Usable
{
    protected float rateOfFire = 0.05F;
    protected float damage = 1F;
    protected float lastShot;
    protected float speed = 50;

    public void Fire(Vector3 startingPosition)
    {
        if (Time.time - lastShot >= rateOfFire)
        {
            TriggerWeapon(startingPosition);
            lastShot = Time.time;
        }
    }

    protected virtual void TriggerWeapon(Vector3 startingPosition)
    {
    }

    void Start()
    {
        lastShot = Time.time;
    }
}

