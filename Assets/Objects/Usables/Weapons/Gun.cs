using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Gun : Weapon
{
    protected override void TriggerWeapon()
    {
        GameObject projectile = (GameObject)GameObject.Instantiate((GameObject) Resources.Load("Mobiles/Projectiles/Shell"), ship.Model.transform.position, new Quaternion());
        (projectile.AddComponent<ProjectileBehaviour>()).info.damage = 5F;
        StartCoroutine(moveObject(projectile, Vector3.right * speed, 20));
    }

    IEnumerator moveObject(GameObject o, Vector3 velocity, float distance)
    {
        Vector3 originalPosition = o.transform.position;

        while (Vector3.Distance(o.transform.position, originalPosition) <= distance)
        {
            o.transform.Translate(velocity * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Destroy(o);
    }
}

