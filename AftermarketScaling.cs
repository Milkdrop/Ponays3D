using UnityEngine;
using System.Collections;

public class AftermarketScaling : MonoBehaviour {

	void Start () {
		gameObject.transform.localScale = new Vector2 ((float)Screen.width / 840, (float)Screen.height / 473);
	}
}
