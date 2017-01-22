using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface ICameraShakeTarget : IEventSystemHandler
    {
        void OnShake(float intensity);
    }
}
