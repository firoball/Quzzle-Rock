using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{
    public class StatisticsManager
    {
        public GameStatistics game;
        public GlobalStatistics global;

        private GameObject m_resultUI;

        public StatisticsManager(GameObject resultUI)
        {
            m_resultUI = resultUI;
            global.Load();
        }

        public void Init()
        {
            //only game statistics are initialized on start of new game round!
            game.Turns = 0;
            game.ExtraTurns = 0;
            game.Combos = 0;
            game.BiggestCombo = 0;
            game.PlayTime = Time.time;

            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnReset());
        }

        public void Report()
        {
            ReportGame();
            ReportGlobal();
        }

        public void Store()
        {
            //update global statistics
            global.Save();
            PlayerPrefs.Save();
        }

        private void ReportGame()
        {
            //these strings should not be hardcoded, but let's get this done
            Dictionary<string, string> results = new Dictionary<string, string>();
            string key;
            string value;

            key = "Play Time";
            game.PlayTime = Time.time - game.PlayTime;
            TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(game.PlayTime));
            value = string.Format("{0}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            results.Add(key, value);

            key = "Total Turns";
            value = game.Turns.ToString();
            results.Add(key, value);

            key = "Extra Turns";
            value = game.ExtraTurns.ToString();
            results.Add(key, value);

            key = "Combo Count";
            value = game.Combos.ToString();
            results.Add(key, value);

            key = "Longest Combo";
            value = game.BiggestCombo.ToString();
            results.Add(key, value);

            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnAddResults(results));
        }

        private void ReportGlobal()
        {
            //these strings should not be hardcoded, but let's get this done
            Dictionary<string, string> results = new Dictionary<string, string>();
            string key;
            string value;

            key = "Total Time";
            global.PlayTime += game.PlayTime;
            TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(global.PlayTime));
            value = string.Format("{0}d {1}:{2:D2}", time.Days, time.Hours, time.Minutes);
            results.Add(key, value);

            key = "Games Played";
            value = global.GamesPlayed.ToString();
            results.Add(key, value);

            key = "Abort Ratio";
            float percent = 100.0f * (Convert.ToSingle(global.GamesAborted) / Convert.ToSingle(global.GamesPlayed));
            value = percent.ToString("F1") + "%";
            results.Add(key, value);

            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnAddResults(results));
        }

    }
}
