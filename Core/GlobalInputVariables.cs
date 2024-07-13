using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class GlobalInputVariables
    {
        // this class created for read input and save input keys to localfile

        public static KeyCode jumpKey = KeyCode.Space;
        public static KeyCode sprintKey = KeyCode.LeftShift;
        public static KeyCode crouchKey = KeyCode.LeftControl;
        public static KeyCode pauseKey = KeyCode.Escape;
        public static KeyCode freeLook = KeyCode.V;
        public static KeyCode lookLeftKey = KeyCode.Q;
        public static KeyCode lookRightKey = KeyCode.E;
        public static KeyCode interactKey = KeyCode.F;
        public static KeyCode viewMode = KeyCode.Tab;
    }
}
