using UnityEngine;
using System.Collections;

public class SelectionLines : MonoBehaviour {
	
	public SelectableBuilding bldscript;
	public GameObject formation;
	private ChangelingTroop troopscript;
	private Formation formscript;
	public static Vector2 init;
	public Vector2 initdebug;
	private GameObject[] selectableScripts;
	private GameObject[] buildingScripts;
	private Selectable selscript;
	private SelectableBuilding buildscript;
	public bool alreadyunsel = true;
	public SelectedTroopsManager SelTroMan;
	public static bool validmouse;
	public bool fist = true;
	public GameObject cursor;
	public bool validated;
	public static bool drawing;
	public bool first = true;
	public Texture texture;
	private Rect rect;

	public bool finseldebug;
	public bool drawdebug;

	public bool validinit;

	public int a;
	public bool rarest;

	//SETTING VARIABLES BEGIN
	void Start () { 
		formscript = formation.GetComponent<Formation> ();
		drawing = false;
	}
	//SETTING VARIABLES END

	void aranj() { //METHOD USED FOR DELAYING THE ARRANGEMENT
		SelTroMan.Arrange ();
	}

	void Update () {
		if (Input.mousePosition.y >= Screen.height * 0.23044f && Input.mousePosition.y <= Screen.height * 0.97251f) //CHECKING IF THE MOUSE IS NOT OVER THE GUI
			validmouse = true;
		else
			validmouse = false;

		if (Input.GetMouseButtonDown (0)) //USER BEGINS TO DRAW, BUT NOT REALLY
		if (validmouse) {
			init = Input.mousePosition;
			rect.xMin = init.x;
			rect.yMin = Screen.height - init.y;
			validated = true;
			bldscript.selected = false;
			SelectedTroopsManager.bldsel = false;
			if (CameraMovement.hitDUDE2.collider.tag == "BDsup") {
				bldscript = CameraMovement.hitDUDE2.collider.transform.parent.GetComponent<SelectableBuilding> ();
				bldscript.selected = true;
				SelectedTroopsManager.bldsel = true;
				SelectedTroopsManager.selbld = bldscript.gameObject.transform.parent.gameObject;
			}
		}

		if (validated && ((Mathf.Abs (rect.xMin - Input.mousePosition.x) >= 8f) || (rect.yMin - (Screen.height - Input.mousePosition.y)) >= 8f)) { //DRAWING FOR REAL
			if (IsInvoking("turnoffdraw"))
				CancelInvoke("turnoffdraw");
			drawing = true;
			cursor.transform.localScale = new Vector3 (0f, 0f, 1f);
		}

		if (Input.GetMouseButtonUp (0) && validated) { //USER STOPS USING THE SELECTION BOX
			
			cursor.transform.localScale = new Vector3 (1f, 1f, 1f);
			validinit = false;
			validated = false;
			Invoke ("turnoffdraw", 0.1f);

			selectableScripts = GameObject.FindGameObjectsWithTag ("CHsup");

			for (int i = 0; i <= selectableScripts.Length - 1; i++) { //TRIGGER ALL SELECTABLE SCRIPTS TO CHECK IF THEY ARE AFFECTED
				selscript = selectableScripts [i].GetComponent<Selectable> ();
				selscript.dostuff ();
			}

			CancelInvoke ("aranj");
			setoffsets ();
			Invoke ("aranj", 0.06f);
		}
	}

	public void setoffsets() {
		if (formscript.LX.Count >= 1 && formscript.LZ.Count >= 1) { //THERE WAS ENOUGH ONLY ONE COUNT CHECK, BUT I DID 2 FOR A MORE SUGGESTIVE IF
			formscript.LX.Sort ();
			formscript.LZ.Sort ();
			for (int i = 1; i <= formation.transform.childCount - 1; i++) {
				troopscript = formation.transform.GetChild (i).GetComponent<ChangelingTroop> ();
				troopscript.offsetX = (formscript.LX [formscript.LX.Count - 1] + formscript.LX [0]) / 2 - troopscript.gameObject.GetComponentInChildren<Selectable> ().origX;
				troopscript.offsetZ = (formscript.LZ [formscript.LZ.Count - 1] + formscript.LZ [0]) / 2 - troopscript.gameObject.GetComponentInChildren<Selectable> ().origZ;
			}
		}
	}

	void turnoffdraw() {
		drawing = false;
	}

	void OnGUI() {
		if (validated) {
			rect.xMax = Input.mousePosition.x;
			rect.yMax = Screen.height - Input.mousePosition.y;

			if (rect.yMax < Screen.height * (1-0.97251f))
				rect.yMax = Screen.height * (1-0.97251f);
			
			if (rect.yMax > Screen.height * (1-0.23044f))
				rect.yMax = Screen.height * (1-0.23044f);
		
			if (drawing)
				GUI.DrawTexture (rect, texture);
		}
	}
}
