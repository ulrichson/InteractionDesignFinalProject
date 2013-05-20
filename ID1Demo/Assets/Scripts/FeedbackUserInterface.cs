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
	public bool drawGui = true;
	
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
		
		// WORKAROUND: PointCloudBehaviour always enables the renderes when image targets are detected
		SetRenderComponents();
	}
	
	void OnGUI() {
		if (drawGui) {
			// Draw left viewport
			GUILayout.BeginArea(new Rect(0, Screen.height - buttonSize * 2, Screen.width * UserInterface.leftViewPort.width, buttonSize * 2));
			DrawControls();
			GUILayout.EndArea();
			
			// Draw right viewport
			GUILayout.BeginArea(new Rect(UserInterface.rightViewPort.x * Screen.width, Screen.height - buttonSize * 2, Screen.width * UserInterface.rightViewPort.width, buttonSize * 2));
			DrawControls();
			GUILayout.EndArea();
			
			if (showToastMessage) {
				ShowToast("Thanks for your feedback!");
			}	
		}
	}
	
	void DrawControls() {
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		if (GUILayout.Button(textureThumbsUp, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)) || GUILayout.Button(textureThumbsDown, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			showToastMessage = true;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		if (GUILayout.Button(texturePrevious, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			SelectPreviousObject();
		}
		if (GUILayout.Button(textureNext, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			SelectNextObject();	
		}
		GUILayout.EndVertical();
		
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
			// Renderer rc = objectContainer.transform.GetChild(i).GetComponent<Renderer>();
			// rc.enabled = objIndex == i;
			objectContainer.transform.GetChild(i).gameObject.SetActive(objIndex == i);
		}
	} 
}
