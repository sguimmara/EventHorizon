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
        Destroy(this.gameObject);
    }

}

