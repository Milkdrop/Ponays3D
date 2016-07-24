using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectedTroopsManager : MonoBehaviour {
	public bool bldseldebug;
	public static bool bldsel;
	public static GameObject selbld;
	public Building bldscript;
	public SelectionLines selectionlines;
	public bool spTownCenter;
	public SpriteState TownCenter;
	public bool spPoniesAtArms;
	public SpriteState PoniesAtArms;
	public bool spChangeling;
	public SpriteState Changeling;
	
	public GameObject moreobject;
	private Text more;
	public Text troopname;
	public GUIlifeCount lifecounttext;

	public static int number;
	public bool dirty;
	private ChangelingTroop objscript;
	private Selectable selscript;
	private TroopButton buttonscript;

	private GameObject[] troops;
	private GameObject[] avatars;
	public GameObject TroopAvatar;
	public GameObject overlord;
	public float distance;

	public float offsetX;
	public float offsetY;

	void Start() {
		more = moreobject.GetComponentInChildren<Text> ();
		number = 0;
	}

	void Update() {
		//bldseldebug = bldsel;
	}

	public void TroopDeath() {
		CancelInvoke ("Arrange");
		Invoke ("Arrange", 0.1f);
	}

	void Spawn(int ind, float y) {
		Instantiate (TroopAvatar, new Vector3 (gameObject.transform.position.x + ((float)Screen.width / 840) * (distance + (ind%9) * distance), gameObject.transform.position.y - ((float)Screen.height / 473) * y, gameObject.transform.position.z), Quaternion.identity);
		avatars = GameObject.FindGameObjectsWithTag ("avatar");

		avatars [avatars.Length - 1].transform.SetParent (overlord.transform.parent);
		avatars [avatars.Length - 1].transform.localScale = new Vector3 (1, 1, 1);
		buttonscript = avatars [avatars.Length - 1].GetComponentInChildren<TroopButton> ();
		buttonscript.order = ind;
		objscript = troops [ind].GetComponent<ChangelingTroop> ();

		if (objscript.tip == "Villager") {
			buttonscript.gameObject.GetComponent<Button> ().spriteState = Changeling;
			buttonscript.gameObject.GetComponent<Image> ().sprite = Changeling.disabledSprite;
			goto nexte2;
		}
		if (objscript.tip == "Ponies-At-Arms") {
			buttonscript.gameObject.GetComponent<Button> ().spriteState = PoniesAtArms;
			buttonscript.gameObject.GetComponent<Image> ().sprite = PoniesAtArms.disabledSprite;
			goto nexte2;
		}

		nexte2:
		objscript.GUILbar = avatars [avatars.Length - 1].GetComponentInChildren<LifeBar> ();
		objscript.GUILbar.reflectlief = troops [ind].GetComponent<ChangelingTroop> ();

		nexte3:;
	}

	public void Arrange() {
		troops = GameObject.FindGameObjectsWithTag ("troopsel");

		if (dirty && avatars != null) {
			for (int i = 1; i <= avatars.Length - 1; i++)
				Destroy (avatars [i]);
		}
		more.text = "";

		if (troops.Length == 1) { //Only one troop => details
			Instantiate (TroopAvatar, new Vector3 (gameObject.transform.position.x + ((float)Screen.width / 840) * (distance + offsetX), gameObject.transform.position.y - ((float)Screen.height / 473) * offsetY, gameObject.transform.position.z), Quaternion.identity);
			avatars = GameObject.FindGameObjectsWithTag ("avatar");

			avatars [avatars.Length - 1].transform.SetParent (overlord.transform.parent);
			avatars [avatars.Length - 1].transform.localScale = new Vector3 (1, 1, 1);

			buttonscript = avatars [avatars.Length - 1].GetComponentInChildren<TroopButton> ();
			buttonscript.order = 0;

			objscript = troops [troops.Length - 1].GetComponent<ChangelingTroop> ();

			if (objscript.specie == 1) {
				objscript.FoodCountGUI = avatars [avatars.Length - 1].transform.GetChild (2).gameObject.GetComponentInChildren<Text> ();
				avatars [avatars.Length - 1].transform.GetChild (2).gameObject.GetComponentInChildren<Text> ().text = objscript.food + "";
				avatars [avatars.Length - 1].transform.GetChild (2).gameObject.GetComponentInChildren<Image> ().transform.localScale = new Vector2 (0.08f, 0.08f);
			}

			if (objscript.tip == "Villager") {
				buttonscript.gameObject.GetComponent<Button> ().spriteState = Changeling;
				buttonscript.gameObject.GetComponent<Image> ().sprite = Changeling.disabledSprite;
				goto nexte;
			}
			if (objscript.tip == "Ponies-At-Arms") {
				buttonscript.gameObject.GetComponent<Button> ().spriteState = PoniesAtArms;
				buttonscript.gameObject.GetComponent<Image> ().sprite = PoniesAtArms.disabledSprite;
				goto nexte;
			}

			nexte:
			objscript.GUILbar = avatars [avatars.Length - 1].GetComponentInChildren<LifeBar> ();
			objscript.GUILbar.reflectlief = troops [troops.Length - 1].GetComponent<ChangelingTroop> ();
			lifecounttext = avatars [avatars.Length - 1].GetComponentInChildren<GUIlifeCount> ();
			troopname = avatars [avatars.Length - 1].transform.GetChild(1).GetComponent<Text> ();
			troopname.transform.SetParent (avatars [avatars.Length - 1].transform.GetChild (0).transform.parent);
			lifecounttext.transform.SetParent (avatars [avatars.Length - 1].transform.GetChild (0).transform.parent);

			lifecounttext.lifeorigin = troops [troops.Length - 1].GetComponentInChildren<LifeBar> ();
			lifecounttext.begin ();
			troopname.text = objscript.tip;
		} else {
			if (troops.Length > 27) {
				for (int ind = 0; ind <= 8; ind++)
					Spawn (ind, 0);
				for (int ind = 9; ind <= 17; ind++)
					Spawn (ind, 30);
				for (int ind = 18; ind <= 25; ind++)
					Spawn (ind, 60);

				if (troops.Length == 27)
					Spawn (26, 60);
				else {
					more.text = "+" + (troops.Length - 26) + '\n' + "More";

					moreobject.transform.position = new Vector3 (gameObject.transform.position.x + ((float)Screen.width / 840) * (distance + (26 % 9) * distance), gameObject.transform.position.y - ((float)Screen.height / 473) * 60, gameObject.transform.position.z);
					moreobject.transform.SetParent (overlord.transform.parent);
				}
				goto ende;
			} else if (troops.Length >= 19) {
			
				for (int ind = 0; ind <= 8; ind++)
					Spawn (ind, 0);
				for (int ind = 9; ind <= 17; ind++)
					Spawn (ind, 30);
				for (int ind = 18; ind <= troops.Length - 1; ind++)
					Spawn (ind, 60);

				goto ende;
			}

			if (troops.Length >= 10) {
			
				for (int ind = 0; ind <= 8; ind++)
					Spawn (ind, 0);
				for (int ind = 9; ind <= troops.Length - 1; ind++)
					Spawn (ind, 30);
				goto ende;
			}

			for (int ind = 0; ind <= troops.Length - 1; ind++) //only one Row
			Spawn (ind, 0);
		}
		ende:;
		dirty = true;
	}

	public void Chosen() {
		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.LeftShift))
			troops [number].GetComponentInChildren<Selectable> ().Deselect ();
		else {
			for (int i = 0; i < number; i++)
				troops [i].GetComponentInChildren<Selectable> ().Deselect ();
		
			for (int i = number + 1; i <= troops.Length - 1; i++)
				troops [i].GetComponentInChildren<Selectable> ().Deselect ();
		}
		selectionlines.setoffsets ();
		Arrange ();
	}
}
