﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RUIS.UI
{
    public class UI_System : MonoBehaviour
    {
        #region variables
        [Header("Main Properties")]
        [SerializeField] private UI_Screen m_StartScreen;

        [Header("System Events")]
        [SerializeField] private UnityEvent onSwitchedScreen = new UnityEvent();

        [Header("Fader properties")]
        [SerializeField] private Image m_Fader;
        [SerializeField] private float m_FadeInDuration = 1f;
        [SerializeField] private float m_FadeOutDuration = 1f;

        [SerializeField] private int registerClickRefresh = 1;

        [SerializeField] private Component[] screens = new Component[0];

        private Stack<UI_Screen> previousScreens;
        private bool isBackwardNavigating = false;
        private UI_Screen previousScreen;
        // public UI_Screen PreviousScreen { get { return previousScreen; } }

        private UI_Screen currentScreen;
        [SerializeField] private UI_Screen CurrentScreen { get { return currentScreen; } }
        #endregion

        #region main methods

        void Awake()
        {
            screens = GetComponentsInChildren<UI_Screen>(true);
            previousScreens = new Stack<UI_Screen>();
        }

        // Start is called before the first frame update
        void Start()
        {

            InitializeScreens();

            if (m_StartScreen)
            {
                SwitchScreens(m_StartScreen);
            }

            if (m_Fader)
            {
                m_Fader.gameObject.SetActive(true);
            }
            FadeIn();
        }
        #endregion

        #region helper methods
        public void SwitchScreens(UI_Screen aScreen)
        {
            bool backwardTransition = isBackwardNavigating;
            //StartCoroutine(WaitToLoadScene(registerClickRefresh));
            if (aScreen)
            {
                if (currentScreen)
                {
                    currentScreen.CloseScreen(!backwardTransition);
                    if (isBackwardNavigating)
                    {
                        isBackwardNavigating = false;
                    }
                    else
                    {
                        // previousScreen = currentScreen;
                        if (previousScreens.Contains(currentScreen))
                        {
                            Debug.Log("RUIS: Repeating Screen");
                        }
                        else
                        {
                            Debug.Log("RUIS: No Repeating Screen");
                            previousScreens.Push(currentScreen);
                            Debug.Log($"Stack size: {previousScreens.Count} after pushing");
                        }
                    }
                }
                else
                {
                    Debug.Log("No Current Screen available");
                }
            }

            currentScreen = aScreen;
            currentScreen.gameObject.SetActive(true);
            currentScreen.StartScreen(!backwardTransition);

            onSwitchedScreen?.Invoke();
        }

        public void FadeIn()
        {
            if (m_Fader)
            {
                m_Fader.CrossFadeAlpha(0f, m_FadeInDuration, false);
            }
        }

        public void FadeOut()
        {
            if (m_Fader)
            {
                m_Fader.CrossFadeAlpha(1f, m_FadeOutDuration, false);
            }
        }

        public void GoToPreviousScreen()
        {
            if (previousScreens.Count > 0)
            {
                UI_Screen temp = previousScreens.Peek();
                previousScreens.Pop();
                Debug.Log($"Stack size: {previousScreens.Count} after popping");

                isBackwardNavigating = true;
                SwitchScreens(temp);
            }
        }

        public void LoadScene(int SceneTransitionWait)
        {
            StartCoroutine(WaitToLoadScene(SceneTransitionWait));
        }

        IEnumerator WaitToLoadScene(int WaitTime)
        {
            yield return new WaitForSeconds(WaitTime);
        }

        void InitializeScreens()
        {
            foreach (var screen in screens)
            {
                screen.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}