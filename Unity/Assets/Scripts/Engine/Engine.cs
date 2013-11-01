using EventHorizon.AI;
using EventHorizon.Objects;
using EventHorizon.Sound;
using EventHorizon.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace EventHorizon.Core
{
    public delegate void GameEvent();
    public delegate void EventLevel(Level level);
    public delegate void EventMobile(Mobile sender);

    public class Engine : MonoBehaviour, ISingleton
    {
        public Engine Instance { get; private set; }

        public void Register()
        {
            if (Instance == null)
                Instance = this;
            else Debug.LogError(string.Format("Singleton marked class {0} has more than one instance running.",this.GetType().ToString()));
        }
    }
}

