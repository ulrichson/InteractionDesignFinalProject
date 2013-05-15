using UnityEngine;
using System.Collections;

public class FeedbackUserInterface : MonoBehaviour {
	
	public GameObject objectContainer;
	public Texture2D textureThumbsUp;
	public Texture2D textureThumbsDown;
	public Texture2D texturePrevious;
	public Texture2D textureNext;
	public GUIStyle toastStyle;
	public float toastDuration = 2.0f;
	public int buttonSize = 128;
	public int margin = 10;
	
	private bool showToastMessage = false;
	private float timeForToastMessage;
	private int objIndex = 0;

	// Use this for initialization
	void Start () {
		timeForToastMessage = toastDuration;
		SetRenderComponents();
	}
	
	// Update is called once per frame
	void Update () {
		if (showToastMessage) {
			timeForToastMessage -= Time.deltaTime;
			if (timeForToastMessage < 0.0f) {
				timeForToastMessage = toastDuration;
				showToastMessage = false;
			}
		}
	}
	
	void OnGUI() {
		// Draw left viewport
		GUILayout.BeginArea(new Rect(margin, Screen.height - buttonSize - margin, Screen.width * UserInterface.leftViewPort.width - margin, buttonSize + margin));
		DrawControls();
		GUILayout.EndArea();
		
		// Draw right viewport
		GUILayout.BeginArea(new Rect(UserInterface.rightViewPort.x * Screen.width + margin, Screen.height - buttonSize - margin, Screen.width * UserInterface.rightViewPort.width - margin, buttonSize + margin));
		DrawControls();
		GUILayout.EndArea();
		
		if (showToastMessage) {
			ShowToast("Thanks for your feedback!");
		}
	}
	
	void DrawControls() {
		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(textureThumbsUp, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)) || GUILayout.Button(textureThumbsDown, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			showToastMessage = true;
		}
		if (GUILayout.Button(texturePrevious, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			SelectPreviousObject();
		}
		if (GUILayout.Button(textureNext, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			SelectNextObject();	
		}
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
	} 
	
	void ShowToast(string msg) {
		// Draw left viewport
		GUILayout.BeginArea(new Rect(0, 0, Screen.width * UserInterface.leftViewPort.width, Screen.height));
		
	    GUILayout.FlexibleSpace();
	    GUILayout.BeginHorizontal();
	    GUILayout.FlexibleSpace();
	 
	    GUILayout.Label(msg, toastStyle);
	 
	    GUILayout.FlexibleSpace();
	    GUILayout.EndHorizontal();
	    GUILayout.FlexibleSpace();

		GUILayout.EndArea();
		
		// Draw right viewport
		GUILayout.BeginArea(new Rect(Screen.width * UserInterface.rightViewPort.x, 0, Screen.width * UserInterface.rightViewPort.width, Screen.height));
		
	    GUILayout.FlexibleSpace();
	    GUILayout.BeginHorizontal();
	    GUILayout.FlexibleSpace();
	 
	    GUILayout.Label(msg, toastStyle);
	 
	    GUILayout.FlexibleSpace();
	    GUILayout.EndHorizontal();
	    GUILayout.FlexibleSpace();
		
		GUILayout.EndArea();
	}
	
	void SelectNextObject() {
		objIndex++;
		if (objIndex >= objectContainer.transform.childCount) {
			objIndex = 0;
		}
		SetRenderComponents();
	}
	
	void SelectPreviousObject() {
		objIndex--;
		if (objIndex < 0) {
			objIndex = objectContainer.transform.childCount - 1;
		}
		SetRenderComponents();
	}
	
	void SetRenderComponents() {
		for (int i = 0; i < objectContainer.transform.childCount; i++) {
			Renderer rc = objectContainer.transform.GetChild(i).GetComponent<Renderer>();
			rc.enabled = objIndex == i;
		}
	} 
	
	void OnPointCloudStateChange ()
	{
		SetRenderComponents();
	}
}
