using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Logic.Objects
{
    public interface ITokenEventTarget : IEventSystemHandler
    {
        void OnMoveTo(Vector3 newPosition);
        void OnFakeMoveTo(Vector3 newPosition);
        void OnRemove();
        void OnRemove(float delay);
    }
}
