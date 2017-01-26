using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Structs;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PlayTurn : MonoBehaviour
    {
        private static PlayTurn s_singleton;
        private bool m_gameHasEnded = false;

        [SerializeField]
        private GameObject m_endedMenu;
        [SerializeField]
        private GameObject m_levelMenu;
        [SerializeField]
        private GameObject m_wonPopup;
        [SerializeField]
        private GameObject m_lostPopup;

        private RuleSet m_ruleSet;

        public static bool GameHasEnded
        {
            get
            {
                if (s_singleton != null)
                {
                    return s_singleton.m_gameHasEnded;
                }
                else
                {
                    return false;
                }
            }
        }

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
            m_gameHasEnded = true;
            PlayField.Lock();
            StartCoroutine(EndDelay());
        }

        public static void New()
        {
            s_singleton.m_ruleSet.Restart();
            s_singleton.m_ruleSet.TurnStart();
            s_singleton.m_gameHasEnded = false;
            PlayField.Restart(true);
            PlayField.Unlock();
        }

        public static void Retry()
        {
            s_singleton.m_ruleSet.Restart();
            s_singleton.m_ruleSet.TurnStart();
            s_singleton.m_gameHasEnded = false;
            PlayField.Restart(false);
            PlayField.Unlock();
        }

        private IEnumerator EndDelay()
        {
            yield return new WaitForSeconds(3.0f);
            if (m_endedMenu != null)
            {
                ExecuteEvents.Execute<IMenuEventTarget>(m_endedMenu, null, (x, y) => x.OnShow(false));
            }
        }

        private IEnumerator ProcessPlayField()
        {
            //take control from player
            if (m_levelMenu != null)
            {
                ExecuteEvents.Execute<IMenuEventTarget>(m_levelMenu, null, (x, y) => x.OnHide(false));
            }
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
                        AudioManager.Play("refill");
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
                SpawnLabelforEnd(m_wonPopup);
            }
            else if (m_ruleSet.GameLost())
            {
                End();
                SpawnLabelforEnd(m_lostPopup);
            }
            else
            {
                //test playfield for stuck state
                if (PlayField.IsStuck())
                {
                    //create new playfield
                    float clearDelay = PlayField.Clear();
                    AudioManager.Play("stuck field");
                    yield return new WaitForSeconds(clearDelay + 0.2f);
                    PlayField.Populate();
                }

                //proceed with next round
                m_ruleSet.TurnStart();
                PlayField.Unlock();
                if (m_levelMenu != null)
                {
                    ExecuteEvents.Execute<IMenuEventTarget>(m_levelMenu, null, (x, y) => x.OnShow(false));
                }
            }
        }

        private void SpawnLabelforEnd(GameObject obj)
        {
            if (obj != null)
            {
                Instantiate(obj, Vector3.zero, Quaternion.identity);
            }
        }
    }
}
