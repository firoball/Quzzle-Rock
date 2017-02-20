using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IResultSetTarget : IEventSystemHandler
    {
        void OnSetResult(string key, string value);
    }
}
