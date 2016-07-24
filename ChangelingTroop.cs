using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangelingTroop : MonoBehaviour {


	//MOVEMENT AND PATHFINDING//
	public float speed;
	private bool positiveCycler;
	private float positiveCyclerDist;
	private float negativeCyclerDist;
	public Collider previousWPcollider;
	private Vector3[] obstacleCoord;
	private int cycler;
	private float minimumdist;
	public float offsetX;
	public float offsetZ;
	private float mindist;
	private Vector3 NextWaypoint;
	private Vector3 nextWAYPOINTdest;
	public Collider target;
	public Vector3 movDESTINATION;
	public RaycastHit waypointHIT;
	public LayerMask wpSearchLayer;
	private LayerMask groundMASK = 1 << 8;
	private LayerMask foodMASK = 1 << 10;
	private Ray wpRay;
	private Ray obstacleRay;

	//GATHERING SYSTEM//
	public Text FoodCountGUI;
	public Resources22 resources;
	public GameObject currentgather;
	private GameObject[] FoodDrop;
	private Vector3 originalgatherposition;
	public int food;
	public float gatherspeed;
	public RaycastHit gatherHIT;
	public Vector3 gatherHITpoint;
	Ray gatherRay;
	private int mini;

	//RECONAISSANCE//
	public int specie;
	public string tip;

	//MISC//
	public bool building;
	public GameObject ponermodel;
	private Animation anim;
	public LifeBar Lbar;
	public LifeBar GUILbar;
	public GameObject basicSupplicant;
	public int job;
	public GameObject selection;
	public int initlief = 100;
	public int life = 100;
	public Selectable selscript;
	public RaycastHit hitDUDE;
	Ray rayer;

	void Start() {
		if (!building) {
			anim = ponermodel.GetComponent<Animation> ();
			a_idle ();
			obstacleCoord = new Vector3[4];
			rayer.origin = transform.position;
			rayer.direction = Vector3.down;
		}
	}

	public void job0() { //DELETE ALL CURRENT JOBS
		CancelInvoke ("decrementLife");
		CancelInvoke ("gatherFood");
	}

	public void job1() {
		InvokeRepeating ("decrementLife", 0.1f, 0.1f);
	}

	public void job2() {
		InvokeRepeating ("gatherFood", gatherspeed, gatherspeed);
	}

	public void gototarget(GameObject targeter) {
		gatherRay.origin = gameObject.transform.position;
		gatherRay.direction = targeter.transform.position - transform.position;
		Physics.Raycast (gatherRay, out gatherHIT, Mathf.Infinity, foodMASK);
		movDESTINATION = new Vector3 (gatherHIT.point.x, gatherHIT.collider.gameObject.GetComponent<BoxCollider>().bounds.center.y - gatherHIT.collider.gameObject.GetComponent<BoxCollider>().bounds.extents.y, gatherHIT.point.z);
		findnodes ();
		a_walk ();
		CancelInvoke ("mover");
		CancelInvoke ("arrangeheight");
		InvokeRepeating ("mover", 0f, 0.025f);
		InvokeRepeating ("arrangeheight", 0.05f, 0.05f);
	}

	void gatherFood() {
		food++;
		if(FoodCountGUI != null) FoodCountGUI.text = food + "";
		if (food >= 10) {
			mindist = Mathf.Infinity;
			FoodDrop = GameObject.FindGameObjectsWithTag ("FoodDrop");
			for (int i = 0; i <= FoodDrop.Length - 1; i++) {
				float dist = (float)Mathf.Sqrt (Mathf.Pow (Mathf.Abs (transform.position.x - FoodDrop [i].transform.position.x), 2) + Mathf.Pow (Mathf.Abs (transform.position.z - FoodDrop [i].transform.position.z), 2));
				if (dist <= mindist) {
					mindist = dist;
					mini = i;
				}
			}
			target = FoodDrop [mini].GetComponent<BoxCollider> ();
			gototarget (FoodDrop [mini]);
			job = -2;
			CancelInvoke ("gatherFood");
		}
	}

	void decrementLife() {
		life--;
		Lbar.Upd8Life ();
		if (life <= 0) {
			a_die ();
			selscript.death ();
			CancelInvoke ("decrementLife");
		}
	}

	void arrangeheight() {
		rayer.origin = transform.position;
		Physics.Raycast (rayer, out hitDUDE, Mathf.Infinity, groundMASK);
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - (hitDUDE.distance - 0.548f), transform.localPosition.z);
	}

	public void findnodes() {
		wpRay.origin = transform.position;
		wpRay.direction = new Vector3 (movDESTINATION.x, transform.position.y, movDESTINATION.z) - transform.position;
		Physics.Raycast (wpRay, out waypointHIT, Vector3.Distance(transform.position, movDESTINATION), wpSearchLayer);

		if (waypointHIT.collider == null) 
			Physics.Raycast (wpRay, out waypointHIT, 0.05f, wpSearchLayer); //if character is inside a collider
		
		if (waypointHIT.collider == null || target == waypointHIT.collider)
			nextWAYPOINTdest = movDESTINATION;
		else {
			if (previousWPcollider != waypointHIT.collider) {
				cycler = 4;
				previousWPcollider = waypointHIT.collider;
				/*minX minZ*/ obstacleCoord [0] = new Vector3(waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.x - waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.x - 1.5f, 0f, waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.z - waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.z - 1.5f);
				/*minX maxZ*/ obstacleCoord [1] = new Vector3(waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.x - waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.x - 1.5f, 0f, waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.z + waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.z + 1.5f);
				/*maxX maxZ*/ obstacleCoord [2] = new Vector3(waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.x + waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.x + 1.5f, 0f, waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.z + waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.z + 1.5f);
				/*maxX minZ*/ obstacleCoord [3] = new Vector3(waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.x + waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.x + 1.5f, 0f, waypointHIT.collider.GetComponent<BoxCollider>().bounds.center.z - waypointHIT.collider.GetComponent<BoxCollider>().bounds.extents.z - 1.5f);

				minimumdist = Mathf.Infinity;
				for (int i = 0; i <= 3; i++) 
					if (Vector3.Distance (transform.position, obstacleCoord[i]) < minimumdist && cycler != i) {
						cycler = i;
						minimumdist = Vector3.Distance (transform.position, obstacleCoord [i]);
					}

				//Set Distances BEGIN
				if (cycler == 3) 
					positiveCyclerDist = Vector3.Distance (obstacleCoord [cycler], obstacleCoord [0]);
				else
					positiveCyclerDist = Vector3.Distance (obstacleCoord [cycler], obstacleCoord [cycler + 1]);

				if (cycler == 0) 
					negativeCyclerDist = Vector3.Distance (obstacleCoord [3], obstacleCoord [cycler]);
				else
					negativeCyclerDist = Vector3.Distance (obstacleCoord [cycler - 1], obstacleCoord [cycler]);
				//Set Distances END

				if (positiveCyclerDist < negativeCyclerDist)
					positiveCycler = true;
				else
					positiveCycler = false;
			}

			if (positiveCycler == true) {
				cycler++;
				if (cycler == 4)
					cycler = 0;
			} else {
				cycler--;
				if (cycler == -1)
					cycler = 3;
			}
			nextWAYPOINTdest = obstacleCoord [cycler];
		}
		CancelInvoke ("rotater");
		InvokeRepeating ("rotater", 0f, 0.05f);
	}

	void rotater() {
		ponermodel.transform.rotation = Quaternion.Lerp (ponermodel.transform.rotation, Quaternion.LookRotation(new Vector3 (nextWAYPOINTdest.x, ponermodel.transform.position.y, nextWAYPOINTdest.z) - ponermodel.transform.position), 0.3f);
	}

	public void a_walk() {
		anim.clip = anim ["Pony_Rig|Walk"].clip;
		anim.Play ();
	}

	public void a_idle() {
		anim.clip = anim ["Pony_Rig|Idle"].clip;
		anim.Play ();
	}

	public void a_die() {
		anim.clip = anim ["Pony_Rig|Die"].clip;
		anim.Play ();
	}

	public void startmovin() {
		a_walk ();
		previousWPcollider = null;
		findnodes ();
		CancelInvoke ("mover");
		CancelInvoke ("arrangeheight");
		InvokeRepeating ("mover", 0f, 0.025f);
		InvokeRepeating ("arrangeheight", 0.05f, 0.05f);
	}

	void mover() {
		transform.position += ponermodel.transform.forward * speed * UpgradeManager.TroopSpeed;
	
		if (Mathf.Abs (transform.position.x - nextWAYPOINTdest.x) < 0.3f && Mathf.Abs (transform.position.z - nextWAYPOINTdest.z) < 0.3f) { //WE REACHED DESTINATION
			if (nextWAYPOINTdest == movDESTINATION) {
				CancelInvoke ("rotater");
				a_idle ();
				if (job == 0)
					job0 ();
				if (job == 1)
					job1 ();
				if (job == 2) {
					originalgatherposition = movDESTINATION;
					job2 ();
				}

				CancelInvoke ("mover");
				CancelInvoke ("arrangeheight");

				if (job == -2) {
					resources.food += food;
					resources.upd8food ();
					food = 0;
					if (FoodCountGUI != null)
						FoodCountGUI.text = food + "";
					job = 2;
					movDESTINATION = originalgatherposition;
					a_walk ();
					InvokeRepeating ("mover", 0f, 0.025f);
					InvokeRepeating ("arrangeheight", 0.05f, 0.05f);
				}
			} else
				findnodes ();
		}
	}
}
