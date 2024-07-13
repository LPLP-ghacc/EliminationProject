using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool crouch;

        public Action onPauseCall;

        public bool isCursorLocked;

        public void Update()
        {
            move = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            jump = Input.GetKey(GlobalInputVariables.jumpKey);
            sprint = Input.GetKey(GlobalInputVariables.sprintKey);
            crouch = Input.GetKey(GlobalInputVariables.crouchKey);

            Pause();
        }

        private void Pause()
        {
            if (Input.GetKeyDown(GlobalInputVariables.pauseKey))
            {
                onPauseCall?.Invoke();
            }
        }

        public void SetCursorLocked(bool isLocked)
        {
            if (isLocked)
            {
                isCursorLocked = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                isCursorLocked = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
