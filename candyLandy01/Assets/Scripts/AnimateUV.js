 
#pragma strict
var scrollSpeed :float = 0.5;
var u : boolean = false;
var v : boolean = false;
private var tick : int =0;
 
function Update () {
tick ++;
if (tick %2) return;
 
var offset : float = Time.time * scrollSpeed % 1;
if (u == true && v == true)
{
GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offset,offset));
}
else if (u == true)
{
GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offset,0));
}
else if (v == true)
{
GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(0,offset));
}
 
tick = tick %50;
 
}
 