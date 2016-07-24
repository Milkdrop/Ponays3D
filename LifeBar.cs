using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {

	public GameObject avatar;
	public ChangelingTroop reflectlief;
	public int Behaviour;
	public int initlife;
	public int life;
	public GameObject bar;

	public void Start() {
		initlife = reflectlief.initlief;
		if (Behaviour == 2) {
			InvokeRepeating ("Upd8Life", 0f, 0.2f);
		}
	}

	public void Upd8Life() {
		life = reflectlief.life;
		if(life >= 0) bar.transform.localScale = new Vector3 ((float) life / initlife, 1f, 1f);
		else bar.transform.localScale = new Vector3 (0f, 1f, 1f);
	}
}