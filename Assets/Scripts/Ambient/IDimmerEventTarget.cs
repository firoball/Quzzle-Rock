using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Ambient
{
    public interface IDimmerEventTarget : IEventSystemHandler
    {
        void OnDimmer(bool enableDim);
    }
}
