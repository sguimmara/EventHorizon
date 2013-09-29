using UnityEngine;
using System.Collections;

public abstract class Ship : Mobile
{
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    public void FirePrimary(Vector3 startingPosition)
    {
        if (primaryWeapon != null)
        {
            primaryWeapon.Fire(startingPosition);
        }
        else Debug.LogWarning("Primary is null");
    }

    public override string ToString()
    {
        return "Ship";
    }
}
