using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Mobile : MonoBehaviour
{
    public string Name { get; protected set; }

    protected Vector3 Velocity;
    protected float Speed = 1f;

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

    public void Move(Vector3 offset)
    {
        Velocity += offset;
    }

    public void Stop()
    {
        Velocity = Vector3.zero;
    }

    public void SetSpeed(float speed)
    {
        this.Speed = speed;
    }

    public void SetSpeed(float speed, float time)
    {
        StartCoroutine(Accelerate(speed, time));
    }

    IEnumerator Accelerate(float newSpeed, float duration)
    {
        StopCoroutine("Accelerate");
        float f = 0;
        float originalSpeed = this.Speed;

        while (this.Speed <= newSpeed && f <= duration)
        {
            f += Time.deltaTime;

            this.Speed = Mathf.Lerp(originalSpeed, newSpeed, f / duration);
            yield return new WaitForEndOfFrame();
        }
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

