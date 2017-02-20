using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Game.UI
{
    public interface IResultEventTarget : IEventSystemHandler
    {
        void OnAddResults(Dictionary<string, string> results);
        void OnReset();
    }
}
