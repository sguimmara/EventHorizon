using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Linq.Expressions;

static class Utils
{
   static void Break()
    {
        if (DebuggerGlobals.debuggerLevel == DebuggerTolerance.LowTolerance)
            Debug.Break();

        else if (DebuggerGlobals.debuggerLevel == DebuggerTolerance.NoTolerance)
            Debug.DebugBreak();
    }
 
    public static T Load<T>(string s) where T : UnityEngine.Object
    {
        T result = (T)Resources.Load(s);

        if (result == null)
            Debug.LogWarning("No object of type " + typeof(T) + " found at " + s);

        return result;
    }


}
