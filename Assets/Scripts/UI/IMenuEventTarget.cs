using UnityEngine.EventSystems;

namespace Game.UI
{
    public interface IMenuEventTarget : IEventSystemHandler
    {
        void OnShow(bool immediately);
        void OnHide(bool immediately);
    }
}
