using UnityEngine;
using System.Collections.Generic;
using Game.Logic;

namespace Game.Rules
{
    public abstract class RuleSet : MonoBehaviour
    {
        public virtual void Restart()
        {
            Debug.LogWarning("RuleSet: Restart not implemented.");
        }

        public virtual void ProcessCombo(List<Combination> combinationList)
        {
            Debug.LogWarning("RuleSet: ProcessCombo not implemented.");
        }

        public virtual void PlayerDone()
        {
            Debug.LogWarning("RuleSet: PlayerDone not implemented.");
        }

        public virtual void TurnStart()
        {
            Debug.LogWarning("RuleSet: TurnStart not implemented.");
        }

        public virtual void TurnEnd()
        {
            Debug.LogWarning("RuleSet: TurnEnd not implemented.");
        }

        public virtual bool IsGameWon()
        {
            Debug.LogWarning("RuleSet: IsGameWon not implemented.");
            return false;
        }

        public virtual bool IsGameLost()
        {
            Debug.LogWarning("RuleSet: IsGameLost not implemented.");
            return true;
        }

        public virtual void Abort()
        {
            Debug.LogWarning("RuleSet: GameEnd not implemented.");
        }

        public virtual void PlayFieldModifier()
        {
            Debug.LogWarning("RuleSet: PlayFieldModifier not implemented.");
        }

    }
}
