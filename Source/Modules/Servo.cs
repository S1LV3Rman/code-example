using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Source
{
    public class Servo : Module, IIndicator
    {
        [SerializeField] private Transform target;

        private Tweener current;
        
        private void Awake()
        {
            Reset();
        }

        public void SetValue(int value)
        {
            var targetEuler = new Vector3(0f, value, 0f);
            var angle = Quaternion.Angle(target.localRotation, Quaternion.Euler(targetEuler));
            var duration = angle / 15f;

            current?.Kill();
            current = target.DOLocalRotate(targetEuler, duration).SetEase(Ease.Linear);
        }

        public void Reset()
        {
            target.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }
}