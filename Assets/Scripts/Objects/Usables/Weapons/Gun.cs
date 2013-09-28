using EventHorizonGame;
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
        GameObject projectile = (GameObject) GameObject.Instantiate((GameObject)Resources.Load("Mobiles/Projectiles/Shell"));
        Projectile m = projectile.AddComponent<Projectile>();
        m.motionParams = new MotionParameters { Velocity = new Vector3(1, 0, 0), Acceleration = 1, Inertia = 1F, MaxSpeed = 1, CurrentSpeed = 0 };
        m.SetModel(projectile, startingPosition, false);
    }
}

