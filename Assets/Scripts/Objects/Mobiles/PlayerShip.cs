using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizonGame;
using EventHorizonGame.Graphics;

public class PlayerShip : Ship
{
    //Weapon secondary;

    void LimitShipPositionWithinBoundaries()
    {
        if (Model != null)
        {
            Vector3 pos = Model.transform.position;
            Rect wb = Globals.GameArea;

            float newX = Mathf.Clamp(pos.x, wb.x + Size.width / 2, wb.x + wb.width - Size.width / 2);
            float newY = Mathf.Clamp(pos.y, wb.y + Size.height / 2, wb.y + wb.height - Size.height / 2);
            Model.transform.position = new Vector3(newX, newY, 0);
        }
    }

    protected override void Start()
    {
        base.Start();
        primaryWeapon = Model.AddComponent<Gun>();
        primaryWeapon.SetShip(this);
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
