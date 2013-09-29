using EventHorizonGame;
using EventHorizonGame.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Gun : Weapon
{
    protected override void TriggerWeapon(Vector3 startingPosition)
    {
        int depth = 2;
        Projectile p = Pool.Instance.Create<Projectile>("Shell", new Vector3(startingPosition.x, startingPosition.y, depth), "Mobiles/Projectiles/Shell");
        p.motionParams.Velocity = Vector3.right;
        //p.motionParams = new Movement { Velocity = new Vector3(0.5F, 0, 0), Acceleration = 1F, Inertia = 1F, MaxSpeed = 1F, CurrentSpeed = 0 };
        //p.data = new Properties { damage = 0, currentHP = 1, isDestroyable = false };
        //p.ScreenDepth = depth;
    }
}

