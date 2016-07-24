using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selectable : MonoBehaviour {

	public ChangelingTroop objscript;
	public float origX;
	public float origZ;
	public AudioSource CHselected;
	public GameObject lifebarr;
	public LifeBar Lbar;
	public Formation formscript;
	public ChangelingTroop motherscript;
	public bool inside;
	public GameObject basicSupplicant;
	public GameObject formationSupplicant;
	public GameObject mothership;
	public bool selected;
	public GameObject selection;
	public BoxCollider OurCollider;
	public MeshCollider OurColliderMesh;
	public Vector2 CHUL;
	public Vector2 CHDR;
	public BoxCollider col;
	public AudioSource CHdeath;
	public SelectedTroopsManager SelTroMan;
	public GameObject lifebar;

	void Start() { //SET STUFF
		objscript = gameObject.GetComponentInParent<ChangelingTroop>();
		if (!objscript.building) {
			OurCollider = gameObject.GetComponent<BoxCollider> ();
			motherscript = gameObject.GetComponentInParent<ChangelingTroop> ();
		} else {
			OurColliderMesh = gameObject.GetComponent<MeshCollider> ();
		}
	}
		
	public void Select() {
		if (!objscript.building) {
			origX = transform.position.x;
			origZ = transform.position.z;
			formscript.LX.Add (origX);
			formscript.LZ.Add (origZ);
		}

		CHselected.Play ();

		//LIFE BAR OPTIMIZATIONS BEGIN
		lifebarr.SetActive (true);
		Lbar.Start ();
		Lbar.Upd8Life ();
		//LIFE BAR OPTIMIZATIONS END

		//INFORM SELECTEDTROOPS SCRIPT BEGIN
		SelectedTroops.TotalNB++;
		if (objscript.tip == "Villager") SelectedTroops.ChangelingsNB++;
		if (objscript.tip == "Ponies-At-Arms") SelectedTroops.PAANB++;
		//INFORM SELECTEDTROOPS SCRIPT END
		
		mothership.transform.tag = "troopsel"; //SET SEL TAG
		mothership.transform.SetParent (formationSupplicant.transform.parent);
		selection.gameObject.SetActive (true);
		selected = true;
	}

	public void Deselect() {
		lifebarr.SetActive (false); //LIFE BAR OPTIMIZATION

		//REMOVE COORDINATES FROM X/Z 
		if (!objscript.building) {
			formscript.LX.Remove (origX);
			formscript.LZ.Remove (origZ);
		}
		//INFORM SELECTEDTROOPS SCRIPT

		SelectedTroops.TotalNB--;
		if (objscript.tip == "Villager") SelectedTroops.ChangelingsNB--;
		if (objscript.tip == "Ponies-At-Arms") SelectedTroops.PAANB--;

		mothership.transform.tag = "troopunsel"; //SET UNSEL TAG
		mothership.transform.SetParent(basicSupplicant.transform.parent);
		selection.gameObject.SetActive (false);
		selected = false;
	}

	public void death() {
		col.enabled = false;
		if (selected) {
			selected = false;
			SelectedTroops.TotalNB--;
			if (objscript.tip == "Villager")
				SelectedTroops.ChangelingsNB--;
			if (objscript.tip == "Ponies-At-Arms")
				SelectedTroops.PAANB--;
		}
		selection.SetActive (false);
		lifebar.SetActive (false);

		CHdeath.pitch = Random.Range (0.8f, 1.2f);
		CHdeath.Play ();

		mothership.transform.tag = "troopunsel";
		mothership.transform.SetParent(basicSupplicant.gameObject.transform.parent);

		Invoke ("Destroyed", 15f);
		SelTroMan.TroopDeath ();

		gameObject.SetActive (false);
	}

	void Destroyed() {
		Destroy (mothership);
	}

	public void dostuff() {
		
		//USELESS SHENANIGANS BEGIN
		if (!objscript.building) {
			CHUL = Camera.main.WorldToScreenPoint (new Vector3 (OurCollider.bounds.center.x - OurCollider.bounds.extents.x, OurCollider.bounds.center.y + OurCollider.bounds.extents.y, transform.position.z));
			CHDR = Camera.main.WorldToScreenPoint (new Vector3 (OurCollider.bounds.center.x + OurCollider.bounds.extents.x, OurCollider.bounds.center.y - OurCollider.bounds.extents.y, transform.position.z));
		} else {
			CHUL = Camera.main.WorldToScreenPoint (new Vector3 (OurColliderMesh.bounds.center.x - OurColliderMesh.bounds.extents.x, OurColliderMesh.bounds.center.y + OurColliderMesh.bounds.extents.y, transform.position.z));
			CHDR = Camera.main.WorldToScreenPoint (new Vector3 (OurColliderMesh.bounds.center.x + OurColliderMesh.bounds.extents.x, OurColliderMesh.bounds.center.y - OurColliderMesh.bounds.extents.y, transform.position.z));
		}
		if (CHDR.x > 0 || CHUL.x < Screen.width && CHDR.y < Screen.height * 0.97251f && CHUL.y > Screen.height * 0.23044f) {
			if ((((SelectionLines.init.x < CHDR.x && SelectionLines.init.x > CHUL.x) || (Input.mousePosition.x < CHDR.x && Input.mousePosition.x > CHUL.x)) || (SelectionLines.init.x > CHDR.x && Input.mousePosition.x < CHDR.x) || (SelectionLines.init.x < CHUL.x && Input.mousePosition.x > CHUL.x))
			    &&
			    (((SelectionLines.init.y > CHDR.y && SelectionLines.init.y < CHUL.y) || (Input.mousePosition.y > CHDR.y && Input.mousePosition.y < CHUL.y) || (SelectionLines.init.y < CHDR.y && Input.mousePosition.y > CHDR.y) || (SelectionLines.init.y > CHUL.y && Input.mousePosition.y < CHUL.y))))
				inside = true;
			else
				inside = false;
		//USELESS SHENANIGANS END

			if (inside) {
				if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.LeftShift)) {
					if (selected) {
						if (SelectionLines.drawing == false)
							Deselect ();
					} else {
						Select ();
					}
				} else if (!selected) {
					Select ();
				}
			} else if (selected && !(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.LeftControl)))
				Deselect ();
		} else
			if (selected) Deselect ();
	}
}
