using EventHorizonGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Projectile : Mobile
{
    protected override void Start()
    {
        Depth = 2;
    }

    public override void Collide(Mobile other)
    {
        Pool.Instance.CreateDecal("Explosion", Model.transform.position, 0.1F, 0.5F, 1.5F);
        Destroy(this.gameObject);        
    }
}

