using UnityEngine.EventSystems;

namespace Game.Ambient
{
    public interface ICameraAlertTarget : IEventSystemHandler
    {
        void OnAlert(float intensity);
    }
}
