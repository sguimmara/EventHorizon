using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventHorizon : MonoBehaviour
{
    Player player;

    List<Mobile> mobiles;

    void Start()
    {
        mobiles = new List<Mobile>();
        player = gameObject.AddComponent<Player>();
    }

    public void AddMobile(Mobile m)
    {
        if (mobiles.Contains(m))
            Debug.LogWarning("EventHorizon - Le mobile " + m + " est déjà présent dans la liste");

        else mobiles.Add(m);
    }
	
		void Update()
	{
		
	}
}

