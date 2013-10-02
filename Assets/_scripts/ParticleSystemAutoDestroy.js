#pragma strict

private var ps : ParticleSystem;
 
////////////////////////////////////////////////////////////////
 
function Start () {
         ps = GetComponent(ParticleSystem);
}
 
function Update () {
    if(ps)
    {
       if(!ps.IsAlive())
       {
         Destroy(gameObject);
       }
    }
}