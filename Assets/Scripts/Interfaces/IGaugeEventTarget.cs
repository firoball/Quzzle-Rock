using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IGaugeEventTarget : IEventSystemHandler
    {
        void OnUpdate(int value, int maxValue);
    }
}
