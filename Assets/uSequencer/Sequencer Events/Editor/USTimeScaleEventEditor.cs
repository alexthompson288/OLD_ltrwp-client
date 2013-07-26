using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomUSEditor(typeof(USTimeScaleEvent))]
public class USTimeScaleEventEditor : USEventBaseEditor
{
	new public Rect RenderEvent(Rect myArea, USEventBase thisEvent)
	{
		USTimeScaleEvent timeScaleEvent = thisEvent as USTimeScaleEvent;

		if (!timeScaleEvent)
			Debug.LogWarning("Trying to render an event as a USTimeScaleEvent, but it is a : " + thisEvent.GetType().ToString());
		
		thisEvent.Duration = timeScaleEvent.scaleCurve[timeScaleEvent.scaleCurve.length-1].time;
		
		// Draw our Whole Box.
		if (thisEvent.Duration > 0)
		{
			float endPosition = USControl.convertTimeToEventPanePosition(thisEvent.Firetime + thisEvent.Duration);
			myArea.width = endPosition - myArea.x;
		}
		DrawDefaultBox(myArea, thisEvent);

		GUILayout.BeginArea(myArea);
		{
			GUILayout.Label(GetReadableEventName(thisEvent), defaultBackground);
		}
		GUILayout.EndArea();

		return myArea;
	}
}
