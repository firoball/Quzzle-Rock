using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IResultEventTarget : IEventSystemHandler
    {
        void OnAddResults(Dictionary<string, string> results);
        void OnReset();
    }
}
