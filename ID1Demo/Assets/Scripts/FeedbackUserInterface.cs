using UnityEngine;
using System.Collections;

public class FeedbackUserInterface : MonoBehaviour
{
	
	public GameObject objectContainer;
	public Texture2D textureThumbsUp;
	public Texture2D textureThumbsDown;
	public Texture2D texturePrevious;
	public Texture2D textureNext;
	public Texture2D textureNormal;
	public Texture2D textureHover;
	public Texture2D textureActive;
	public GUIStyle toastStyle;
	public float toastDuration = 2.0f;
	public int buttonSize = 128;
	public bool drawGui = true;
	private bool showToastMessage = false;
	private float timeForToastMessage;
	private int objIndex = 0;
	private GUIStyle buttonStyle;
	
	// Use this for initialization
	void Start ()
	{
		timeForToastMessage = toastDuration;
		SetRenderComponents ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (showToastMessage) {
			timeForToastMessage -= Time.deltaTime;
			if (timeForToastMessage < 0.0f) {
				timeForToastMessage = toastDuration;
				showToastMessage = false;
			}
		}
		
		// WORKAROUND: PointCloudBehaviour always enables the renderes when image targets are detected
		SetRenderComponents ();
	}
	
	void OnGUI ()
	{
		if (buttonStyle == null)
			buttonStyle = new GUIStyle(GUI.skin.button);
		
		buttonStyle.normal.background = textureNormal;
		buttonStyle.hover.background = textureHover;
		buttonStyle.active.background = textureActive;
		
		if (drawGui) {
			
//			if (PointCloudBehaviour.Instance.useSplitView) {
//				// Draw left viewport
//				GUILayout.BeginArea (new Rect (0, Screen.height - buttonSize * 2, Screen.width * UserInterface.leftViewPort.width, buttonSize * 2));
//				DrawControls ();
//				GUILayout.EndArea ();
//				
//				// Draw right viewport
//				GUILayout.BeginArea (new Rect (UserInterface.rightViewPort.x * Screen.width, Screen.height - buttonSize * 2, Screen.width * UserInterface.rightViewPort.width, buttonSize * 2));
//				DrawControls ();
//				GUILayout.EndArea ();
//			} else {
				GUILayout.BeginArea (new Rect (0, Screen.height - buttonSize * 2, Screen.width, buttonSize * 2));
				DrawControls ();
				GUILayout.EndArea ();
//			}
			
			if (showToastMessage) {
				ShowToast ("Thanks for your feedback!");
			}
		}
	}
	
	void DrawControls ()
	{
		GUILayout.BeginHorizontal ();
		
		GUILayout.BeginVertical ();
		if (GUILayout.Button (textureThumbsUp, buttonStyle, GUILayout.Width (buttonSize), GUILayout.Height (buttonSize)) || GUILayout.Button (textureThumbsDown, buttonStyle, GUILayout.Width (buttonSize), GUILayout.Height (buttonSize))) {
			showToastMessage = true;
		}
		GUILayout.EndVertical ();
		GUILayout.FlexibleSpace ();
		GUILayout.BeginVertical ();
		if (GUILayout.Button (texturePrevious, buttonStyle, GUILayout.Width (buttonSize), GUILayout.Height (buttonSize))) {
			SelectPreviousObject ();
		}
		if (GUILayout.Button (textureNext, buttonStyle, GUILayout.Width (buttonSize), GUILayout.Height (buttonSize))) {
			SelectNextObject ();	
		}
		GUILayout.EndVertical ();
		
		GUILayout.EndHorizontal ();
	}
	
	void ShowToast (string msg)
	{
		if (PointCloudBehaviour.Instance.useSplitView) {
			// Draw left viewport
			GUILayout.BeginArea (new Rect (0, 0, Screen.width * UserInterface.leftViewPort.width, Screen.height));
			
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		 
			GUILayout.Label (msg, toastStyle);
		 
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
	
			GUILayout.EndArea ();
			
			// Draw right viewport
			GUILayout.BeginArea (new Rect (Screen.width * UserInterface.rightViewPort.x, 0, Screen.width * UserInterface.rightViewPort.width, Screen.height));
			
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		 
			GUILayout.Label (msg, toastStyle);
		 
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
			
			GUILayout.EndArea ();
		} else {
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
			
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		 
			GUILayout.Label (msg, toastStyle);
		 
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
	
			GUILayout.EndArea ();
		}
	}
	
	void SelectNextObject ()
	{
		objIndex++;
		if (objIndex >= objectContainer.transform.childCount) {
			objIndex = 0;
		}
		SetRenderComponents ();
	}
	
	void SelectPreviousObject ()
	{
		objIndex--;
		if (objIndex < 0) {
			objIndex = objectContainer.transform.childCount - 1;
		}
		SetRenderComponents ();
	}
	
	void SetRenderComponents ()
	{
		for (int i = 0; i < objectContainer.transform.childCount; i++) {
			// Renderer rc = objectContainer.transform.GetChild(i).GetComponent<Renderer>();
			// rc.enabled = objIndex == i;
			objectContainer.transform.GetChild (i).gameObject.SetActive (objIndex == i);
		}
	} 
}
