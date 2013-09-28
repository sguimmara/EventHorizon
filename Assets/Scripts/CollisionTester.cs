using UnityEngine;
using System.Collections;

public class CollisionTester : MonoBehaviour
{
    public Mobile parent;

    void OnTriggerEnter(Collider other)
    {
        CollisionTester otherCollider = other.gameObject.GetComponent<CollisionTester>();
        parent.Collide(otherCollider.parent);
    }
}
