using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Mobile : MonoBehaviour
{
    public string Name { get; protected set; }

    protected Vector3 Velocity = Vector3.zero;
    protected float MaxSpeed = 10f;
    protected float Acceleration = 0.9F;
    protected bool isAccelerating = false;
    protected float Inertia = 0.9F;
    float currentSpeed = 0;

    Vector3 lastPos;

    public GameObject Model { get; protected set; }

    public virtual void LoadDefaultModel()
    {
        SetModel(Utils.Load<GameObject>("DefaultMobile"));
    }

    public void SetModel(GameObject g)
    {
        if (Model != null)
            GameObject.DestroyImmediate(Model);

        Model = (GameObject)GameObject.Instantiate(g);
        Model.gameObject.GetComponent<CollisionTester>().parent = this;
    }

    public void Move(Vector3 normalizedDirection)
    {
        Velocity += (normalizedDirection * Time.deltaTime * Acceleration);
        Mathf.Clamp(Velocity.magnitude, 0, MaxSpeed);
    }

    public void Stop()
    {
        Velocity = Vector3.zero;
    }

    void Update()
    {
        Velocity *= Inertia;
        //if (!isAccelerating)
        //    Velocity.Scale( += (normalizedDirection * Time.deltaTime * Acceleration);
        Model.transform.Translate(Velocity, Space.World);
        lastPos = Model.transform.position;
    }

    public override string ToString()
    {
        return Name;
    }

    public virtual void Collide(Mobile other)
    {
        Debug.Break();
    }

    public virtual void HitVoid()
    {
        Debug.Log("HitVoid");
    }


}

