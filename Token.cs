using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class Token : MonoBehaviour, ITokenEventTarget,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler//, IPointerClickHandler
    {
        private static GameObject s_hoveredToken = null;
        private static GameObject s_selectedToken = null;
        private static GameObject s_lastSelectedToken = null;
        private static GameObject s_draggedToken = null;
        private static GameObject s_swappedToken1 = null;
        private static GameObject s_swappedToken2 = null;
        private static bool s_locked = false;

        private Vector3 m_originalRotation;
        private Vector3 m_currentRotation;
        private Vector3 m_originalPosition;
        private Vector3 m_targetPosition;
        private float m_hoverTimer;
        private float m_moveTimer;
        private float m_moveTimerReverse;
        private float m_removeTimer;
        private float m_refTime;
        private GameObject m_marker;
        private TokenStatus m_tokenStatus;

        private const float c_animationSpeed = 5.0f;
        private const float c_slowDownSpeed = 2.5f;
        private const float c_animationAngle = 20.0f;
        private const float c_movementSpeed = 5.0f;
        private const float c_scalingSpeed = 5.0f;
        private const float c_removeSpeed = 500.0f;

        private enum TokenStatus : int
        {
            HOVER = 0,
            SWAP = 1,
            DRAG = 2,
            SELECT = 3,
            UNSELECT = 4
        }
        //static int cnt = 0;
        public static bool Locked
        {
            get
            {
                return s_locked;
            }

            set
            {
                //Debug.Log(cnt++ +" Tokens are locked? " + value);
                s_locked = value;
            }
        }

        void Awake()
        {
            m_originalRotation = transform.eulerAngles;
            m_hoverTimer = 0.0f;
            m_moveTimer = 0.0f;
            m_moveTimerReverse = 0.0f;
            m_removeTimer = 0.0f;
            m_refTime = Time.time;
            m_tokenStatus = TokenStatus.UNSELECT;
        }

        void Start()
        {
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
            //smoothly fade back to original position
            if (m_hoverTimer > 0.0f)
            {
                m_hoverTimer = Mathf.Max(0.0f, m_hoverTimer - c_slowDownSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.Slerp(m_originalRotation, m_currentRotation, m_hoverTimer);
            }

            //move to new position
            if (m_moveTimer > 0.0f)
            {
                m_moveTimer = Mathf.Max(0.0f, m_moveTimer - c_movementSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(m_targetPosition, m_originalPosition, m_moveTimer);
            }

            //move back to old position
            if ((m_moveTimer <= 0.0f) && (m_moveTimerReverse > 0.0f))
            {
                m_moveTimerReverse = Mathf.Max(0.0f, m_moveTimerReverse - c_movementSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(m_originalPosition, m_targetPosition, m_moveTimerReverse);
            }

            //token is being removed
            if (m_removeTimer > 0.0f)
            {
                m_removeTimer = Mathf.Max(0.0f, m_removeTimer - c_scalingSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, m_removeTimer);
                transform.Rotate(0.0f, 0.0f, Time.deltaTime * c_removeSpeed);
            }

            //token is being hovered
            if (s_hoveredToken == gameObject)
            {
                //TODO: move rotation to TokenHoverVisualizer OnMouseOver
                float angle = c_animationAngle * Mathf.Sin(c_animationSpeed * (Time.time - m_refTime));
                m_currentRotation = new Vector3(m_originalRotation.x, angle, m_originalRotation.z);
                transform.eulerAngles = m_currentRotation;
            }

            /* check whether token is still the selected one
             * if different token was selected, trigger token switch
             */
            if (s_lastSelectedToken == gameObject && s_selectedToken != gameObject)
            {
                TrySwap(s_selectedToken);
            }

            UpdateMarker();
            EventHandler();
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

        private void EventHandler()
        {
            if (
                ((s_swappedToken1 != null) && (s_swappedToken2 != null))
                || (m_removeTimer > 0.0f) || s_locked
                )
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
        public void OnMoveTo(Vector3 newPosition)
        {
            m_originalPosition = transform.position;
            m_targetPosition = newPosition;
            m_moveTimer = 1.0f;
            m_moveTimerReverse = 0.0f;
        }

        public void OnFakeMoveTo(Vector3 newPosition)
        {
            OnMoveTo(newPosition);
            m_moveTimerReverse = 1.0f;
        }

        public void OnRemove()
        {
            StartCoroutine(WaitRemove(0.0f));
        }

        public void OnRemove(float delay)
        {
            delay = Mathf.Max(delay, 0.0f);
            StartCoroutine(WaitRemove(delay));
        }

        //todo: may need evaluation of PointerEventData.isDragging
        public void OnPointerEnter(PointerEventData data)
        {
            s_hoveredToken = gameObject;

            //TODO: move rotation to TokenHoverVisualizer (so it does not trigger on touch)
            m_hoverTimer = 0.0f;
            m_refTime = Time.time;
            transform.eulerAngles = m_originalRotation;
        }

        //todo: may need evaluation of PointerEventData.isDragging
        public void OnPointerExit(PointerEventData data)
        {
            s_hoveredToken = null;

            //TODO: move rotation to TokenHoverVisualizer (so it does not trigger on touch)
            m_hoverTimer = 1.0f;
        }

        public void OnPointerDown(PointerEventData data)
        {
            s_draggedToken = gameObject;
        }

        string dbg = "";
        public void OnPointerUp(PointerEventData data)
        {
            dbg += "\n"+gameObject.name+" OnPointerUp\n";
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
                    dbg += "was selected\n";
                    s_lastSelectedToken = null;
                    s_selectedToken = null;
                }
                else
                {
                    dbg += "was not selected\n";
                    s_lastSelectedToken = s_selectedToken;
                    s_selectedToken = gameObject;
                }
            }
        }

        //todo use OnPointerUp and evaluate PointerEventData.pointerPress for same or diff GO.
        /*public void OnPointerClick(PointerEventData data)
        {
            s_draggedToken = null;

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
        }*/
        #endregion events

        private IEnumerator WaitSwapDone()
        {
            yield return new WaitForSeconds(0.5f);
            s_swappedToken1 = null;
            s_swappedToken2 = null;
        }

        private IEnumerator WaitRemove(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_removeTimer = 1.0f;
            Destroy(gameObject, 0.6f);
            ExecuteEvents.Execute<ITokenDestructionEventTarget>(gameObject, null, (x, y) => x.OnDestruction());
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 80, 300, 250), dbg);
        }
    }
}
