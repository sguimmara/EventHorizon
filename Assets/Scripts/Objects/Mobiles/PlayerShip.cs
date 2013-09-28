using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizonGame;

public class PlayerShip : Ship
{
    Weapon secondary;

    public override void LoadDefaultModel()
    {
        GameObject model = Utils.Load<GameObject>("Mobiles/Ships/Player/HarbingerOfDeath");

        if (model == null)
            Debug.LogWarning("HarbingerOfDeath - LoadDefaultModel() - Null");

        SetModel(model, EventHorizon.Instance.STARTING_POSITION);
        primary = gameObject.AddComponent<Gun>();
        primary.SetShip(this);
    }

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
