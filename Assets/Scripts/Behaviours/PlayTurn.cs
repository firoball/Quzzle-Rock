using UnityEngine;
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

        [SerializeField]
        private GameObject m_endedMenu;
        [SerializeField]
        private GameObject m_wonMenu;
        [SerializeField]
        private GameObject m_levelMenu;
        [SerializeField]
        private GameObject m_wonPopup;
        [SerializeField]
        private GameObject m_lostPopup;

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

        public static void New()
        {
            s_singleton.m_ruleSet.Restart();
            s_singleton.m_ruleSet.TurnStart();
            PlayField.Restart(true);
            PlayField.Unlock();
        }

        public static void Retry()
        {
            s_singleton.m_ruleSet.Restart();
            s_singleton.m_ruleSet.TurnStart();
            PlayField.Restart(false);
            PlayField.Unlock();
        }

        public static void Abort()
        {
            s_singleton.m_ruleSet.Abort();
        }

        void Start()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                m_ruleSet = GetComponent<RuleSet>();
                if (m_ruleSet != null)
                {
                    //PlayField.Unlock();
                    New();
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
                Debug.Log("PlayTurn: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        private void Finished(bool won)
        {
            PlayField.Lock();
            StartCoroutine(FinishedDelay(won));
        }

        private void SpawnLabelforEnd(GameObject obj)
        {
            if (obj != null)
            {
                Instantiate(obj, Vector3.zero, Quaternion.identity);
            }
        }

        private IEnumerator FinishedDelay(bool won)
        {
            GameObject menu;
            if (won)
            {
                SpawnLabelforEnd(m_wonPopup);
                menu = m_wonMenu;
            }
            else
            {
                SpawnLabelforEnd(m_lostPopup);
                menu = m_endedMenu;
            }
            yield return new WaitForSeconds(3.0f);
            /* screen dimming was reset on intention in ProcessPlayField()
             * make sure this is undone when game was finished
             */
            ExecuteEvents.Execute<IDimmerEventTarget>(m_levelMenu, null, (x, y) => x.OnDimmer(true));
            ExecuteEvents.Execute<IMenuEventTarget>(menu, null, (x, y) => x.OnShow(false));
        }

        private IEnumerator ProcessPlayField()
        {
            //take control from player
            ExecuteEvents.Execute<IMenuEventTarget>(m_levelMenu, null, (x, y) => x.OnHide(false));
            PlayField.Lock();
            /* screen dimming is controlled by level menu. When menu is hidden, dimming will be
             * active. This is not wanted during combo and refill phase, so override setting here
             */
            ExecuteEvents.Execute<IDimmerEventTarget>(m_levelMenu, null, (x, y) => x.OnDimmer(false));
            yield return new WaitForSeconds(0.2f);
            m_ruleSet.PlayerDone();

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
            if (m_ruleSet.IsGameWon())
            {
                Finished(true);
                AudioManager.Play("won");
            }
            else if (m_ruleSet.IsGameLost())
            {
                Finished(false);
                AudioManager.Play("lost");
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
                ExecuteEvents.Execute<IMenuEventTarget>(m_levelMenu, null, (x, y) => x.OnShow(false));
            }
        }

    }
}
