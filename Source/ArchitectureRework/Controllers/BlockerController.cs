using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class BlockerController
    {
        private readonly MenusView _view;

        private Stack<Transform> _activeWindows;

        public BlockerController(MenusView view)
        {
            view.BlockerButton.OnClick.AddListener(view.WindowCloser.CloseTopMost);

            _activeWindows = new Stack<Transform>();
            _view = view;
        }

        public void BlockClosing()
        {
            _view.WindowCloser.enabled = false;
        }

        public void ShowUnderMenu(IMenu menu)
        {
            _view.Blocker.SetAsLastSibling();
            menu.Transform.SetAsLastSibling();
            
            ShowBlocker();
            
            _activeWindows.Push(menu.Transform);
        }

        public void ToPreviousMenu()
        {
            _activeWindows.Pop();
            
            if (_activeWindows.Count == 0)
            {
                HideBlocker();
            }
            else
            {
                var previousMenu = _activeWindows.Peek();
                    
                _view.Blocker.SetAsLastSibling();
                previousMenu.transform.SetAsLastSibling();
            }
        }

        public void HideAll()
        {
            _view.WindowCloser.CloseAll();
        }

        private void ShowBlocker()
        {
            _view.BlockerCanvas.alpha = 1f;
            _view.BlockerCanvas.blocksRaycasts = true;
        }

        private void HideBlocker()
        {
            _view.BlockerCanvas.alpha = 0f;
            _view.BlockerCanvas.blocksRaycasts = false;
        }
        
        // For future
        // private Task<TResult> DialogMenuAsync<TResult>(IResultMenu<TResult> dialogMenu)
        // {
        //     var tcs = new TaskCompletionSource<TResult>();
        //     dialogMenu.OnResult += result =>
        //     {
        //         tcs.SetResult(result);
        //         LiftPreviousWindow();
        //     };
        //     
        //     OpenMenu(dialogMenu);
        //     
        //     return tcs.Task;
        // }
    }
}