using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{
    public class PlayTurn : MonoBehaviour
    {
        private static PlayTurn s_singleton;

        private RuleSet m_ruleSet;

        public static void PlayerDone()
        {
            if (s_singleton != null)
            {
                s_singleton.StartCoroutine(s_singleton.ProcessPlayField());
            }
            else
            {
                Debug.Log("PlayTurn not assigned to any GameObject");
            }
        }

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                m_ruleSet = GetComponent<RuleSet>();
                if (m_ruleSet != null)
                {
                    PlayField.Unlock();
                }
                else
                {
                    //bypass all kind of null pointer checks by simply locking the PlayField
                    PlayField.Lock();
                    Debug.LogWarning("PlayTurn: No RuleSet found. Game cannot be started.");
                }
            }
            else
            {
                Debug.LogWarning("PlayTurn: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        private void End()
        {
            m_ended = true;
            Debug.Log("Game ended.");
        }

        private void New()
        {
            m_ruleSet.Restart();
            m_ended = false;
            PlayField.Restart(true);
            PlayField.Unlock();
        }

        private void Retry()
        {
            m_ruleSet.Restart();
            m_ended = false;
            PlayField.Restart(false);
            PlayField.Unlock();
        }

        private IEnumerator ProcessPlayField()
        {
            //take control from player
            PlayField.Lock();
            yield return new WaitForSeconds(0.2f);

            //Resolve all combinations unitl no more are found
            bool combo = false;
            do
            {
                List<Combination> comboList;
                if (PlayField.ResolveCombinations(out comboList))
                {
                    combo = true;
                    m_ruleSet.ProcessCombo(comboList);
                    yield return new WaitForSeconds(0.3f);

                    //Refill empty field positions
                    bool filled;
                    do
                    {
                        yield return new WaitForSeconds(0.1f);
                        filled = PlayField.Refill();
                    } while (!filled);
                    yield return new WaitForSeconds(0.2f);
                    m_ruleSet.PlayFieldModifier();
                }
                else
                {
                    combo = false;
                }
            } while (combo);

            //end current turn
            yield return new WaitForSeconds(0.3f);
            m_ruleSet.TurnEnd();

            //check if game is over
            if (m_ruleSet.GameWon())
            {
                End();
            }
            else if (m_ruleSet.GameLost())
            {
                End();
            }
            else
            {
                //test playfield for stuck state
                if (PlayField.IsStuck())
                {
                    //create new playfield
                    float clearDelay = PlayField.Clear();
                    yield return new WaitForSeconds(clearDelay + 0.2f);
                    PlayField.Populate();
                }

                //proceed with next round
                m_ruleSet.TurnStart();
                PlayField.Unlock();
            }
        }

        private void OnGUI()
        {
            if (m_ended)
            {
                GUI.Label(new Rect(10, 70, 100, 20), "Game ended.");
                if (GUI.Button(new Rect(10, 95, 100, 20), "New Game"))
                {
                    New();
                }
                if (GUI.Button(new Rect(10, 120, 100, 20), "Retry"))
                {
                    Retry();
                }
            }
        }
        private bool m_ended = false;
    }
}
