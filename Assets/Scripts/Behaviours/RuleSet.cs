using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{
    public abstract class RuleSet : MonoBehaviour
    {
        public virtual void Restart()
        {
            Debug.LogWarning("RuleSet: Restart not implemented.");
        }

        public virtual void ProcessCombo(List<Combination> combinationList)
        {
            Debug.LogWarning("RuleSet: ProcessComboStart not implemented.");
        }

        public virtual void TurnStart()
        {
            Debug.LogWarning("RuleSet: Next not implemented.");
        }

        public virtual void TurnEnd()
        {
            Debug.LogWarning("RuleSet: PlayFieldDone not implemented.");
        }

        public virtual bool GameWon()
        {
            Debug.LogWarning("RuleSet: GameWon not implemented.");
            return false;
        }

        public virtual bool GameLost()
        {
            Debug.LogWarning("RuleSet: GameLost not implemented.");
            return true;
        }

        public virtual void PlayFieldModifier()
        {
            Debug.LogWarning("RuleSet: PlayFieldModifier not implemented.");
        }

    }
}
