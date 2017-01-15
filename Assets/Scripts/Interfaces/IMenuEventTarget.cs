using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IMenuEventTarget : IEventSystemHandler
    {
        void OnShow(bool immediately);
        void OnHide(bool immediately);
    }
}
