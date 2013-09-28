using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum DebuggerTolerance { NoTolerance, LowTolerance, Ignore };

static class DebuggerGlobals
{
    public static DebuggerTolerance debuggerLevel = DebuggerTolerance.NoTolerance;
}

