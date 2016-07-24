using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject parentsupplicant;
	public GameObject[] debug0;
	public GameObject chgl;
	public GameObject chglSupplicant;
	private Material chglmat;

	public float minPoint;
	public float maxPoint;

	private float new0;
	private bool specMovement;
	private bool noZ;
	private Vector3 vitesse;
	private Vector3 vitez;

	private float xMOVEMENT;
	private float zMOVEMENT;

	private float vitesseX;
	private float vitesseZ;

	private LayerMask unitMASK = 1 << 9;
	private LayerMask GroundMASK = 1 << 8;
	public LayerMask BuildingMASK;
	private RaycastHit hitDUDE;
	public static RaycastHit hitDUDE2;
	private Ray ray1;
	private Ray ray;

	private TerrainCollider col;

	void Start () {
		ray.direction = Vector3.down;
	}

	void Update () {
		//DEBUG BEGIN
		if (Input.GetKey (KeyCode.Space)) {
			Instantiate (chgl, new Vector3 (Random.Range (240, 260), 32f, Random.Range (340, 360)), Quaternion.identity);

			debug0 = GameObject.FindGameObjectsWithTag ("troopunsel");
			debug0 [debug0.Length - 1].transform.SetParent (parentsupplicant.transform.parent);
			debug0 [debug0.Length - 1].GetComponentInChildren<Renderer>().material.SetColor("_mask1", new Color(1, (float) Random.Range(0.294f, 0.729f), 0.254f));
			debug0 [debug0.Length - 1].GetComponentInChildren<Renderer>().material.SetColor("_mask2", new Color(1, (float) Random.Range(0.294f, 0.729f), 0.254f));
		}

		ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (ray1, out hitDUDE2, Mathf.Infinity);
		//USELESS RAYCASTS BEGIN
		ray.origin = Camera.main.transform.position;
		Physics.Raycast (ray, out hitDUDE, Mathf.Infinity, GroundMASK);
		//USELESS RAYCASTS END
		new0 = Camera.main.transform.position.y - hitDUDE.distance;

		//SCROLLING BEGIN
		if (noZ) gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y - Input.GetAxis ("Mouse ScrollWheel") * 100, gameObject.transform.position.z), ref vitez, 0.2f);
		else gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y - Input.GetAxis ("Mouse ScrollWheel") * 100, gameObject.transform.position.z + Input.GetAxis ("Mouse ScrollWheel") * 200 * (float) Mathf.Sqrt(3)/2), ref vitez, 0.2f);

		if (gameObject.transform.position.y > new0 + maxPoint) {
			noZ = true;
			gameObject.transform.position = Vector3.SmoothDamp (gameObject.transform.position, new Vector3 (gameObject.transform.position.x, new0 + maxPoint, gameObject.transform.position.z), ref vitesse, 0.01f);
		}

		if (gameObject.transform.position.y < new0 + minPoint) {
			noZ = true;
			gameObject.transform.position = Vector3.SmoothDamp (gameObject.transform.position, new Vector3 (gameObject.transform.position.x, new0 + minPoint, gameObject.transform.position.z), ref vitesse, 0.01f);
		}

		if (gameObject.transform.position.y >= new0 + minPoint && gameObject.transform.position.y <= new0 + maxPoint)
			noZ = false;
		//SCROLLING END

		//MOVEMENT BEGIN
		//JUDGING MOUSE POSITION BEGIN
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			if (Input.mousePosition.x < Screen.width * 0.1f) 
				xMOVEMENT = -(Screen.width * 0.1f - Input.mousePosition.x) / Screen.width * 0.1f;

			if (Input.mousePosition.x > Screen.width * 0.9f)
				xMOVEMENT = (Input.mousePosition.x - Screen.width * 0.9f) / Screen.width * 0.1f;

			if (Input.mousePosition.y < Screen.height * 0.1f)
				zMOVEMENT = (Input.mousePosition.y - Screen.height * 0.1f) / Screen.height * 0.1f;

			if (Input.mousePosition.y > Screen.height * 0.9f)
				zMOVEMENT = -(Screen.height * 0.9f - Input.mousePosition.y) / Screen.height * 0.1f;
		}

		if (Input.mousePosition.x == 0 || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
			xMOVEMENT = -0.02f * ((float) (gameObject.transform.position.y - new0) / maxPoint);

		if (Input.mousePosition.x >= Screen.width - 1 || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
			xMOVEMENT = 0.02f * ((float) (gameObject.transform.position.y - new0) / maxPoint);

		if (Input.mousePosition.y == 0 || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow))
			zMOVEMENT = -0.02f * ((float) (gameObject.transform.position.y - new0) / maxPoint);

		if (Input.mousePosition.y >= Screen.height - 1 || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow))
			zMOVEMENT = 0.02f * ((float) (gameObject.transform.position.y - new0) / maxPoint);


		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)
			||
			Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.RightArrow))
			specMovement = true;
		else
			specMovement = false;

		//JUDGING MOUSE POSITION END

		//RESETING VALUES BEGIN
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			if (Input.mousePosition.x > Screen.width * 0.1f && Input.mousePosition.x < Screen.width * 0.9f && !specMovement)
				xMOVEMENT = 0;//xMOVEMENT = Mathf.SmoothDamp(xMOVEMENT, 0f, ref vitesseX, 0.05f);
			if (Input.mousePosition.y > Screen.height * 0.1f && Input.mousePosition.y < Screen.height * 0.9f && !specMovement)
				zMOVEMENT = 0;//yMOVEMENT = Mathf.SmoothDamp(yMOVEMENT, 0f, ref vitesseY, 0.05f);
		} else {
			if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width - 1 && !specMovement)
				xMOVEMENT = 0;

			if (Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height - 1 && !specMovement)
				zMOVEMENT = -0; // -0 is the most powerful number
		}

		if (!(Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) && specMovement)
			xMOVEMENT = 0;

		if (!(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) && !(Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow)) && specMovement)
			zMOVEMENT = 0;

		gameObject.transform.position = new Vector3 (gameObject.transform.position.x + xMOVEMENT * 32, gameObject.transform.position.y, gameObject.transform.position.z + zMOVEMENT * 32);
	}
}
