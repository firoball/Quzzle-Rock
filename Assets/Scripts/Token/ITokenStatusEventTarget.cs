using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
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
