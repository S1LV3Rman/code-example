using System;
using CW.Common;
using UnityEngine;

namespace Source
{
    public class PlayAreaController : IController
    {
        private readonly CameraInterface camera;
        private readonly OmegaBotInterface omegaBot;
        private readonly MouseInputService mouse;
        private readonly AmplitudeService _amplitude;
        private readonly PlayAreaInterface playArea;
        private readonly ExecutionManagerInterface executionManager;
        
        private readonly int defaultLayer;
        private readonly int botLayer;

        private bool _modesHovered;
        private float _modesTimeToCollapse;

        private bool _followMode;
        private bool _focusing;
        private bool _changingAngle;
        
        private Vector3 _remainingBotPosition;
        private Vector3 _targetBotPosition;
        
        private Vector3 _remainingCameraPosition;
        private Vector3 _targetCameraPosition;
        
        private float _remainingCameraAngle;
        private float _targetCameraAngle;
        
        private float _remainingBotAngle;

        private bool _botHit;
        private bool _botHeld;

        private bool _botRotated;

        public PlayAreaController(
            PlayAreaInterface playArea, ExecutionManagerInterface executionManager,
            CameraInterface camera, OmegaBotInterface omegaBot,
            MouseInputService mouse, AmplitudeService amplitude)
        {
            this.mouse = mouse;
            _amplitude = amplitude;
            this.camera = camera;
            this.omegaBot = omegaBot;
            this.playArea = playArea;
            this.executionManager = executionManager;

            defaultLayer = LayerMask.GetMask("Default");
            botLayer = LayerMask.GetMask("OmegaBot");
        }
        
        public void Init()
        {
            playArea.OnZoomIn += ZoomIn;
            playArea.OnZoomOut += ZoomOut;
            playArea.OnFocus += Focus;

            playArea.OnFreeMode += FreeMode;
            playArea.OnTopDownMode += TopDownMode;
            playArea.OnFollowMode += FollowMode;
            
            playArea.OnModesClick += ModesClick;
            playArea.OnModesEnter += ModesEnter;
            playArea.OnModesExit += ModesExit;
            
            //_mouseInput.OnWheel += Zoom;
            _targetCameraPosition = camera.Position;
            _targetCameraAngle = camera.LocalRotation.x;
        }

        public void Run()
        {
            MoveCamera();
            RotateCamera();
            ProcessBotDrag();
            ProcessCameraModesMenu();
        }

        private void ProcessBotDrag()
        {
            if (mouse.PressedButton == 0)
            {
                if (_botHit)
                {
                    if (mouse.Up)
                    {
                        ReleaseBot();
                    }
                    else if (!_botHeld && 
                             (mouse.Press.Old || !Physics.Raycast(mouse.Hover.GetRay(camera.Camera), out _, Mathf.Infinity, botLayer)))
                    {
                        HoldBot();
                    }

                    if (_botHeld)
                    {
                        if (Physics.Raycast(mouse.Hover.GetRay(camera.Camera), out var mouseHit, Mathf.Infinity, defaultLayer))
                        {
                            var topDownOrigin = mouseHit.point;
                            topDownOrigin.y += 10000f;
                            var topDownRay = new Ray(topDownOrigin, Vector3.down);
                            if (Physics.SphereCast(topDownRay, 0.1f, out var hit, Mathf.Infinity, defaultLayer))
                            {
                                topDownOrigin.y = hit.point.y + 0.1f;
                                _targetBotPosition = topDownOrigin;
                                SmoothBotDrag();
                                SmoothBotRotate();
                            }
                        }
                    }
                }
                else if (!mouse.IsOverUI)
                {
                    if (mouse.Down &&
                        Physics.Raycast(mouse.Hover.GetRay(camera.Camera), out _, Mathf.Infinity, botLayer))
                    {
                        _botHit = true;
                    }
                }
            }
            else
            {
                ReleaseBot();
            }
        }

        private void HoldBot()
        {
            _amplitude.SendEvent("bot-drag-start",
                new Property("in-follow", _followMode));
            
            _botHeld = true;
            _botRotated = false;
            
            playArea.OffFollow();
            
            omegaBot.DisablePhysics();
            omegaBot.ResetVerticalRotation();
            omegaBot.ResetVelocity();
            
            camera.LockZoom();
        }

        private void ReleaseBot()
        {
            if (!_botHeld) return;
            
            _amplitude.SendEvent("bot-drag-end");
            
            _botHeld = false;
            _botHit = false;
            
            omegaBot.EnablePhysics();
            
            camera.UnlockZoom();
        }

        private void SmoothBotDrag()
        {
            var oldPosition = omegaBot.Position;
            omegaBot.Position = _targetBotPosition;
            _remainingBotPosition = omegaBot.Position - oldPosition;

            var factor = CwHelper.DampenFactor(playArea.BotMoveDamping, Time.deltaTime);
            var newRemainingPosition = Vector3.Lerp(_remainingBotPosition, Vector3.zero, factor);
            omegaBot.Position = oldPosition + _remainingBotPosition - newRemainingPosition;

            _remainingBotPosition = newRemainingPosition;
        }
        
