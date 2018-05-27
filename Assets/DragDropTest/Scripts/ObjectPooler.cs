using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler mPoolerScript;							//	Making Pooler script as static for easy access.
	[SerializeField] GameObject poolableObject;							//	prefab/instance of the Object which needs to be pooled
	[SerializeField] int poolCounter = 4;								//	Max Number of object in the pool created at the start of the class
	bool willGrow = true;												//	Allow to increase pool size dynamically
	[SerializeField] List<GameObject> mPooledObjectsList;				//	List of the pool objects

	void Awake () {
		mPoolerScript = this;
		mPooledObjectsList = new List<GameObject> ();					//	Simply populating the pool to the default poolsize
		for (int i = 0; i < poolCounter; i++) {
			GameObject obj = (GameObject)Instantiate (poolableObject);	//
			obj.SetActive (false);										//	Populating the object pool
			mPooledObjectsList.Add (obj);								//	
		}
	}

	public GameObject GetPooledObject (){
		for (int i = 0; i < mPooledObjectsList.Count; i++) {			//	Going through pool to find available/free object for reusing
			if (!mPooledObjectsList[i].activeInHierarchy) {
				return mPooledObjectsList [i];							//	Sending the free poolable object
			}
		}

		if (willGrow) {													//	If no free poolable object is available, than checking if its allowed to create new poolable objects.
			GameObject obj = (GameObject)Instantiate (poolableObject);		
			mPooledObjectsList.Add (obj);
			return obj;													// Returning the newly created poolable object for use.
		}
		return null;													// returning null as no free poolable object is found and growing of pool was also not allowed.
	}
}
