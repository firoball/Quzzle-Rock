using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface ITokenEventTarget : IEventSystemHandler
    {
        void OnMoveTo(Vector3 newPosition);
        void OnFakeMoveTo(Vector3 newPosition);
        void OnRemove();
        void OnRemove(float delay);
    }
}
