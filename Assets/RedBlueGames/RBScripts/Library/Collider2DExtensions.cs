﻿using UnityEngine;
using System.Collections;

namespace RedBlue
{
	public static class Collider2DExtensions
	{
		public static void DebugDraw (this Collider2D collider, Color color)
		{
			if (collider.GetType () == typeof(CircleCollider2D)) {
				((CircleCollider2D)collider).DebugDraw (color);
			} else if (collider.GetType () == typeof(BoxCollider2D)) {
				((BoxCollider2D)collider).DebugDraw (color);
			} else if (collider.GetType () == typeof(PolygonCollider2D)) {
				((PolygonCollider2D)collider).DebugDraw (color);
			} else if (collider.GetType () == typeof(EdgeCollider2D)) {
				((EdgeCollider2D)collider).DebugDraw (color);
			} else {
				throw new System.NotImplementedException ("Tried to DebugDraw a collider of unrecognized type. Type: " + collider.GetType ());
			}
		}
	
		public static void DebugDraw (this CircleCollider2D circleCollider, Color color)
		{
			Vector3 scale = circleCollider.transform.localScale;
		
			// Radius scaling mimics how Unity scales the collider
			float scaledRadius = Mathf.Max (Mathf.Abs (scale.x), Mathf.Abs (scale.y)) * circleCollider.radius;
		
			// Apply Transform to Offset
			Vector2 transformedOffset = circleCollider.transform.TransformVector (circleCollider.offset);
		
			DebugUtilities.DrawCircle ((Vector2)circleCollider.transform.position + transformedOffset, scaledRadius, color);
		}
	
		public static void DebugDraw (this BoxCollider2D boxCollider, Color color)
		{
			// Define the corners about the origin
			Vector2 halfSize = boxCollider.size * 0.5f;
			Vector2 cornerTL = new Vector2 (-halfSize.x, halfSize.y);
			Vector2 cornerBR = new Vector2 (halfSize.x, -halfSize.y);
			Vector2 cornerTR = new Vector2 (halfSize.x, halfSize.y);
			Vector2 cornerBL = new Vector2 (-halfSize.x, -halfSize.y);
		
			// Transform the corners
			Transform boxcolliderTransform = boxCollider.transform;
			cornerTL = boxcolliderTransform.TransformVector (cornerTL);
			cornerBR = boxcolliderTransform.TransformVector (cornerBR);
			cornerBL = boxcolliderTransform.TransformVector (cornerBL);
			cornerTR = boxcolliderTransform.TransformVector (cornerTR);
		
			// Get offset transformed
			Vector2 offset = boxcolliderTransform.TransformVector (boxCollider.offset);
			Vector2 position = boxcolliderTransform.position;
			cornerTL += position + offset;
			cornerBR += position + offset;
			cornerBL += position + offset;
			cornerTR += position + offset;
		
			DebugUtilities.DrawPolygon (new Vector2[] {
				cornerTL,
				cornerTR,
				cornerBR,
				cornerBL
			}, color);
		}
	
		public static void DebugDraw (this PolygonCollider2D polyCollider, Color color)
		{
			if (polyCollider.pathCount >= 1) {
				Transform colliderTransform = polyCollider.transform;
				Vector2 position = colliderTransform.position;
				Vector2[] path = polyCollider.GetPath (0);
				Vector2[] transformedPath = new Vector2[path.Length];
			
				// Get transformed Offset
				Vector3 transformedOffset = colliderTransform.TransformVector (polyCollider.offset);
			
				// Transform the points in the path
				for (int i = 0; i < path.Length; i++) {
					transformedPath [i] = colliderTransform.TransformVector (path [i]);
					transformedPath [i] += position + (Vector2)transformedOffset;
				}
			
				DebugUtilities.DrawPolygon (transformedPath, color);
			}
		}
	
		public static void DebugDraw (this EdgeCollider2D edgeCollider, Color color)
		{
			if (edgeCollider.pointCount >= 1) {
				Transform colliderTransform = edgeCollider.transform;
				Vector2 position = colliderTransform.position;
				Vector2[] transformedPath = new Vector2[edgeCollider.points.Length];
			
				// Get transformed Offset
				Vector3 transformedOffset = colliderTransform.TransformVector (edgeCollider.offset);
			
				// Transform the points in the path
				for (int i = 0; i < edgeCollider.points.Length; i++) {
					transformedPath [i] = colliderTransform.TransformVector (edgeCollider.points [i]);
					transformedPath [i] += position + (Vector2)transformedOffset;
				}
			
				DebugUtilities.DrawEdges (transformedPath, color);
			}
		}
	}
}
