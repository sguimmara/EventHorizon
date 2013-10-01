using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizonGame;
using EventHorizonGame.Data;
using EventHorizonGame.Graphics;
using EventHorizonGame.FX;

public enum EventType { Explosion, Shoot };

public abstract class Mobile : MonoBehaviour
{
    public string Name;
    protected Rect Size;
    public int ScreenDepth;
    public bool NoCollision;
    public bool AutoTrigger;

    public Properties data;
    public Movement motionParams;
    public SpriteSlots Sprites;

    public GameObject Model { get; protected set; }

    public event EventHandler OnMobileExplosion;
    public event EventHandler OnMobileShoot;

    protected void TriggerEvent(Mobile sender, EventType type, MobileArgs args)
    {
        //        switch (type)
        //        {
        //            case EventType.Explosion: OnMobileExplosion(sender, args);
        //                break;
        //            case EventType.Shoot: OnMobileShoot(sender, args);
        //                break;
        //            default:
        //                break;
        //        }
    }

    public Rect GetRectSize()
    {
        Bounds b = gameObject.GetComponent<MeshRenderer>().bounds;
        Rect size = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

        return size;
    }

    public void Move(Vector3 direction)
    {
        motionParams.Direction = Vector3.Normalize (motionParams.Direction += direction);
        Accelerate();
    }

    public void Stop()
    {
        motionParams.Velocity = Vector3.zero;
    }

    protected virtual void Update()
    {
        UpdatePosition();

        EnforceDepth();
        DestroyWhenOutOfVoidArea();

        if (data.currentHP <= 0 && !data.Indestructible)
        {
            //                TriggerEvent(this, EventType.Explosion, new MobileArgs { mobile = this, explosionEffect = "", shootEffect = "" });
            //                Pool.Instance.CreateDecal("Explosion2", Model.transform.position, 1F, 4F, 5F);
            //                Destroy(Model);
        }
    }

    public void Accelerate()
    {
        motionParams.CurrentSpeed += motionParams.Acceleration * Time.deltaTime;
    }

    protected void UpdatePosition()
    {
        motionParams.CurrentSpeed = Mathf.Clamp(motionParams.CurrentSpeed, 0, motionParams.MaxSpeed);
        motionParams.Inertia = motionParams.Inertia <= 0 ? 0.1F : motionParams.Inertia;
        
        motionParams.CurrentSpeed *= (1 / motionParams.Inertia);
        motionParams.Velocity = motionParams.Direction * motionParams.CurrentSpeed;

        //transform.position = new Vector3(transform.position.x + motionParams.Velocity.x, transform.position.y + motionParams.Velocity.y, transform.position.z);
        transform.Translate(motionParams.Velocity.x, motionParams.Velocity.y, 0);
    }

    void EnforceDepth()
    {
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, p.y, (float)ScreenDepth);
    }

    public override string ToString()
    {
        return Name;
    }

    public virtual void Collide(Mobile other)
    {
        if (!data.Indestructible)
        {
            data.currentHP -= (other.data.currentHP);
        }
    }

    protected virtual void Awake()
    {
        data.currentHP = data.maxHP;
        Size = GetRectSize();
    }

    // Destroy the mobile when its rectangle is *totally* out of Spawn area.
    private void DestroyWhenOutOfVoidArea()
    {
        if (this.transform.position.x < Globals.VoidArea.x - Size.width / 2
            || this.transform.position.x > Globals.VoidArea.x + Globals.VoidArea.width + Size.width / 2
            || this.transform.position.y < Globals.VoidArea.y - Size.height / 2
            || this.transform.position.y > Globals.VoidArea.y + Globals.VoidArea.height + Size.height / 2)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void Start()
    {
        ScreenDepth = 0;
        motionParams.Direction = transform.right;
        motionParams.CurrentSpeed = motionParams.MaxSpeed; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!NoCollision)
        {
            if (this.tag != other.tag)
            {
                Debug.Log("collide");
                Collide(other.GetComponent<Mobile>());
            }
        }
    }

    void OnGUI()
    {
        string s = string.Concat(
            "Speed: ", motionParams.CurrentSpeed,
            " Acc: ", motionParams.Acceleration,
            " X: ", motionParams.Direction.x,
            " Y: ", motionParams.Direction.y);

        GUI.Label(new Rect(0, 0, 200, 20), s);
    }
}

