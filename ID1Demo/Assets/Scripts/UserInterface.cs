using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

private enum mode {
		UNKOWN,
		SLAM,
		IMAGE_TRACKING,
		IMAGE_TRACKING_SLAM
	}
	
	mode current_mode;
	
	public Texture2D ui;
	private int arrowOffset;
	Rect initBoxRect;
	Rect initBoxTexCoords;
	Rect initArrowRect;
	Rect initArrowTexCoords;
	float scale = 1f;
	bool draw_init;
	
	const float buttonHeightNormalized = 0.1f;
	Rect guiArea;
	public float ButtonHeight {get; set;}
	
	private string modeButtonLabel;
	private string recordButtonLabel;
	
	bool showGUI = true;

	void Start()
	{	
		guiArea = new Rect(0, Screen.height * (1f - buttonHeightNormalized), Screen.width, Screen.height * buttonHeightNormalized);
		ButtonHeight = Screen.height * buttonHeightNormalized;
		
		scale = Mathf.Min(Screen.height, Screen.width)/768f;
		initBoxTexCoords = new Rect(1f/512f, (511f-289f)/512f, 429f/512f, 289f/512f);
		
		modeButtonLabel = "";
		recordButtonLabel = "Record";
		
		current_mode = mode.UNKOWN;
		
		OnPointCloudStateChanged();
		NextMode();
	}
	
	void Update() {
		
		int initBoxWidth = (int)(scale * 429);
		int initBoxHeight = (int)(scale * 289);
		initBoxRect = new Rect((Screen.width-initBoxWidth)/2, (Screen.height - initBoxHeight)/2, initBoxWidth, initBoxHeight);
		int initArrowWidth = (int)(30 * scale);
		int initArrowHeight = (int)(28 * scale);
		initArrowRect = new Rect((int)(initBoxRect.x + scale * 26f), initBoxRect.y + arrowOffset, initArrowWidth, initArrowHeight);
		initArrowTexCoords = new Rect(45f/512f, 1f - (349f+28f)/512f, 30f/512f, 28f/512f);
		guiArea = new Rect(0, Screen.height * (1f - buttonHeightNormalized), Screen.width, Screen.height * buttonHeightNormalized);
		ButtonHeight = Screen.height * buttonHeightNormalized;
		
		if (Input.touchCount > 0 && 
      		Input.GetTouch(0).phase == TouchPhase.Began &&
			Input.GetTouch(0).position.y > ButtonHeight)
		{
			if (current_mode == mode.SLAM)
			{
				pointcloud_state state = PointCloudBehaviour.State;
				if (state == pointcloud_state.POINTCLOUD_IDLE)	
				{
					PointCloudAdapter.pointcloud_start_slam();
				}
				else if (state == pointcloud_state.POINTCLOUD_INITIALIZING)
				{
					PointCloudAdapter.pointcloud_reset();
				}
			}
		}
	}
	
	void NextMode() {
		PointCloudAdapter.pointcloud_reset();
		switch(current_mode)
		{
		case mode.SLAM:
			current_mode = mode.IMAGE_TRACKING;
			modeButtonLabel = "Imaged Recognition";
			PointCloudAdapter.pointcloud_disable_map_expansion();
			PointCloudBehaviour.ActivateAllImageTargets();
			break;
		case mode.IMAGE_TRACKING:
			current_mode = mode.IMAGE_TRACKING_SLAM;
			modeButtonLabel = "SLAM from Image";
			PointCloudAdapter.pointcloud_enable_map_expansion();
			PointCloudBehaviour.ActivateAllImageTargets();
			break;
		case mode.IMAGE_TRACKING_SLAM:
		default:
			current_mode = mode.SLAM;
			modeButtonLabel = "SLAM";
			PointCloudBehaviour.DeactivateAllImageTargets();
			PointCloudAdapter.pointcloud_enable_map_expansion();
			break;
		}
		OnPointCloudStateChanged();
	}
	
	void OnPointCloudStateChanged() {
		pointcloud_state state = PointCloudBehaviour.State;
		draw_init = current_mode == mode.SLAM && state == pointcloud_state.POINTCLOUD_IDLE || state == pointcloud_state.POINTCLOUD_INITIALIZING;
		
		if (draw_init)
		{
			arrowOffset = (int)(89f * scale);
			
			if (state == pointcloud_state.POINTCLOUD_INITIALIZING)
			{
				arrowOffset = (int)((89f + 59f) * scale); 
			}
		}
	}
	
	void OnGUI()
	{		
		if (GUI.Button (new Rect (10, 10, 30, 20), "")) {
			showGUI = !showGUI;
		}
		
		if (showGUI) {
			GUILayout.BeginArea(guiArea);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button (recordButtonLabel, GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/3))) {
				if (recordButtonLabel == "Record")
				{
					Kamcord.StartRecording();
					recordButtonLabel = "Stop";
				}
				else
				{
					Kamcord.StopRecording();
					Kamcord.ShowView();
					recordButtonLabel = "Record";
				}
			}
			else if (GUILayout.Button("Reset", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/3)))
			{
				PointCloudBehaviour.Instance.Reset();
			}
			else if (GUILayout.Button(modeButtonLabel, GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/3)))
			{
				NextMode();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
				
			if(draw_init)
			{
				GUI.DrawTextureWithTexCoords(initBoxRect, ui, initBoxTexCoords);
				GUI.DrawTextureWithTexCoords(initArrowRect, ui, initArrowTexCoords);
			}
			
			GUI.Label (new Rect (10, 30, 200, 20), "Position: " + SceneTransform.Instance.position);
			GUI.Label (new Rect (10, 50, 200, 20), "Rotation: " + SceneTransform.Instance.rotation);
			GUI.Label (new Rect (10, 70, 200, 20), "Scale: " + SceneTransform.Instance.scale);
			
			// Wrap everything in the designated GUI Area
			GUILayout.BeginArea (new Rect (Screen.width - 300 - 10, 10, 300, 300));
			
			GUILayout.BeginVertical ();
			
			GUILayout.Label ("Position sensitivity: " + SceneTransform.Instance.positionSensitivity);
			SceneTransform.Instance.positionSensitivity = GUILayout.HorizontalSlider (SceneTransform.Instance.positionSensitivity, 1.0f, 50.0f);
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("p.x (+)")) {
				SceneTransform.Instance.position.x += SceneTransform.Instance.positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.x (-)")) {
				SceneTransform.Instance.position.x -= SceneTransform.Instance.positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.y (+)")) {
				SceneTransform.Instance.position.y += SceneTransform.Instance.positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.y (-)")) {
				SceneTransform.Instance.position.y -= SceneTransform.Instance.positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.z (+)")) {
				SceneTransform.Instance.position.z += SceneTransform.Instance.positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.z (-)")) {
				SceneTransform.Instance.position.z -= SceneTransform.Instance.positionSensitivity;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("r.x (+)")) {
				SceneTransform.Instance.rotation.x += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.x (-)")) {
				SceneTransform.Instance.rotation.x -= 1.0f;
			}
			if (GUILayout.RepeatButton ("r.y (+)")) {
				SceneTransform.Instance.rotation.y += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.y (-)")) {
				SceneTransform.Instance.rotation.y -= 1.0f;
			}
			if (GUILayout.RepeatButton ("r.z (+)")) {
				SceneTransform.Instance.rotation.z += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.z (-)")) {
				SceneTransform.Instance.rotation.z -= 1.0f;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("s (+)")) {
				SceneTransform.Instance.scale += 0.01f;
			}
			if (GUILayout.RepeatButton ("s (-)")) {
				SceneTransform.Instance.scale -= 0.01f;
			}
			GUILayout.EndHorizontal ();
			
			if (GUILayout.Button ("Reset")) {
				SceneTransform.Instance.position = Vector3.zero;
				SceneTransform.Instance.rotation = Vector3.zero;
				SceneTransform.Instance.scale = 1.0f;
				SceneTransform.Instance.positionSensitivity = 10.0f;
			}
			
			GUILayout.Label("Sun Light");
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("rotation (+)")) {
				SunLight.Instance.sceneOrientation += 1.0f;
			}
			if (GUILayout.RepeatButton ("rotation (-)")) {
				SunLight.Instance.sceneOrientation -= 1.0f;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.Label("3D Stero Vision");
			
			if (GUILayout.Button("Enable 3D Stero Vision")) {
				AnaglyphizerC.Instance.enabled = true;	
			}
			
			GUILayout.EndVertical ();
			
			GUILayout.EndArea ();	
		}
	}
}
