using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RUIS.UI
{
    public class Timed_UI_Screen : UI_Screen
    {
        #region Variables
        [Header("Timed Screen Properties")]
        public float m_ScreenTime = 2f;
        public UnityEvent onTimeCompleted = new UnityEvent();

        private float startTime;

        #endregion

        #region Main Methods

        #endregion

        #region Helper Methods
        public override void StartScreen(bool forwardNavigation)
        {
            base.StartScreen(true);

            startTime = Time.time;
            StartCoroutine(WaitForTime());
        }

        IEnumerator WaitForTime()
        {
            yield return new WaitForSeconds(m_ScreenTime);

            if (onTimeCompleted != null)
            {
                onTimeCompleted.Invoke();
            }
        }
        #endregion
    }
}