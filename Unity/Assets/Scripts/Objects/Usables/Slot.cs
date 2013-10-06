using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    [Serializable]
    public class Slot
    {
        public Transform location;
        public SlotType Type;
        public Usable Content;
        public bool Active;

        public virtual Texture2D Icon { get { return Content == null ? null : Content.Icon; } }

        public void Trigger()
        {
            Content.Trigger();
        }

        public void Initialize()
        {
            if (Content != null)
            {
                Content = (Usable)GameObject.Instantiate(Content, location.position, location.rotation);
                Content.transform.parent = location;
                Content.transform.localPosition = Vector3.zero;
                Content.Initialize();
                Active = true;
            }

            else Debug.LogWarning("Slot.Set() parameter null");
        }

        public void Set(Usable usable)
        {
            if (usable != null)
            {
                Content = (Usable)GameObject.Instantiate(Content, location.position, location.rotation);
                Content.transform.parent = location;
                Content.transform.localPosition = Vector3.zero;
                Content.Initialize();
                Active = true;
            }

            else Debug.LogWarning("Slot.Set() parameter null");
        }

        public void Remove()
        {
            if (Content != null)
                UnityEngine.Object.Destroy(Content);

            Active = false;
        }

        public Slot(Transform location, SlotType type)
        {
            this.location = location;
            this.Type = type;
        }
    }
}
