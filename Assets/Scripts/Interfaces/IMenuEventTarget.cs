using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IMenuEventTarget : IEventSystemHandler
    {
        void Show(bool immediately);
        void Hide(bool immediately);
    }
}
