using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface IDimmerEventTarget : IEventSystemHandler
    {
        void OnDimmer(bool enableDim);
    }
}
