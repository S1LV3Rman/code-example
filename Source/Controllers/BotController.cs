using UnityEngine;

namespace Source
{
    public class BotController : IController
    {
        private readonly ConsoleInterface _console;
        private readonly OmegaBotInterface _omegaBot;
        private readonly CircuitBoard _board;
        private readonly CursorsConfig _cursors;

        private bool _resizing;
        private bool _resizerHovered;
        private float _resizeStartHeight;
        private Vector3 _resizeStartPos;

        public BotController(ConsoleInterface console, OmegaBotInterface omegaBot, CircuitBoard board, CursorsConfig cursors)
        {
            _console = console;
            _omegaBot = omegaBot;
            _board = board;
            _cursors = cursors;
        }
        
        public void Init()
        {
            _console.OnResizeBegin += BeginConsoleResizing;
            _console.OnResizeEnd += EndResizing;
            _console.OnResizerEnter += ResizerEnter;
            _console.OnResizerExit += ResizerExit;
            _console.Height = 33f;

            _board.OnConsoleMessage += _console.AddMessage;
            _board.OnSetMotorPower += _omegaBot.SetMotorsPower;
            _board.OnSetPortValue += _omegaBot.SetModuleValue;

            _omegaBot.OnChangeSensorValue += _board.ChangePortValue;
            _omegaBot.OnRangefinderValueChange += _board.SetRange;
        }

        public void Run()
        {
            if (_resizing)
            {
                var mousePos = Input.mousePosition;
                var delta = mousePos.y - _resizeStartPos.y;

                //todo: Move constants to config
                _console.Height = Mathf.Clamp(_resizeStartHeight + delta,
                    33f, _console.ParentHeight * 0.25f);
            }
        }

        private void BeginConsoleResizing()
        {
            _resizeStartHeight = _console.Height;
            BeginResizing();
        }

        private void BeginResizing()
        {
            _resizeStartPos = Input.mousePosition;
            _resizing = true;
        }

        private void EndResizing()
        {
            _resizing = false;
            if(!_resizerHovered)
                SetDefaultCursor();
        }

        private void ResizerEnter()
        {
            _resizerHovered = true;
            SetResizeCursor();
        }

        private void ResizerExit()
        {
            _resizerHovered = false;
            if(!_resizing)
                SetDefaultCursor();
        }

        private void SetResizeCursor()
        {
            Cursor.SetCursor(_cursors.VResize, new Vector2(16f, 16f), CursorMode.Auto);
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}