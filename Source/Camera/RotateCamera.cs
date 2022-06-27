using CW.Common;
using Lean.Touch;
using UnityEngine;

namespace Source
{
    public class RotateCamera : MonoBehaviour
    {
        /// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The movement speed will be multiplied by this.
		/// -1 = Inverted Controls.</summary>
		public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } } [SerializeField] private float sensitivity = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = -1.0f;

		/// <summary>This allows you to control how much momentum is retained when the dragging fingers are all released.
		/// NOTE: This requires <b>Damping</b> to be above 0.</summary>
		public float Inertia { set { inertia = value; } get { return inertia; } } [SerializeField] [Range(0.0f, 1.0f)] private float inertia;

		[SerializeField]
		private float remainingDelta;

		private bool _active;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
			_active = true;
		}

		protected virtual void LateUpdate()
		{
			if (!_active) return;
			
			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Get the last and current screen point of all fingers
			var lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
			var screenPoint     = LeanGesture.GetScreenCenter(fingers);

			// Get delta of them
			var delta = (screenPoint - lastScreenPoint).x;

			// Store the current rotation
			var localRotation = transform.localRotation;

			// Add to remainingDelta
			remainingDelta += delta * sensitivity;

			// Get t value
			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingDelta = Mathf.Lerp(remainingDelta, 0, factor);

			// Shift this rotation by the change in delta
			transform.localRotation = Quaternion.Euler(
				localRotation.eulerAngles.x,
				localRotation.eulerAngles.y + remainingDelta - newRemainingDelta,
				localRotation.eulerAngles.z);

			if (fingers.Count == 0 && inertia > 0.0f && damping > 0.0f)
			{
				newRemainingDelta = Mathf.Lerp(newRemainingDelta, remainingDelta, inertia);
			}

			// Update remainingDelta with the dampened value
			remainingDelta = newRemainingDelta;
		}

		public void Lock()
		{
			_active = false;
		}

		public void Unlock()
		{
			_active = true;
		}
    }
}

#if UNITY_EDITOR
namespace Source
{
	using UnityEditor;
	using TARGET = RotateCamera;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class RotateCamera_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
		}
	}
}
#endif