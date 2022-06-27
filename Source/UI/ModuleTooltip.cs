using Lean.Common;
using Lean.Gui;
using Source;
using UnityEngine;

namespace Source
{
    public class ModuleTooltip : LeanTooltip
    {
	    [SerializeField]
	    private Camera camera;
	    
	    [SerializeField]
	    private bool followMouse = true;

	    [System.NonSerialized]
	    private RectTransform cachedParentRect;

	    [System.NonSerialized]
	    private bool cachedParentRectSet;

	    [System.NonSerialized]
	    private Vector2 defaultPivot;

	    private static Vector3[] parentCorners = new Vector3[4];
        protected override void Update()
        {
	        if (cachedRectTransform == false)
	        {
		        var parent = transform.parent;
		        if (parent != null)
		        {
			        cachedParentRect = parent.GetComponent<RectTransform>();
			        cachedParentRectSet = true;
		        }
		        
		        cachedRectTransform    = GetComponent<RectTransform>();
		        defaultPivot           = cachedRectTransform.pivot;
		        cachedRectTransformSet = true;
			}

			var finalData  = default(LeanTooltipData);
			var finalPoint = default(Vector3);
			var mousePoint = default(Vector2);

			switch (Activation)
			{
				case ActivationType.HoverOrPress:
				{
					if (HoverShow == true)
					{
						finalData  = HoverData;
						mousePoint = HoverPointer.position;
					}
				}
				break;

				case ActivationType.Hover:
				{
					if (HoverShow == true && PressShow == false)
					{
						finalData  = HoverData;
						mousePoint = HoverPointer.position;
					}
				}
				break;

				case ActivationType.Press:
				{
					if (PressShow == true && HoverShow == true && HoverData == PressData)
					{
						finalData  = PressData;
						mousePoint = PressPointer.position;
					}
				}
				break;
			}

			if (Move == true)
			{
				RectTransformUtility.ScreenPointToWorldPointInRectangle(
					cachedParentRect, mousePoint, camera, out finalPoint);
			}

			if (tooltip != finalData)
			{
				currentDelay  = 0.0f;
				tooltip       = finalData;
				shown         = false;

				Hide();
			}

			if (tooltip != null)
			{
				cachedRectTransform.pivot = defaultPivot;
				currentDelay += Time.unscaledDeltaTime;

				if (currentDelay >= ShowDelay)
				{
					if (shown == false)
					{
						Show();
					}

					if (Move == true)
					{
						cachedRectTransform.position = finalPoint;
					}
				}
			}

			if (Move == true && Boundary != BoundaryType.None)
			{
				cachedRectTransform.GetWorldCorners(corners);

				var min       = Vector2.Min(corners[0], Vector2.Min(corners[1], Vector2.Min(corners[2], corners[3])));
				var max       = Vector2.Max(corners[0], Vector2.Max(corners[1], Vector2.Max(corners[2], corners[3])));
				var pivot     = cachedRectTransform.pivot;
				var position  = cachedRectTransform.position;
				var boundaryX = Boundary;
				var boundaryY = Boundary;

				Vector2 minBounds;
				Vector2 maxBounds;
				if (cachedParentRectSet)
				{
					cachedParentRect.GetWorldCorners(parentCorners);
					
					minBounds = Vector2.Min(parentCorners[0], Vector2.Min(parentCorners[1], Vector2.Min(parentCorners[2], parentCorners[3])));
					maxBounds = Vector2.Max(parentCorners[0], Vector2.Max(parentCorners[1], Vector2.Max(parentCorners[2], parentCorners[3])));
				}
				else
				{
					minBounds = camera.ScreenToWorldPoint(Vector3.zero);
					maxBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
				}

				// If the tooltip cannot be flipped at its current position & size, revert to Boundary = Position?
				if (Boundary == BoundaryType.FlipPivot)
				{
					var size = max - min;

					if (finalPoint.x - size.x < minBounds.x && finalPoint.x + size.x > maxBounds.x)
					{
						boundaryX = BoundaryType.ShiftPosition;
					}

					if (finalPoint.y - size.y < minBounds.y && finalPoint.y + size.y > maxBounds.y)
					{
						boundaryY = BoundaryType.ShiftPosition;
					}
				}

				if (boundaryX == BoundaryType.FlipPivot)
				{
					if (min.x < minBounds.x) pivot.x = 0.0f; 
					else if (max.x > maxBounds.x) pivot.x = 1.0f;
				}
				else if (boundaryX == BoundaryType.ShiftPosition)
				{
					if (min.x < minBounds.x) position.x += minBounds.x - min.x; 
					else if (max.x > maxBounds.x) position.x += maxBounds.x - max.x;
				}

				if (boundaryY == BoundaryType.FlipPivot)
				{
					if (min.y < minBounds.y) pivot.y = 0.0f; 
					else if (max.y > maxBounds.y) pivot.y = 1.0f;
				}
				else if (boundaryY == BoundaryType.ShiftPosition)
				{
					if (min.y < minBounds.y) position.y += minBounds.y - min.y; 
					else if (max.y > maxBounds.y) position.y += maxBounds.y - max.y;
				}

				cachedRectTransform.pivot    = pivot;
				cachedRectTransform.position = position;
			}
        }
    }
}
