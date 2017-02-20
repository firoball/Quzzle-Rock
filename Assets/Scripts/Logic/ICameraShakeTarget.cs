using UnityEngine.EventSystems;

namespace Game.Logic
{
    public interface ICameraShakeTarget : IEventSystemHandler
    {
        void OnShake(float intensity);
    }
}
