using UnityEngine;
using System.Collections;

public abstract class Usable : MonoBehaviour
{
    protected Ship ship;

    public void SetShip(Ship ship)
    {
        this.ship = ship;
    }
}
