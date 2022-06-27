using System;
using CW.Common;
using Lean.Gui;
using Lean.Touch;
using UnityEngine;

namespace Source
{
    public class MouseInputService
    {
        public LeanFinger Hover { get; private set; }
        public LeanFinger Press { get; private set; }
        public Vector2 Position { get; private set; }
        public float Wheel { get; private set; }
        public bool IsOverUI { get; private set; }
        public bool LeftPressed { get; private set; }
        public bool RightPressed { get; private set; }
        public bool MiddlePressed { get; private set; }
        public int PressedButton { get; private set; }

        public bool Down { get; private set; }
        public bool Up { get; private set; }

        public MouseInputService()
        {
            PressedButton = -1;
            LeanTouch.OnFingerUpdate += InputHandler;
        }

        private void InputHandler(LeanFinger finger)
        {
            if (Down)
                Down = false;
            
            if (Up)
            {
                Up = false;
                Press = null;
                PressedButton = -1;
            }
            
            if (finger.Index == LeanTouch.HOVER_FINGER_INDEX)
            {
                Hover = finger;

                Position = finger.ScreenPosition;
                IsOverUI = finger.IsOverGui;
                Wheel = Input.mouseScrollDelta.y;

                LeftPressed = Input.GetMouseButton(0);
                RightPressed = Input.GetMouseButton(1);
                MiddlePressed = Input.GetMouseButton(2);
            }
            else if (finger.Index == LeanTouch.MOUSE_FINGER_INDEX)
            {
                if (PressedButton < 0)
                {
                    for (var i = 0; i < 3; ++i)
                        if (Input.GetMouseButtonDown(i))
                        {
                            PressedButton = i;
                            Down = true;
                            Press = finger;
                        }
                }
                else
                {
                    if (Input.GetMouseButtonUp(PressedButton))
                        Up = true;
                    Press = finger;
                }
            }
        }

        ~MouseInputService()
        {
            LeanTouch.OnFingerUpdate -= InputHandler;
        }
    }
}