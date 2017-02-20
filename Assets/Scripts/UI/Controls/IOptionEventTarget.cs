using UnityEngine.EventSystems;

namespace Game.UI.Controls
{
    public interface IOptionEventTarget : IEventSystemHandler
    {
        void OnConfirm();
        void OnAbort();
    }
}