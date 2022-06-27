using System;
using System.Collections;
using UnityEngine;

namespace Source
{
    public class Rangefinder : MonoBehaviour
    {
        [SerializeField] private Transform RayStartPoint;
        [SerializeField] private float radius;
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask raycastMask;
        [SerializeField] [Range(0.01f, 1f)] private float updateFrequency = 0.1f;

        private float _distance;
        private bool _isPlaying;
        
        public event Action<int> OnValueChange;

        private void OnEnable()
        {
            StartCoroutine(SensorLoop());
        }

        private void OnDisable()
        {
            _isPlaying = false;
        }

        private IEnumerator SensorLoop()
        {
            _isPlaying = true;
            
            while (_isPlaying)
            {
                _distance = GetDistanceToObstacle() * 100f;
                OnValueChange?.Invoke((int) _distance);
                yield return new WaitForSeconds(updateFrequency);
            }
        }

        private float GetDistanceToObstacle()
        {
            var distance = maxDistance * 0.5f;
            var forward = RayStartPoint.forward;
            var start = RayStartPoint.position;
            var secondStart = start + distance * Vector3.forward;
            
            if (Physics.SphereCast(start, radius * 0.5f, forward,
                out var hit, distance, raycastMask))
            {
                return hit.distance;
            }

            return Physics.SphereCast(secondStart, radius, forward,
                out hit, distance, raycastMask)
                ? hit.distance + distance : maxDistance;
        }
    }
}