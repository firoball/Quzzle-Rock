using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IOptionEventTarget : IEventSystemHandler
    {
        void OnConfirm();
        void OnAbort();
    }
}