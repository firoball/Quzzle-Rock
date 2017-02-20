using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface ICameraAlertTarget : IEventSystemHandler
    {
        void OnAlert(float intensity);
    }
}
