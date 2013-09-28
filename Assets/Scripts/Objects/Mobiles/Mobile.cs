﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizonGame;

public abstract class Mobile : MonoBehaviour
{
    public string Name;
    protected Rect Size;
    public int Depth;

    public MobileData data;

    public MotionParameters motionParams;

    Vector3 lastPos;

    public GameObject Model { get; protected set; }

    public static Rect GetRectSize(GameObject model)
    {
        Bounds b = model.GetComponent<MeshRenderer>().bounds;
        Rect size = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

        return size;
    }

    public void SetModel(GameObject g)
    {
        Model = g;
        Size = GetRectSize(Model);

        if (EventHorizon.Instance.USE_PLACEHOLDERS)
        {
            Model.renderer.sharedMaterial = EventHorizon.Instance.PLACEHOLDER;
        }
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
            motionParams.Velocity *= motionParams.Inertia;
            Model.transform.Translate(motionParams.Velocity, Space.World);

            lastPos = Model.transform.position;
            EnforceDepth();
            DestroyWhenOutOfSpawnArea();

            if (data.hp == 0)
                Destroy(Model);
        }
        else Debug.LogWarning("Mobile - Update() - Model is null");
    }

    void EnforceDepth()
    {
        Vector3 p = Model.transform.position;
        Model.transform.position.Set(p.x, p.y, Depth);
    }

    public override string ToString()
    {
        return Name;
    }

    public virtual void Collide(Mobile other)
    {
        if (data.isDestroyable)
        {
            data.hp -= (other.data.hp);
        }
    }

    void Awake()
    {
        motionParams = new MotionParameters { MaxSpeed = 0, Inertia = 0F, Acceleration = 0F, Velocity = Vector3.zero, CurrentSpeed = 0 };
    }

    // Destroy the mobile when its rectangle is *totally* out of Spawn area.
    private void DestroyWhenOutOfSpawnArea()
    {
        if (Model.transform.position.x < Globals.SpawnArea.x - Size.width / 2
            || Model.transform.position.x > Globals.SpawnArea.x + Globals.SpawnArea.width + Size.width / 2
            || Model.transform.position.y < Globals.SpawnArea.y - Size.height / 2
            || Model.transform.position.y > Globals.SpawnArea.y + Globals.SpawnArea.height + Size.height / 2)
        {
            Destroy(Model);
            Destroy(this);
        }
    }

    protected virtual void Start()
    {
        Depth = 0;
    }
}

