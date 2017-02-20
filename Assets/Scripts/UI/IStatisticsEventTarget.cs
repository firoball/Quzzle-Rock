using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IStatisticsEventTarget : IEventSystemHandler
    {
        void OnShow();
    }
}