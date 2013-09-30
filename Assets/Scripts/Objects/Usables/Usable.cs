using UnityEngine;
using System.Collections;

public abstract class Usable : MonoBehaviour
{
    public abstract void Trigger();
    public abstract void Initialize();
    public bool Active;
}
