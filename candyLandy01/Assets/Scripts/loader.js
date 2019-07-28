#pragma strict

function Start () {

}

function Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
		
		//if (Input.GetButtonDown ("Reload")||Input.GetKeyDown (KeyCode.R)) {
			//Application.LoadLevel(Application.loadedLevel);
		//}

		/*
		if (Input.GetButton ("Loadnext")) {
			if (Application.loadedLevel == 0){
				Application.LoadLevel(1);
			}
			else {
				Application.LoadLevel(0);
			}
		}
		*/
		
	}