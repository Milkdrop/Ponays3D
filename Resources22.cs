using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resources22 : MonoBehaviour {

	public int food;

	public Text foodtxt;

	void Start() {
		food = 0;
	}

	public void upd8food() {
		foodtxt.text = food + "";
	}
}
