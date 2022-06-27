using System;
using UnityEngine;

namespace Source
{
    public interface IMenu
    {
        public bool BlockClosing { get; }
        public Transform Transform { get; }
        public event Action OnShow;
        public void Open();
    }
    
    public interface IResultMenu<out TResult> : IMenu
    {
        public event Action<TResult> OnResult;
    }
}