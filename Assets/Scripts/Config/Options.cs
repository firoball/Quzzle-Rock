using UnityEngine;
using System.Collections;

namespace Game.Config
{
    public class Options
    {
        private static bool s_showResults = true;

        public static bool ShowResults
        {
            get
            {
                return s_showResults;
            }

            set
            {
                s_showResults = value;
            }
        }
    }
}
