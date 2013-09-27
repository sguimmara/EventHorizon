using UnityEngine;
using System.Collections;

public abstract class Ship : Mobile
{
    protected Weapon primary;

    protected float HitPoints;

    public void FirePrimary()
    {
        if (primary != null)
        {
            primary.Fire();
        }
        else Debug.LogWarning("Primary is null");
    }

    public override string ToString()
    {
        return "Ship";
    }
}
