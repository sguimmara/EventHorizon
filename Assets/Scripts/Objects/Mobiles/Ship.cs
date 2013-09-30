using UnityEngine;
using System.Collections;
using EventHorizonGame.Items;

public abstract class Ship : Mobile
{
    public Usable[] Slots;

    public void Trigger()
    {
        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].Active)
            {
                Slots[i].Trigger();
            }
            else Debug.LogWarning(string.Concat("Slot ", i.ToString(), " is null"));
    }

    public override string ToString()
    {
        return "Ship";
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (Usable slot in Slots)
        {
            slot.Initialize();
            slot.Active = true;
        }
    }

    void Update()
    {

        if (AutoTrigger)
            Trigger();
    }
}
