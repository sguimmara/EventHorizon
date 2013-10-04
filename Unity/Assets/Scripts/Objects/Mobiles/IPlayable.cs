using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHorizon
{
    public interface IPlayable
    {
        bool IsPlayable { get; set; }
        void Control();
    }
}
