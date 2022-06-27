using System;
using System.Linq;
using CW.Common;
using Lean.Gui;
using Lean.Touch;
using UnityEngine;

namespace Source
{
    public class ZoomCamera : MonoBehaviour
    {
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

		public float Value => camera.localPosition.z;
		
		[SerializeField]
		private float remainingDelta;

		private bool _active;

		private Transform camera;

		private void Awake()
		{
			camera = transform.GetChild(0);
			_active = true;
		}

		protected virtual void LateUpdate()
		{
			if (!_active) return;
			
			float wheelDelta = 0f;
			
			var finger = LeanTouch.Fingers.FirstOrDefault(f => f.Index == LeanTouch.HOVER_FINGER_INDEX);
			if (finger is {IsOverGui: false})
				wheelDelta = Input.mouseScrollDelta.y;
			
			Zoom(wheelDelta);
		}

		public void Zoom(float wheelDelta)
		{
			var zoom = camera.localPosition.z;

			var scaledDelta = wheelDelta * sensitivity * zoom / -5f;

			var delta = Mathf.Clamp(zoom + remainingDelta + scaledDelta, -20f, -1f) - zoom - remainingDelta;

			remainingDelta += delta;

			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			var newRemainingDelta = Mathf.Lerp(remainingDelta, 0, factor);

			camera.localPosition = new Vector3(0f, 0f, zoom + remainingDelta - newRemainingDelta);

			if (wheelDelta == 0 && inertia > 0.0f && damping > 0.0f)
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
	using TARGET = ZoomCamera;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class ZoomCamera_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
		}
	}
}
#endif