using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : Ship
{
    Weapon secondary;

    public override void LoadDefaultModel()
    {
        GameObject model = Utils.Load<GameObject>("Mobiles/Ships/Player/HarbingerOfDeath");

        if (model == null)
            Debug.LogWarning("HarbingerOfDeath - LoadDefaultModel() - Null");

        SetModel(model);
        primary = gameObject.AddComponent<Gun>();
        primary.SetShip(this);
    }

    void LimitShipPositionWithinBoundaries()
    {

    }

    public override string ToString()
    {
        return "PlayerShip";
    }

    protected override void Update()
    {
        base.Update();
        LimitShipPositionWithinBoundaries();
    }
}
