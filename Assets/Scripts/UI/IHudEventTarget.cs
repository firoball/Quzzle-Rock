using UnityEngine.EventSystems;

namespace Game.UI
{
    public interface IHudEventTarget : IEventSystemHandler
    {
        void OnUpdateScore(int score);
        void OnUpdateScoreMax(int scoreMax);
        void OnUpdateTurns(int turns);
        void OnUpdateTurnsMax(int turnsMax);
        void OnUpdateCombos(int combos);
        void OnRestart();
    }
}
