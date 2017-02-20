using UnityEngine.EventSystems;

namespace Game.UI.Controls
{
    public interface IGaugeEventTarget : IEventSystemHandler
    {
        void OnUpdate(int value, int maxValue);
        void OnReset();
    }
}
