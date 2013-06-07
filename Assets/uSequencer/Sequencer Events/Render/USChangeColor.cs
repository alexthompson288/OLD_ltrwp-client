using UnityEngine;
using System.Collections;

[USequencerFriendlyName("Change Color")]
[USequencerEvent("Render/Change Objects Color")]
public class USChangeColor : USEventBase 
{	
	public Color newColor;
	private Color previousColor;
	
	public override void FireEvent()
	{	
		if(!AffectedObject)
			return;
		
		if(!Application.isPlaying && Application.isEditor)
		{
			previousColor = AffectedObject.renderer.sharedMaterial.color;
			AffectedObject.renderer.sharedMaterial.color = newColor;
		}
		else
		{
			previousColor = AffectedObject.renderer.material.color;
			AffectedObject.renderer.material.color = newColor;
		}
	}
	
	public override void ProcessEvent(float deltaTime)
	{
		
	}
	
	public override void StopEvent()
	{
		UndoEvent();
	}
	
	public override void UndoEvent()
	{
		if(!AffectedObject)
			return;
		
		if(!Application.isPlaying && Application.isEditor)
			AffectedObject.renderer.sharedMaterial.color = previousColor;
		else
			AffectedObject.renderer.material.color = previousColor;
	}
}
