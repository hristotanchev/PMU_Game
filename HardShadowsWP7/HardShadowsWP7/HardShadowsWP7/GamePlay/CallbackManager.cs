using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardShadows.GamePlay
{
    public delegate void BasicCallback(int state);

    class CallbackManager
    {
        private static CallbackManager instance;

        public static CallbackManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CallbackManager();
                }

                return instance;
            }
        }

        public void LevelClick(int level)
        {
            TransitionManager.Instance.EndCallback = LevelManager.Instance.SwitchLevel;
            TransitionManager.Instance.EndState = level;
            TransitionManager.Instance.InitiateTransition();
        }
    }
}
