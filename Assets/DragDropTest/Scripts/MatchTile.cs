using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MatchTile : MonoBehaviour {

	[SerializeField] Sprite mTileImageType_1;												//	Sprite type 1
	[SerializeField] Sprite mTileImageType_2;												//	Sprite type 2
	[SerializeField] GameObject[] mTileHolderList;											//	Holding the array of Containers in which Tiles are to be placed.
	[SerializeField] GameObject mBreakSprite;												//	Break Animation Game Object
	int tileDestroyedCount;																	//	Maintaining the counter for each tile set destroyed

	void OnEnable(){
		DragDropHandler.OnDestroyTile += PerformDestroyTile;								//	Registering to evening 					
	}

	void OnDisable(){
		DragDropHandler.OnDestroyTile -= PerformDestroyTile;								//	Deregistering to event
	}

	#region Tile Generation
	void Start () {
		tileDestroyedCount = 0;
		GenerateTilesRandomly ();
	}

	void GenerateTilesRandomly (){
		for (int i = 0; i < 4; i++) {
			GameObject tileObject_1 = CreateTile (ObjectPooler.mPoolerScript.GetPooledObject(), i);
			GameObject tileObject_2 = CreateTile (ObjectPooler.mPoolerScript.GetPooledObject(), i+4);
			int alternateID = Random.Range (0, 2);
			if (alternateID == 0) {
				tileObject_1.name = "Tile_1";
				tileObject_2.name = "Tile_2";
				tileObject_1.GetComponentInChildren<Image> ().sprite = mTileImageType_1;
				tileObject_2.GetComponentInChildren<Image> ().sprite = mTileImageType_2;
			}else{
				tileObject_1.name = "Tile_2";
				tileObject_2.name = "Tile_1";
				tileObject_1.GetComponentInChildren<Image> ().sprite = mTileImageType_2;
				tileObject_2.GetComponentInChildren<Image> ().sprite = mTileImageType_1;
			}
		}
	}

	GameObject CreateTile ( GameObject tileObject, int index ){
		tileObject.transform.SetParent (mTileHolderList[index].transform);
		RectTransform rectTransform 		= tileObject.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition3D	= Vector3.zero;
		rectTransform.sizeDelta				= Vector2.zero;
		tileObject.SetActive (true);
		return tileObject;
	}

	public void PerformDestroyTile(Vector3 pos, Transform parent){
		tileDestroyedCount++;																//	Updating the destroy counter 
		mBreakSprite.transform.SetParent (parent);											//	
		mBreakSprite.GetComponent<RectTransform> ().anchoredPosition3D	= pos;				//	Setting and placing the Tile Break sprite aniamtion on the last dreak image
		mBreakSprite.GetComponent<RectTransform> ().sizeDelta 			= Vector2.zero;		//
		mBreakSprite.SetActive (true);														// 

		if (tileDestroyedCount >= 4) {														//
			Invoke("DownloadFinalImageAssets",0.25f);										//	If all the Tile's are broken loading the Final image from the local Asset Bundle
		}
	}
	#endregion

	#region AssetBundle: Loading file/ assets from Asset Bundle
	void DownloadFinalImageAssets (){	
		string mCurrentPlatform;															//	Depending on the currening running platform defining the Asset bndle path
		#if UNITY_IPHONE || UNITY_IOS														//
		mCurrentPlatform = "iOS";															//
		#elif UNITY_ANDROID																	//
		mCurrentPlatform = "Android";														//
		#elif UNITY_STANDALONE_OSX															//
		mCurrentPlatform = "Mac";															//
		#elif UNITY_STANDALONE_WIN															//
		mCurrentPlatform = "Windows";														//
		#endif

		// Setting the Path depending on the current running platform
		AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile (Path.Combine (Application.streamingAssetsPath, "AssetBundles/" + mCurrentPlatform + "/imageasset.ui"));
		if (myLoadedAssetBundle == null) {
			Debug.Log ("Error: Failed to load AssetBundle!");
			return;
		}

		GameObject prefab				= myLoadedAssetBundle.LoadAsset<GameObject> ("EndImage");
		GameObject FinalObject			= (GameObject)Instantiate (prefab);
		FinalObject.transform.SetParent (GameObject.Find ("Canvas/Main_Panel").transform);
		RectTransform rectTransform		 = FinalObject.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition3D = Vector3.zero;
		rectTransform.sizeDelta			 = Vector2.zero;
	}
	#endregion
}