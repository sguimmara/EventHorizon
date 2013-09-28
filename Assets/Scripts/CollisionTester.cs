using UnityEngine;
using System.Collections;

public class CollisionTester : MonoBehaviour
{

    public Mobile parent;

    void OnCollisionEnter(Collision collider)
    {
        CollisionTester otherCollider = collider.gameObject.GetComponent<CollisionTester>();

        if (otherCollider == null)
            Debug.LogError("Other collider doesn't have CollisionTester Behaviour");
        else if (otherCollider.parent == null)
            Debug.LogWarning("Other collider.parent is null");
        else
            parent.Collide(otherCollider.parent);

    }

}
