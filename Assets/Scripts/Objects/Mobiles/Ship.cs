using UnityEngine;
using System.Collections;

public abstract class Ship : Mobile
{
    protected Weapon primary;

    public void FirePrimary(Vector3 startingPosition)
    {
        if (primary != null)
        {
            primary.Fire(startingPosition);
        }
        else Debug.LogWarning("Primary is null");
    }

    public override string ToString()
    {
        return "Ship";
    }
}
