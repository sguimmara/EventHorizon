using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizonGame;
using EventHorizonGame.Graphics;
using EventHorizonGame.Items;

public class PlayerShip : Ship, IControlable
{
    void LimitShipPositionWithinBoundaries()
    {
        Vector3 pos = transform.position;
        Rect wb = Globals.GameArea;

        float newX = Mathf.Clamp(pos.x, wb.x + Size.width / 2, wb.x + wb.width - Size.width / 2);
        float newY = Mathf.Clamp(pos.y, wb.y + Size.height / 2, wb.y + wb.height - Size.height / 2);
        transform.position = new Vector3(newX, newY, 0);
    }

    protected override void Start()
    {
        base.Start();
        motionParams.Direction = Vector3.zero;
        motionParams.CurrentSpeed = 0F;
    }

    public override string ToString()
    {
        return "PlayerShip";
    }

    protected override void Update()
    {
        base.Update();
        LimitShipPositionWithinBoundaries();
        UserControl();
    }

    public void UserControl()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("down");
            Move(Vector3.down);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(Vector3.up);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Trigger();
        }
    }
}
