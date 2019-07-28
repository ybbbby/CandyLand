#pragma strict

private var prevPos: Vector3;
private var prevRot: Quaternion;

var FPC: GameObject;
var root: GameObject;
var realcam: GameObject;
//var Watercam: GameObject;

function Start () {

}

function Update () {
		if(Input.GetAxis ("Mouse X") == 0){
			prevPos = root.transform.position;
			prevRot = root.transform.rotation;
			
									
			//FPC.transform.position = Flycam.transform.position;
			//FPC.transform.rotation = Quaternion.Euler(Flycam.transform.rotation.eulerAngles.x, Flycam.transform.rotation.eulerAngles.y, Flycam.transform.rotation.eulerAngles.z); //0.0f, centerEye.rotation.eulerAngles.y, 0.0f
			//FPC.transform.rotation = Quaternion.Euler(realcam.transform.rotation.eulerAngles.x, realcam.transform.rotation.eulerAngles.y, realcam.transform.rotation.eulerAngles.z);
			FPC.transform.rotation = Quaternion.Euler(0, realcam.transform.rotation.eulerAngles.y, 0);
			FPC.transform.position = new Vector3(realcam.transform.position.x,FPC.transform.position.y,realcam.transform.position.z);

			root.transform.position = prevPos;
			root.transform.rotation = prevRot;
		}
			
}