using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    public class PlayTurn : MonoBehaviour
    {
        private static PlayTurn singleton;

        private int m_turnsLeft;
        private int m_turnsMax;
        private int m_combos;

        public static void PlayerDone()
        {
            if (singleton != null)
            {
                singleton.StartCoroutine(singleton.ProcessPlayField());
            }
            else
            {
                Debug.Log("PlayTurn not assigned to any GameObject");
            }
        }

        void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
                m_turnsLeft = 30; //TEMP
                Next();
            }
            else
            {
                Debug.LogWarning("PlayTurn: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        void Update()
        {

        }

        private void Next()
        {
            PlayField.Unlock();
            m_combos = 0; //TODO: check if good here
        }

        private void End()
        {
            m_ended = true;
            m_combos = 0;
            Debug.Log("Game ended.");
        }

        private IEnumerator ProcessPlayField()
        {
            //take control from player
            PlayField.Lock();
            yield return new WaitForSeconds(0.2f);
            m_turnsLeft--;

            //Resolve all combinations unitl no more are found
            m_combos = 0;
            bool combo = false;
            do
            {
                if (PlayField.ResolveCombinations())
                {
                    //TODO: evaluate quantity and quality of Combinations, scoring
                    combo = true;
                    m_combos++;
                    yield return new WaitForSeconds(0.3f);

                    //Refill empty field positions
                    bool filled;
                    do
                    {
                        yield return new WaitForSeconds(0.1f);
                        filled = PlayField.Refill();
                    } while (!filled);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    combo = false;
                }
            } while (combo);
            Debug.Log("PlayTurn: " + m_combos + " combos done.");
            yield return new WaitForSeconds(0.3f);

            //check if game is over
            if (m_turnsLeft == 0)
            {
                End();
            }
            else
            {
                Debug.Log("PlayTurn: " + m_turnsLeft + " turns left.");

                //test playfield for stuck state
                if (PlayField.IsStuck())
                {
                    Debug.Log("PlayTurn: no more combinations possible");
                    //TODO: Repopulate
                    m_stuck = true;
                    yield return new WaitForSeconds(3.0f);
                }

                //proceed with next round
                Next();
            }
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Turns left: " + m_turnsLeft);
            if (m_combos > 0)
            {
                GUI.Label(new Rect(10, 30, 100, 20), "Combos: " + m_combos);
            }
            if (m_stuck)
            {
                GUI.Label(new Rect(10, 50, 100, 20), "PlayField stuck.");
            }
            if (m_ended)
            {
                GUI.Label(new Rect(10, 50, 100, 20), "Game ended.");
            }
        }
        private bool m_stuck = false;
        private bool m_ended = false;
    }
}
