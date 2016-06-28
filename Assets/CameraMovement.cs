using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private Vector3 vitesse;
	private Vector3 vitez;

	private float xMOVEMENT;
	private float zMOVEMENT;

	private float vitesseX;
	private float vitesseZ;

	void Update () {

		//SCROLLING BEGIN
		gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y - Input.GetAxis ("Mouse ScrollWheel") * 120, gameObject.transform.position.z), ref vitez, 0.2f);

		if (gameObject.transform.position.y > 20f)
			gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, new Vector3 (gameObject.transform.position.x, 20f, gameObject.transform.position.z), ref vitesse, 0.05f);

		if (gameObject.transform.position.y < 3f)
			gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, new Vector3 (gameObject.transform.position.x, 3f, gameObject.transform.position.z), ref vitesse, 0.05f);
		//SCROLLING END

		//MOVEMENT BEGIN
		//JUDGING MOUSE POSITION BEGIN
		if (Input.mousePosition.x < Screen.width * 0.1f)
			xMOVEMENT = - (Screen.width * 0.1f - Input.mousePosition.x) / Screen.width * 0.1f;

		if (Input.mousePosition.x > Screen.width * 0.9f) 
			xMOVEMENT = (Input.mousePosition.x - Screen.width * 0.9f) / Screen.width * 0.1f;

		if (Input.mousePosition.y < Screen.height * 0.1f) 
			zMOVEMENT = (Input.mousePosition.y - Screen.height * 0.1f) / Screen.height * 0.1f;

		if (Input.mousePosition.y > Screen.height * 0.9f) 
			zMOVEMENT = - (Screen.height * 0.9f - Input.mousePosition.y) / Screen.height * 0.1f;
		//JUDGING MOUSE POSITION END

		//RESETING VALUES BEGIN
		if (Input.mousePosition.x > Screen.width * 0.1f && Input.mousePosition.x < Screen.width * 0.9f) xMOVEMENT = 0;//xMOVEMENT = Mathf.SmoothDamp(xMOVEMENT, 0f, ref vitesseX, 0.05f);
		if (Input.mousePosition.y > Screen.height * 0.1f && Input.mousePosition.y < Screen.height * 0.9f) zMOVEMENT = 0;//yMOVEMENT = Mathf.SmoothDamp(yMOVEMENT, 0f, ref vitesseY, 0.05f);

		gameObject.transform.position = new Vector3 (gameObject.transform.position.x + xMOVEMENT * 32, gameObject.transform.position.y, gameObject.transform.position.z + zMOVEMENT * 32);
	}
}
