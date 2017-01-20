using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces
{
    public interface ITokenDestructionEventTarget : IEventSystemHandler
    {
        void OnDestruction();
    }
}
