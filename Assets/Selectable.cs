using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	private LayerMask unitMASK;
	private LayerMask groundMASK;

	private bool moving;
	private Vector3 movDESTINATION;
	public bool hunted;
	public bool selected;
	public GameObject selection;
	public GameObject mothership;
	private BoxCollider OurCollider;
	private BoxCollider ColliderHit;
	public GameObject moveX;
	private Animator moveXanim;

	private RaycastHit hitDUDE;

	void Start() {
		OurCollider = gameObject.GetComponent<BoxCollider> ();
		moveXanim = moveX.GetComponent<Animator> ();
	}

	void SetMoveXfalse() {
		moveXanim.SetBool ("begin", false);
	}

	void Update () {
		//USELESS SHENANIGANS BEGIN
		transform.LookAt(Camera.main.transform.position);
		//Quaternion rot = Quaternion.LookRotation(Camera.main.transform.position);
		transform.rotation = new Quaternion (0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
		//USELESS SHENANIGANS END

		//MOVE BEGIN
		if (moving) {
			mothership.transform.position = Vector3.MoveTowards (mothership.transform.position, movDESTINATION, 0.1f);

			if (Mathf.Abs (mothership.transform.position.x - movDESTINATION.x) < 0.1f && Mathf.Abs (mothership.transform.position.z - movDESTINATION.z) < 0.1f) {
				moving = false;
				mothership.transform.position = movDESTINATION;
			}
		}
		//MOVE END

		//USELESS RAYCASTS BEGIN
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		//SELECT BEGIN

		if (Input.GetMouseButtonDown (0)) {

			unitMASK = 1 << 9; //WHY
			groundMASK = 1 << 8; //WHY NOT

			if (Physics.Raycast (ray, out hitDUDE, Mathf.Infinity, unitMASK)) {
				ColliderHit = hitDUDE.collider.GetComponent<BoxCollider> ();

				if (ColliderHit == OurCollider)
					selected = !selected;
				/*if (ColliderHit == OurCollider)
					selected = true;
				else
					selected = false;*/
			}

			if (selected)
				selection.transform.localScale = new Vector3 (1f, 1f, 1f);
			else
				selection.transform.localScale = new Vector3 (0f, 0f, 1f);
		}
		//SELECT END
		//USELESS RAYCASTS END

		if (Input.GetMouseButton (1) && selected) {
			Physics.Raycast (ray, out hitDUDE);
			movDESTINATION = new Vector3 (hitDUDE.point.x + Random.Range (-0.5f, 0.5f), 0f, hitDUDE.point.z + Random.Range (-0.5f, 0.5f));
			moveX.transform.position = hitDUDE.point;
			moveXanim.SetBool ("begin", true);
			Invoke ("SetMoveXfalse", 0.1f);
			if (Mathf.Abs (mothership.transform.position.x - movDESTINATION.x) > 1f || Mathf.Abs (mothership.transform.position.z - movDESTINATION.z) > 1f) moving = true;
		}
	}
}
