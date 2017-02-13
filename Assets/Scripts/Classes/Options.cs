using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Classes
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
