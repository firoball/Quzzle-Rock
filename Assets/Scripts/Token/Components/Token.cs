using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class Token : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        private static GameObject s_hoveredToken = null;
        private static GameObject s_selectedToken = null;
        private static GameObject s_lastSelectedToken = null;
        private static GameObject s_draggedToken = null;
        private static GameObject s_swappedToken1 = null;
        private static GameObject s_swappedToken2 = null;
        private static bool s_locked = false;

        private GameObject m_marker;
        private TokenStatus m_tokenStatus;

        private enum TokenStatus : int
        {
            HOVER = 0,
            SWAP = 1,
            DRAG = 2,
            SELECT = 3,
            UNSELECT = 4
        }

        public static bool Locked
        {
            get
            {
                return s_locked;
            }

            set
            {
                s_locked = value;
            }
        }

        void Start()
        {
            m_tokenStatus = TokenStatus.UNSELECT;
            Transform child = transform.FindChild("SelectionMarker");

            if (child != null)
            {
                m_marker = child.gameObject;
            }
            else
            {
                Debug.LogWarning("Token: child object 'SelectionMarker' not found.");
            }

            UpdateMarker();
        }

        void Update()
        {
            /* check whether token is still the selected one
             * if different token was selected, trigger token switch
             */
            if (s_lastSelectedToken == gameObject && s_selectedToken != gameObject)
            {
                TrySwap(s_selectedToken);
            }

            UpdateMarker();
            LockHandler();
        }

        private void UpdateMarker()
        {
            if (m_marker != null)
            {
                //token is being swapped
                if (s_swappedToken1 == gameObject || s_swappedToken2 == gameObject)
                {
                    TriggerVisualizer(TokenStatus.SWAP);
                }
                // token is selected
                else if (s_selectedToken == gameObject)
                {
                    TriggerVisualizer(TokenStatus.SELECT);
                }
                // token is being dragged
                else if (s_draggedToken == gameObject)
                {
                    TriggerVisualizer(TokenStatus.DRAG);
                }
                // token is hovered while another token is being dragged
                else if (s_hoveredToken == gameObject && s_draggedToken != null)
                {
                    if (PlayField.AreNeighbours(s_hoveredToken, s_draggedToken))
                    {
                        TriggerVisualizer(TokenStatus.HOVER);
                    }
                    else
                    {
                        TriggerVisualizer(TokenStatus.UNSELECT);
                    }
                }
                else
                {
                    TriggerVisualizer(TokenStatus.UNSELECT);
                }
            }

        }

        private void TriggerVisualizer(TokenStatus newStatus)
        {
            if (newStatus != m_tokenStatus)
            {
                m_tokenStatus = newStatus;
                switch (newStatus)
                {
                    case TokenStatus.HOVER:
                        {
                            ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnHover());
                            break;
                        }
                    case TokenStatus.SWAP:
                        {
                            ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnSwap());
                            break;
                        }
                    case TokenStatus.DRAG:
                        {
                            ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnDrag());
                            break;
                        }
                    case TokenStatus.SELECT:
                        {
                            ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnSelect());
                            break;
                        }
                    case TokenStatus.UNSELECT:
                        {
                            ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnUnSelect());
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

        }

        private void LockHandler()
        {
            if (s_locked || ((s_swappedToken1 != null) && (s_swappedToken2 != null)))
            {
                gameObject.layer = 2; //Ignore Raycast
            }
            else
            {
                gameObject.layer = 0; //Default
            }
        }

        private void TrySwap(GameObject token)
        {
            if (PlayField.AreNeighbours(gameObject, token))
            {
                bool success = PlayField.SwapTokens(gameObject, token);
                //reset token states
                s_selectedToken = null;
                s_lastSelectedToken = null;
                s_draggedToken = null;
                s_hoveredToken = null;

                //set tokens to be swapped
                s_swappedToken1 = gameObject;
                s_swappedToken2 = token;

                //Move tokens; if swap was not successful perform just a fake move
                if (success)
                {
                    ExecuteEvents.Execute<ITokenEventTarget>(s_swappedToken1, null,
                        (x, y) => x.OnMoveTo(s_swappedToken2.transform.position));
                    ExecuteEvents.Execute<ITokenEventTarget>(s_swappedToken2, null,
                        (x, y) => x.OnMoveTo(s_swappedToken1.transform.position));
                }
                else
                {
                    ExecuteEvents.Execute<ITokenEventTarget>(s_swappedToken1, null,
                        (x, y) => x.OnFakeMoveTo(s_swappedToken2.transform.position));
                    ExecuteEvents.Execute<ITokenEventTarget>(s_swappedToken2, null,
                        (x, y) => x.OnFakeMoveTo(s_swappedToken1.transform.position));
                }

                StartCoroutine(WaitSwapDone());
            }
        }

        #region events

        public void OnPointerEnter(PointerEventData data)
        {
            s_hoveredToken = gameObject;
        }

        public void OnPointerExit(PointerEventData data)
        {
            s_hoveredToken = null;
        }

        public void OnPointerDown(PointerEventData data)
        {
            s_draggedToken = gameObject;
        }

        public void OnPointerUp(PointerEventData data)
        {
            s_draggedToken = null;

            if (s_hoveredToken != null && s_hoveredToken != gameObject)
            {
                TrySwap(s_hoveredToken);
            }

            //pointer is released over same token 
            if (data.pointerPress == gameObject)
            {
                if (s_selectedToken == gameObject)
                {
                    s_lastSelectedToken = null;
                    s_selectedToken = null;
                }
                else
                {
                    s_lastSelectedToken = s_selectedToken;
                    s_selectedToken = gameObject;
                }
            }
        }

        #endregion events

        private IEnumerator WaitSwapDone()
        {
            yield return new WaitForSeconds(0.5f);
            s_swappedToken1 = null;
            s_swappedToken2 = null;
        }

    }
}
