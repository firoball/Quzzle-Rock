using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class Token : MonoBehaviour, ITokenMoveEventTarget
    {
        private static GameObject s_hoveredToken = null;
        private static GameObject s_selectedToken = null;
        private static GameObject s_lastSelectedToken = null;
        private static GameObject s_draggedToken = null;
        private static GameObject s_swappedToken1 = null;
        private static GameObject s_swappedToken2 = null;

        private Vector3 m_originalRotation;
        private Vector3 m_currentRotation;
        private Vector3 m_originalPosition;
        private Vector3 m_targetPosition;
        private float m_hoverTimer;
        private float m_moveTimer;
        private float m_refTime;
        private GameObject m_marker;

        private const float c_animationSpeed = 5.0f;
        private const float c_slowDownSpeed = 2.5f;
        private const float c_animationAngle = 10.0f;
        private const float c_movementSpeed = 5.0f;

        void Start()
        {
            m_originalRotation = transform.eulerAngles;
            m_hoverTimer = 0.0f;
            m_moveTimer = 0.0f;
            m_refTime = Time.time;

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
            // smoothly fade back to original position
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

            /* check whether token is still the selected one
             * if different token was selected, trigger token switch
             */
            if (s_lastSelectedToken == gameObject && s_selectedToken != gameObject)
            {
                TrySwap(s_selectedToken);
            }

            UpdateMarker();
            MouseEventHandler();
        }

        private void UpdateMarker()
        {
            if (m_marker != null)
            {
                //token is being swapped
                if (s_swappedToken1 == gameObject || s_swappedToken2 == gameObject)
                {
                    ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnSwap());
                }
                // token is selected
                else if (s_selectedToken == gameObject)
                {
                    ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnSelect());
                }
                // token is being dragged
                else if (s_draggedToken == gameObject)
                {
                    ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnDrag());
                }
                // token is hovered while another token is being dragged
                else if (s_hoveredToken == gameObject && s_draggedToken != null)
                {
                    if (PlayField.areNeighbours(s_hoveredToken, s_draggedToken))
                    {
                        ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnHover());
                    }
                    else
                    {
                        ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnUnSelect());
                    }
                }
                else
                {
                    ExecuteEvents.Execute<ITokenStatusEventTarget>(m_marker, null, (x, y) => x.OnUnSelect());
                }
            }
        }

        private void MouseEventHandler()
        {
            if (s_swappedToken1 != null && s_swappedToken2 != null)
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
            //if (token != null)
            //{
            //TODO: 1) neighbour check - do nothing if failed
            //TODO: 2) Swap check 
            //         failed: move back and forth tokens to indictate forbidden move
            //         passed: move to switched positions
                if (PlayField.SwapTokens(gameObject, token))
                {
                    //swap was succesful, now reset token states
                    s_selectedToken = null;
                    s_lastSelectedToken = null;
                    s_draggedToken = null;
                    s_hoveredToken = null;

                    //set tokens to be swapped
                    s_swappedToken1 = gameObject;
                    s_swappedToken2 = token;

                    //Move tokens
                    ExecuteEvents.Execute<ITokenMoveEventTarget>(s_swappedToken1, null,
                        (x, y) => x.OnMoveTo(s_swappedToken2.transform.position));
                    ExecuteEvents.Execute<ITokenMoveEventTarget>(s_swappedToken2, null,
                        (x, y) => x.OnMoveTo(s_swappedToken1.transform.position));

                    //TODO: do this in a nicer way
                    StartCoroutine(WaitSwapDone());
                }
            //}
        }

        public void OnMoveTo(Vector3 newPosition)
        {
            m_originalPosition = transform.position;
            m_targetPosition = newPosition;
            m_moveTimer = 1.0f;
        }

        void OnMouseEnter()
        {
            s_hoveredToken = gameObject;

            m_hoverTimer = 0.0f;
            m_refTime = Time.time;
            transform.eulerAngles = m_originalRotation;
        }

        void OnMouseExit()
        {
            s_hoveredToken = null;

            m_hoverTimer = 1.0f;
        }

        void OnMouseOver()
        {
            float angle = c_animationAngle * Mathf.Sin(c_animationSpeed * (Time.time - m_refTime));
            m_currentRotation = new Vector3(m_originalRotation.x, angle, m_originalRotation.z);
            transform.eulerAngles = m_currentRotation;
        }

        void OnMouseDown()
        {
            s_draggedToken = gameObject;
        }

        void OnMouseUp()
        {
            s_draggedToken = null;

            if (s_hoveredToken != null && s_hoveredToken != gameObject)
            {
                TrySwap(s_hoveredToken);
            }
        }

        void OnMouseUpAsButton()
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
        }

        //TODO: this somewhat sucks
        private IEnumerator WaitSwapDone()
        {
            yield return new WaitForSeconds(0.5f);
            s_swappedToken1 = null;
            s_swappedToken2 = null;
            Debug.Log("swap done");
        }
    }
}
