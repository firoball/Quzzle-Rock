using UnityEngine.EventSystems;

namespace Game.UI
{
    public interface IStatisticsEventTarget : IEventSystemHandler
    {
        void OnShow();
    }
}