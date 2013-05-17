/*
EasyScale Script v.1.0
Copyright © 2012 Unluck Software - Egil A Larsen
www.chemicalbliss.com
*/
#pragma strict
@script ExecuteInEditMode()
var multiplier:float = 1;
var _startSize:float =0;
var _gravityModifier:float=0;
var _startSpeed:float=0;

function OnDrawGizmosSelected () {
	if(Application.isEditor){
		if(_startSize ==0){
			_startSize = transform.particleSystem.startSize;
			_gravityModifier = transform.particleSystem.gravityModifier;
			_startSpeed = transform.particleSystem.startSpeed;
		}else{
		transform.particleSystem.startSize = _startSize * multiplier;
		transform.particleSystem.gravityModifier = _gravityModifier * multiplier;
		transform.particleSystem.startSpeed = _startSpeed * multiplier;
		}
	}
}