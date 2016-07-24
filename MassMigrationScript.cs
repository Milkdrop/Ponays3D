using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MassMigrationScript : MonoBehaviour {
	public GameObject formation;
	private Formation formscript;
	public Vector2 originalPos;
	public GameObject infotext;
	public AudioSource CHworking;
	public AudioSource PAAworking;

	private ChangelingTroop troopscript;
	public GameObject basicsupplicant;
	private Animator targetanim;
	public GameObject[] selectableScripts;
	public GameObject formationator;
	public static GameObject[] formarray;
	public bool moving;
	private RaycastHit hitDUDE;
	public GameObject moveX;
	private Animator moveXanim;
	private Vector3 movDESTINY;
	public BoxCollider surroundSquare;
	private float rand;

	public Vector3 formpos;
	public Vector3 oldformpos;
	private Ray ray;
	private LayerMask groundMASK = 1 << 8;

	void Start() {
		moveXanim = moveX.GetComponent<Animator> ();
		formscript = formation.GetComponent<Formation> ();
	}

	void SetMoveXfalse() {
		moveXanim.SetBool ("begin", false);
	}

	void SetSelfalse() {
		targetanim.SetBool ("sel", false);
	}

	void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (ray, out hitDUDE, Mathf.Infinity);

		if (Input.GetMouseButtonDown (1) && SelectionLines.validmouse) {

			if (hitDUDE.collider.tag == "Terra") {
				
				if (SelectedTroops.TotalNB >= 1) {
					moveX.transform.position = new Vector3 (hitDUDE.point.x + 0.1f, hitDUDE.point.y + 0.3f, hitDUDE.point.z - 0.1f);
					moveXanim.SetBool ("begin", true);
					Invoke ("SetMoveXfalse", 0.1f);
				}
			} else {
				if (SelectedTroops.ChangelingsNB >= 1) {
					targetanim = hitDUDE.collider.transform.GetChild (0).GetComponent<Animator> ();
					targetanim.SetBool ("sel", true);
					Invoke ("SetSelfalse", 0.05f);
					surroundSquare = hitDUDE.collider.gameObject.transform.GetChild (1).GetComponent<BoxCollider> ();
				}
			}

			for (int i = 1; i <= formation.transform.childCount - 1; i++) {
				troopscript = formation.transform.GetChild (i).GetComponent<ChangelingTroop> ();
				troopscript.job = 0;
				troopscript.job0 ();
				if (hitDUDE.collider.tag != "Terra" && troopscript.tip == "Villager") {
					surroundSquare = hitDUDE.collider.GetComponent<BoxCollider> ();

					if (hitDUDE.collider.tag == "vine")
						troopscript.job = 1;
					
					if (hitDUDE.collider.tag == "AppleTree") {
						troopscript.currentgather = hitDUDE.collider.gameObject;
						troopscript.job = 2;
					}

					if (hitDUDE.collider.tag == "FoodDrop") {
						if (troopscript.food > 0) {
							troopscript.job = -2;
							troopscript.CancelInvoke ("gatherFood");
						}
						troopscript.target = hitDUDE.collider;
						troopscript.gototarget (hitDUDE.collider.gameObject);
						goto endDrop;
					}

					troopscript.target = surroundSquare;
					rand = Random.Range (1, 5);
					if (rand % 4 == 0)
						troopscript.movDESTINATION = new Vector3 (Random.Range (surroundSquare.bounds.center.x - surroundSquare.bounds.extents.x, surroundSquare.bounds.center.x + surroundSquare.bounds.extents.x), hitDUDE.point.y, surroundSquare.bounds.center.z - surroundSquare.bounds.extents.z);
					if (rand % 4 == 1)
						troopscript.movDESTINATION = new Vector3 (surroundSquare.bounds.center.x - surroundSquare.bounds.extents.x, hitDUDE.point.y, Random.Range (surroundSquare.bounds.center.z - surroundSquare.bounds.extents.z, surroundSquare.bounds.center.z + surroundSquare.bounds.extents.z));
					if (rand % 4 == 2)
						troopscript.movDESTINATION = new Vector3 (Random.Range (surroundSquare.bounds.center.x - surroundSquare.bounds.extents.x, surroundSquare.bounds.center.x + surroundSquare.bounds.extents.x), hitDUDE.point.y, surroundSquare.bounds.center.z + surroundSquare.bounds.extents.z);
					if (rand % 4 == 3)
						troopscript.movDESTINATION = new Vector3 (surroundSquare.bounds.center.x + surroundSquare.bounds.extents.x, hitDUDE.point.y, Random.Range (surroundSquare.bounds.center.z - surroundSquare.bounds.extents.z, surroundSquare.bounds.center.z + surroundSquare.bounds.extents.z));
					
				} else {
					troopscript.target = null;
					troopscript.movDESTINATION = new Vector3 (hitDUDE.point.x - troopscript.offsetX, hitDUDE.point.y, hitDUDE.point.z - troopscript.offsetZ);
				}

				if (SelectedTroops.ChangelingsNB >= 1)
					CHworking.Play ();
				if (SelectedTroops.PAANB >= 1)
					PAAworking.Play ();

				troopscript.startmovin ();
				endDrop:;
			}		
		}
	}
}

