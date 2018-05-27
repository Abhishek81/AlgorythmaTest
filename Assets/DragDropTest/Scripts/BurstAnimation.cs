using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstAnimation : MonoBehaviour {

	public void OnAnimationComplete (){			//	Disabling the Burst Animation Game Object on Animation Event trigger
		this.gameObject.SetActive (false);
	}
}
