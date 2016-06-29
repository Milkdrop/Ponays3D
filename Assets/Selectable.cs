using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	private LayerMask unitMASK;
	private LayerMask groundMASK;

	public Vector2 init;
	private bool first = true;
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

	private Vector2 CHUL;
	private Vector2 CHDR;

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
		CHUL = Camera.main.WorldToScreenPoint(new Vector3(OurCollider.bounds.center.x - OurCollider.bounds.extents.x, OurCollider.bounds.center.y + OurCollider.bounds.extents.y, transform.position.z));
		CHDR = Camera.main.WorldToScreenPoint(new Vector3(OurCollider.bounds.center.x + OurCollider.bounds.extents.x, OurCollider.bounds.center.y - OurCollider.bounds.extents.y, transform.position.z));
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
		Physics.Raycast (ray, out hitDUDE, Mathf.Infinity, unitMASK);
		//USELESS RAYCASTS END

		//SELECT BEGIN
		if (Input.GetMouseButton (0)) {

			if (first) {
				init = Input.mousePosition;
				first = false;
			}

			unitMASK = 1 << 9; //WHY
			groundMASK = 1 << 8; //WHY NOT

			if ((((init.x < CHDR.x && init.x > CHUL.x) || (Input.mousePosition.x < CHDR.x && Input.mousePosition.x > CHUL.x)) || (init.x > CHDR.x && Input.mousePosition.x < CHDR.x) || (init.x < CHUL.x && Input.mousePosition.x > CHUL.x))
				&&
				(((init.y > CHDR.y && init.y < CHUL.y) || (Input.mousePosition.y > CHDR.y && Input.mousePosition.y < CHUL.y) || (init.y < CHDR.y && Input.mousePosition.y > CHDR.y) || (init.y > CHUL.y && Input.mousePosition.y < CHUL.y))))
			{
				selected = true;
			} else
				selected = false;

			if (selected)
				selection.transform.localScale = new Vector3 (1f, 1f, 1f);
			else
				selection.transform.localScale = new Vector3 (0f, 0f, 1f);
		} else
			first = true;
		//SELECT END

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
