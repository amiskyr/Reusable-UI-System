using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RUIS.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Screen : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public Selectable m_StartSelectable;

        [Header("Screen Events")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator animator;
        #endregion

        #region Main Methods

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        // Start is called before the first frame update
        void Start()
        {
            //animator = GetComponent<Animator>();

            if (m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }
        }
        #endregion

        #region helper methods
        public virtual void StartScreen(bool forwardNavigation)
        {
            onScreenStart?.Invoke();
            if (forwardNavigation)
            {
                HandleAnimator("show_forward");
            }
            else
            {
                HandleAnimator("show_backward");
            }
        }

        public virtual void CloseScreen(bool forwardNavigation)
        {
            onScreenClose.Invoke();
            if (forwardNavigation)
            {
                HandleAnimator("hide_forward");
            }
            else
            {
                HandleAnimator("hide_backward");
            }
        }

        /*
        public virtual void HoldScreen()
        {
            onScreenClose.Invoke();
            HandleAnimator("idle");
        }
        */

        void HandleAnimator(string aTrigger)
        {
            if (animator)
            {
                animator.SetTrigger(aTrigger);
            }
            else
            {
                Debug.Log($"ERROR (RUIS): Animator not available for the screen");
            }
        }
        #endregion
    }
}