using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizonGame;
using EventHorizonGame.Data;

public enum EventType { Explosion, Shoot };

public abstract class Mobile : MonoBehaviour
{
    public string Name;
    protected Rect Size;
    public int ScreenDepth;

    public Properties data;
    public Movement motionParams;
    public GameObject ExplosionSprite;

    public GameObject Model { get; protected set; }

    public event EventHandler OnMobileExplosion;
    public event EventHandler OnMobileShoot;

    protected void TriggerEvent(Mobile sender, EventType type, MobileArgs args)
    {
        switch (type)
        {
            case EventType.Explosion: OnMobileExplosion(sender, args);
                break;
            case EventType.Shoot: OnMobileShoot(sender, args);
                break;
            default:
                break;
        }
    }

    public static Rect GetRectSize(GameObject model)
    {
        Bounds b = model.GetComponent<MeshRenderer>().bounds;
        Rect size = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

        return size;
    }

    public void Move(Vector3 normalizedDirection)
    {
        motionParams.Velocity += (normalizedDirection * Time.deltaTime * motionParams.Acceleration);
        Mathf.Clamp(motionParams.Velocity.magnitude, 0, motionParams.MaxSpeed);
    }

    public void Stop()
    {
        motionParams.Velocity = Vector3.zero;
    }

    protected virtual void Update()
    {
        if (Model != null)
        {
            if (motionParams.Velocity.magnitude <= motionParams.MaxSpeed)
            {
                motionParams.Velocity *= motionParams.Inertia;
            }

            else motionParams.Velocity *= (motionParams.MaxSpeed / motionParams.Velocity.magnitude);

            Model.transform.Translate(motionParams.Velocity, Space.World);

            EnforceDepth();
            DestroyWhenOutOfVoidArea();

            if (data.currentHP <= 0 && !data.Indestructible) 
            {
                TriggerEvent(this, EventType.Explosion, new MobileArgs { mobile = this, explosionEffect = "", shootEffect = "" });
                Pool.Instance.CreateDecal("Explosion2", Model.transform.position, 1F, 4F, 5F);
                Destroy(Model);
            }
        }
        else Debug.LogWarning("Mobile - Update() - Model is null");
    }

    void EnforceDepth()
    {
        Vector3 p = Model.transform.position;
        Model.transform.position = new Vector3(p.x, p.y, (float)ScreenDepth);
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

    void Awake()
    {
        Model = gameObject;
        data.currentHP = data.maxHP;
    }

    // Destroy the mobile when its rectangle is *totally* out of Spawn area.
    private void DestroyWhenOutOfVoidArea()
    {
        if (Model.transform.position.x < Globals.VoidArea.x - Size.width / 2
            || Model.transform.position.x > Globals.VoidArea.x + Globals.VoidArea.width + Size.width / 2
            || Model.transform.position.y < Globals.VoidArea.y - Size.height / 2
            || Model.transform.position.y > Globals.VoidArea.y + Globals.VoidArea.height + Size.height / 2)
        {
            Destroy(Model);
        }
    }

    protected virtual void Start()
    {
        ScreenDepth = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.tag != other.tag)
        {
            Debug.Log("collide");
            Collide(other.GetComponent<Mobile>());
        }
    }
}

