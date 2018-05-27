using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	
	[SerializeField] Color mChangeToColor;										//	Defining Color which needs to be changed on the Tile
	Animator mAnim;																//	Animator Controller for the player Object

	void Start () {
		DontDestroyOnLoad (this.gameObject);									//	Making sure that we dont destroy the Player gameObject when the scene is changed
		mAnim = GetComponent<Animator> ();										//	Fetching animator Object attached to the player object.
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {										//	Handling OnMouseLeft click event or on simple touch event on any touch devices.
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 70)) {
				if (hit.collider.name == "WK_HeavyIntantry") {					//	Trying to detect if the click was on 3D model(Player) or not.
					mAnim.SetInteger ("AnimationID", Random.Range(1,4));		//	If the hit is detected as player, than Calling random animation from either on the animations(Attack, Run, Walk).
				}
			}
		}
	}

	public void ResetAnimationID (int newID){									//	This is an event triggered from respective Animation clip's allowing player ender back to ideal animation.
		mAnim.SetInteger ("AnimationID", newID);								//	Resetting AnimationID variable to zero. causing the Animation to revert back to ideal Aniamtion once completing current animation.
	}

	#region Handling Tile Aniamtion 
	public void StartTileAniamtion (){
		GameObject[] tileArray = GameObject.FindGameObjectsWithTag ("Tiles");	//	Getting All the GameObject whose Tag is assigned as Tiles
		int waitTime = 0;
		foreach (GameObject tileObject in tileArray) {
			tileObject.GetComponent<Button> ().enabled = false;					//	Disabling all the buttons on the tile to avoide double tapping
			StartCoroutine(ChangeTileColor(tileObject, waitTime++));			//	Firing a simple Corouting to change the color of the tile after Mentioned delay.
		}
		Invoke ("ChangeScene",waitTime);										//	Lastly calling function to switch to Scene 2 after all the colors are changed to the defined color.
	}

	IEnumerator ChangeTileColor ( GameObject tile, float waitTime ){
		yield return new WaitForSeconds (waitTime);								//	Simple wating for X time, before assigning the new tile color.
		tile.GetComponent<RawImage> ().color = mChangeToColor;					//	Setting color of the current tile to the defined color.
	}
	#endregion

	void ChangeScene (){
		SceneManager.LoadScene("Scene2", LoadSceneMode.Single);					//	Finally loading the New Scene.
		this.transform.position = new Vector3(0, 0, 1000f);						//	Moving the player Character to the bottom Left of screen
	}
}