        private void SmoothBotRotate()
        {
            var wheelDelta = mouse.Wheel;

            if (!_botRotated && wheelDelta != 0)
                _amplitude.SendEvent("bot-rotated");
            
            var rotation = omegaBot.Rotation;
            _remainingBotAngle -= wheelDelta * playArea.BotRotateSensitivity * Mathf.Clamp(Mathf.Abs(_remainingBotAngle), 1f, 5f);

            var targetAngle = omegaBot.Rotation.eulerAngles.y + _remainingBotAngle;
            var excessAngle = targetAngle - Mathf.Round(targetAngle / 5f) * 5f;
            _remainingBotAngle -= excessAngle;

            var factor = CwHelper.DampenFactor(playArea.BotRotateDamping, Time.deltaTime);
            var newRemainingDelta = Mathf.Lerp(_remainingBotAngle, 0, factor);
            omegaBot.Rotation = Quaternion.Euler(
                rotation.eulerAngles.x,
                rotation.eulerAngles.y + _remainingBotAngle - newRemainingDelta,
                rotation.eulerAngles.z);

            _remainingBotAngle = newRemainingDelta;
        }

        private void MoveCamera()
        {
            if (_followMode)
            {
                _targetCameraPosition = omegaBot.Position;
                SmoothMove();
            }
            else if (_focusing)
            {
                SmoothMove();
                
                if (mouse.MiddlePressed || (camera.Position - _targetCameraPosition).magnitude < 0.0001f)
                    _focusing = false;
            }
        }

        private void SmoothMove()
        {
            var oldPosition = camera.Position;
            camera.Position = _targetCameraPosition;
            _remainingCameraPosition = camera.Position - oldPosition;

            var damping = playArea.CameraMoveDamping / Mathf.Abs(camera.CurrentZoom / playArea.CameraMoveSensitivity);
            var factor = CwHelper.DampenFactor(damping, Time.deltaTime);
            var newRemainingPosition = Vector3.Lerp(_remainingCameraPosition, Vector3.zero, factor);
            camera.Position = oldPosition + _remainingCameraPosition - newRemainingPosition;

            _remainingCameraPosition = newRemainingPosition;
        }

        private void RotateCamera()
        {
            if (_changingAngle)
            {
                SmoothCameraRotate();

                var angle = camera.LocalRotation.x;
                if (Mathf.Abs(angle - _targetCameraAngle) < 0.0001f)
                    _changingAngle = false;
            }
        }

        private void SmoothCameraRotate()
        {
            var oldRotation = camera.LocalRotation;
            
            var rotation = camera.LocalRotation;
            rotation.x = _targetCameraAngle;
            camera.LocalRotation = rotation;
            
            _remainingCameraAngle = camera.LocalRotation.x - oldRotation.x;

            var factor = CwHelper.DampenFactor(playArea.CameraRotateDamping, Time.deltaTime);
            var newRemainingAngle = Mathf.Lerp(_remainingCameraAngle, 0, factor);
            oldRotation.x += _remainingCameraAngle - newRemainingAngle;
            camera.LocalRotation = oldRotation;

            _remainingCameraAngle = newRemainingAngle;
        }

        private void ProcessCameraModesMenu()
        {
            if (_modesTimeToCollapse <= 0) 
                return;
            
            if (!_modesHovered)
                _modesTimeToCollapse -= Time.deltaTime;
            
            if (_modesTimeToCollapse <= 0)
                playArea.CollapseModes();
        }

        private void ModesEnter()
        {
            _modesHovered = true;
            //todo: Move constant to config
            _modesTimeToCollapse = 1f;
        }

        private void ModesExit()
        {
            _modesHovered = false;
        }

        private void ModesClick()
        {
            playArea.ExpandModes();
        }

        private void ZoomIn()
        {
            _amplitude.SendEvent("zoom-in");
            camera.Zoom(5f);
        }

        private void ZoomOut()
        {
            _amplitude.SendEvent("zoom-out");
            camera.Zoom(-5f);
        }

        private void Focus()
        {
            _amplitude.SendEvent("focus", 
                new Property("valid", !_followMode));
            
            if (!_followMode)
            {
                _focusing = true;
                
                var position = omegaBot.Position;
                position.y = 0f;
                _targetCameraPosition = position;
            }
        }

        private void FreeMode()
        {
            _amplitude.SendEvent("camera-3/4");
            ResetCameraPosition();
            ChangeAngle(30f);
        }

        private void TopDownMode()
        {
            _amplitude.SendEvent("camera-topdown");
            ResetCameraPosition();
            ChangeAngle(90f);
        }

        private void FollowMode()
        {
            _followMode = !_followMode;
            if (_botHeld) return;
            _amplitude.SendEvent("follow", 
                new Property("on", _followMode));
        }

        private void ChangeAngle(float angle)
        {
            _changingAngle = true;
            _targetCameraAngle = angle;
        }

        private void ResetCameraPosition()
        {
            var position = camera.Position;
            position.y = 0f;
            _targetCameraPosition = position;
        }
    }
}