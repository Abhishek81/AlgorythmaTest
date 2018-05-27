using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler{

	public static GameObject mDragGameObject;								//	Holds current Dragged Tilled 
	static GameObject mDragPlaceholder;										//	Placeholder tile object
	Vector3 mStartPos;	

	public delegate void DestroyTile( Vector3 pos, Transform parent);
	public static event DestroyTile OnDestroyTile;

	#region OnDragBegin event is handled below
	public void OnBeginDrag(PointerEventData eventData){
		mDragPlaceholder = ObjectPooler.mPoolerScript.GetPooledObject();	// Requesting for new pooled object
		mDragPlaceholder.name = this.gameObject.name+"_Placeholder";		//	Assigning proper name to the new pooled object

		Image img = mDragPlaceholder.GetComponentInChildren<Image> ();
		img.sprite = this.GetComponentInChildren<Image>().sprite;			// Setting current picked image sprite to the pooled object
		img.color = new Color (1, 1, 1, 0.5f);								// Making placeholder image  semi transparent.

		mDragPlaceholder.transform.SetParent (this.transform.parent);		//	Placing the placeholder tile in the same slot of the dragged tile.
		RectTransform rt = mDragPlaceholder.GetComponent<RectTransform> ();	//	
		rt.sizeDelta = Vector2.zero;										//	Setting size and postion of the cell
		rt.anchoredPosition3D = Vector3.zero;								//	
		mDragPlaceholder.SetActive (true);									//	Making it visible/active 

		mDragGameObject = this.gameObject;									//	Setting the DrabObject
		mStartPos = this.transform.position;								//	Backing up the original postion so it can be reverted back if matching tile not found

		GetComponent<CanvasGroup> ().blocksRaycasts = false;				//	Disabling raycast event for the tile allowing ray to pass through the tile and hit any of the background tile.
	}
	#endregion

	#region Draging of selected tile handled here
	public void OnDrag(PointerEventData eventData)
	{
		this.transform.position = Input.mousePosition;						//	Update position for the current dragged tile wrt mouse postion 
	}
	#endregion

	#region If user releases the tile on some other tile or on free area
	public void OnEndDrag(PointerEventData eventData)
	{
		mDragGameObject = null;												//	Freeing the currently dragged tile 
		GetComponent<CanvasGroup> ().blocksRaycasts = true;					//	ReEnabling the raycast event for the tile 
		this.transform.position = mStartPos;								//	setting the tile back to its original location
		DragDropHandler.mDragPlaceholder.transform.SetParent (null);		//	unattaching the placeholder image from the parent object
		mDragPlaceholder.SetActive (false);									//	disabling the placeholder Object so that it can reused by the ObjectPooler class
		mDragPlaceholder = null;											//	Freeing the placeholder instace for the next dragable tile.
	}
	#endregion

	#region If Another Tile receives drop event for the Dragged tile
	public void OnDrop (PointerEventData eventData)
	{
		if (this.name == DragDropHandler.mDragGameObject.name) {			//	If the current Selected Tile object same as dragged Tile 
			DragDropHandler.mDragGameObject.SetActive(false);				//	
			DragDropHandler.mDragPlaceholder.SetActive (false);				//	Hidding & Freeing the Drag Tile and Placeholder object & making it reusable for pooling
			DragDropHandler.mDragGameObject = null;							//	
			DragDropHandler.mDragPlaceholder = null;						//	
			this.gameObject.SetActive (false);								//	Hidding the Current Selected tile Object and putting it back into pool.

			if (OnDestroyTile != null) {
				RectTransform trans = this.gameObject.GetComponent<RectTransform> ();
				OnDestroyTile (trans.anchoredPosition3D, this.transform.parent );
			}
		}
	}
	#endregion
}
