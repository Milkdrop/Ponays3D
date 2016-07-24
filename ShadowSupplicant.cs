using UnityEngine;
using System.Collections;

public class ShadowSupplicant : MonoBehaviour {

	public SpriteRenderer sp;
	void Start () {
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}
}
