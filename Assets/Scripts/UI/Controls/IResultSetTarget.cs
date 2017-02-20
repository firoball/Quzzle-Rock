using UnityEngine.EventSystems;

namespace Game.UI.Controls
{
    public interface IResultSetTarget : IEventSystemHandler
    {
        void OnSetResult(string key, string value);
    }
}
