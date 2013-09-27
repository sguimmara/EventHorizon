using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Pool : ScriptableObject
{
    Pool instance
    {
        get
        {
            if (instance == null)
                return new Pool();
            else return instance;
        }
    }

//    public Dictionary<string, GameElement> projectiles { get; private set; }
//    public Dictionary<string, GameElement> enemyShips { get; private set; }
//    public Dictionary<string, GameElement> playerShips { get; private set; }
//
//
//    private Pool()
//    {
//        projectiles = new Dictionary<string, GameElement>();
//        enemyShips = new Dictionary<string, GameElement>();
//        playerShips = new Dictionary<string, GameElement>();
//    }
}

