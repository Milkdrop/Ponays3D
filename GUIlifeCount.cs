using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIlifeCount : MonoBehaviour {

	private Text thistxt;
	public LifeBar lifeorigin;

	public void begin() {
		thistxt = gameObject.GetComponent<Text> ();
		InvokeRepeating("upd8life", 0f, 0.2f);
	}

	void upd8life() {
		thistxt.text = lifeorigin.life + "/" + lifeorigin.initlife;
	}
}
