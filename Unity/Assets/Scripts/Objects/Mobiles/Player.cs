using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.Objects;

namespace EventHorizon.Objects
{
    public class Player : Ship, IPlayable
    {
        public CharacterSheet characterSheet;

        void LimitShipPositionWithinBoundaries()
        {
            Vector3 pos = transform.position;
            Rect wb = Globals.GameArea;

            float newX = Mathf.Clamp(pos.x, wb.x + Size.width / 2, wb.x + wb.width - Size.width / 2);
            float newY = Mathf.Clamp(pos.y, wb.y + Size.height / 2, wb.y + wb.height - Size.height / 2);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }

        protected override void Start()
        {
            base.Start();
            Direction = Vector3.zero;
            CurrentSpeed = 0F;
            IsPlayable = true;
        }

        protected override void Update()
        {
            base.Update();
            LimitShipPositionWithinBoundaries();

            if (IsPlayable)
                Control();
        }

        public void Control()
        {
            if (Input.GetKey(KeyCode.S))
                Move(Vector3.down);

            if (Input.GetKey(KeyCode.Z))
                Move(Vector3.up);

            if (Input.GetKey(KeyCode.Q))
                Move(Vector3.left);

            if (Input.GetKey(KeyCode.D))
                Move(Vector3.right);

            if (Input.GetMouseButton(0)) 
                Trigger();
        }

        public override string ToString()
        {
            return "PlayerShip";
        }

        public bool IsPlayable { get; set; }
    }
}