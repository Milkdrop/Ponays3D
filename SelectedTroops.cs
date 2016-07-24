using UnityEngine;
using System.Collections;

public class SelectedTroops : MonoBehaviour {

	//THIS SCRIPT IS STORING THE TROOP SELECTION NUMBER
	public static int TotalNB;
	public static int ChangelingsNB;
	public static int PAANB;

	public int totaldebug;
	public int changelingdebug;
	public int paadebug;

	void Start () {
		TotalNB = 0;
		ChangelingsNB = 0;
		PAANB = 0;
	}

	void Update () {
		totaldebug = TotalNB;
		changelingdebug = ChangelingsNB;
		paadebug = PAANB;
	}
}
