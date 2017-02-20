using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Game.Logic.Objects
{
    public interface ITokenStatusEventTarget : IEventSystemHandler
    {
        void OnSelect();
        void OnUnSelect();
        void OnHover();
        void OnDrag();
        void OnSwap();
    }
}
