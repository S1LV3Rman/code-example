using System;
using UnityEngine;

namespace Source
{
    public class Motor : MonoBehaviour
    {
        [SerializeField] private float brakePower;
        [SerializeField] private float sensitivity = 1.0f;
        
        public WheelCollider leftFrontWheel;
        public WheelCollider leftBackWheel;
        public WheelCollider rightFrontWheel;
        public WheelCollider rightBackWheel;

        private Transform leftFrontTransform;
        private Transform leftBackTransform;
        private Transform rightFrontTransform;
        private Transform rightBackTransform;

        private void Awake()
        {
            leftFrontTransform = leftFrontWheel.transform.GetChild(0);
            leftBackTransform = leftBackWheel.transform.GetChild(0);
            rightFrontTransform = rightFrontWheel.transform.GetChild(0);
            rightBackTransform = rightBackWheel.transform.GetChild(0);
            
            BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnStop, Stop);
        }

        private void Update()
        {
            UpdateWheelsPosition();
        }

        private void OnDestroy()
        {
            BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnStop, Stop);
        }

        public void Stop()
        {
            leftFrontWheel.motorTorque = 0f;
            leftBackWheel.motorTorque = 0f;
            leftFrontWheel.brakeTorque = brakePower;
            leftBackWheel.brakeTorque = brakePower;

            rightFrontWheel.motorTorque = 0f;
            rightBackWheel.motorTorque = 0f;
            rightFrontWheel.brakeTorque = brakePower;
            rightBackWheel.brakeTorque = brakePower;
        }

        public void Move(float leftSpeed, float rightSpeed)
        {
            leftFrontWheel.motorTorque = leftSpeed * sensitivity;
            leftBackWheel.motorTorque = leftSpeed * sensitivity;
            if (leftSpeed == 0f)
            {
                leftFrontWheel.brakeTorque = brakePower;
                leftBackWheel.brakeTorque = brakePower;
            }
            else
            {
                leftFrontWheel.brakeTorque = 0f;
                leftBackWheel.brakeTorque = 0f;
            }
        
            rightFrontWheel.motorTorque = rightSpeed * sensitivity;
            rightBackWheel.motorTorque = rightSpeed * sensitivity;
            if (rightSpeed == 0f)
            {
                rightFrontWheel.brakeTorque = brakePower;
                rightBackWheel.brakeTorque = brakePower;
            }
            else
            {
                rightFrontWheel.brakeTorque = 0f;
                rightBackWheel.brakeTorque = 0f;
            }
        }

        private void UpdateWheelsPosition()
        {
            leftFrontWheel.GetWorldPose(out var position, out var rotation);
            leftFrontTransform.rotation = rotation;
            leftFrontTransform.position = position;
            
            leftBackWheel.GetWorldPose(out position, out rotation);
            leftBackTransform.rotation = rotation;
            leftBackTransform.position = position;
            
            rightFrontWheel.GetWorldPose(out position, out rotation);
            rightFrontTransform.rotation = rotation;
            rightFrontTransform.position = position;
            
            rightBackWheel.GetWorldPose(out position, out rotation);
            rightBackTransform.rotation = rotation;
            rightBackTransform.position = position;
        }
    }
}