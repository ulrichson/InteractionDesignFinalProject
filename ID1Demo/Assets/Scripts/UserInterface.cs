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
	
	public Texture2D pointCloudLogo;
	public Texture2D ui;
	
	public RenderTexture renderTextureLeftEye;
	public RenderTexture renderTextureRightEye;
	
	public bool useHasbroMy3D = true;
	
	Rect logoRect;
	Rect headerRect;
	Rect headerTexCoords;
	Rect headerBackgroundRect;
	Rect headerBackgroundTexCoords;
	private int arrowOffset;
	Rect initBoxRect;
	Rect initBoxTexCoords;
	Rect initArrowRect;
	Rect initArrowTexCoords;
	float scale = 1f;
	float highres_scale = 0.5f;
	bool draw_init;
	
	const float buttonHeightNormalized = 0.1f;
	Rect guiArea;
	public float ButtonHeight {get; set;}
	
	private string modeButtonLabel;
	private string recordButtonLabel;
	private bool toggle3dStereoVision = true;
	
	public static Rect leftViewPort = new Rect (0.0f, 0.0f, 0.4685f, 1.0f);
	public static Rect rightViewPort = new Rect (0.529f, 0.0f, 0.4685f, 1.0f);
	
	float screenRatio = 0.0f;
	
	bool showGUI = true;
	
	void Start()
	{	
		guiArea = new Rect(0, Screen.height * (1f - buttonHeightNormalized), Screen.width, Screen.height * buttonHeightNormalized);
		ButtonHeight = Screen.height * buttonHeightNormalized;
		
		scale = Mathf.Min(Screen.height, Screen.width)/768f;
		highres_scale = 0.5f * scale;
		
		headerBackgroundTexCoords = new Rect(1f/512f, 1f-(332f+60f)/512f, 24f/512f, 60f/512f);
		headerTexCoords = new Rect(1f/512f, 1f-(289+37)/512f, 282f/512f, 36f/512f);
		initBoxTexCoords = new Rect(1f/512f, (511f-289f)/512f, 429f/512f, 289f/512f);
		
		modeButtonLabel = "";
		recordButtonLabel = "Record";
		
		current_mode = mode.UNKOWN;
		
		OnPointCloudStateChanged();
		NextMode();
	}
	
	void Update() {
		
		headerBackgroundRect = new Rect(0, 0, Screen.width, scale * 60f);
		headerRect = new Rect((int)((Screen.width-scale * 283f)/2f), (int)(scale * (60-36)/2), (int)(scale * 283f), (int)(scale * 36f));
		int initBoxWidth = (int)(scale * 429);
		int initBoxHeight = (int)(scale * 289);
		initBoxRect = new Rect((Screen.width-initBoxWidth)/2, (Screen.height - initBoxHeight)/2, initBoxWidth, initBoxHeight);
		int initArrowWidth = (int)(30 * scale);
		int initArrowHeight = (int)(28 * scale);
		initArrowRect = new Rect((int)(initBoxRect.x + scale * 26f), initBoxRect.y + arrowOffset, initArrowWidth, initArrowHeight);
		initArrowTexCoords = new Rect(45f/512f, 1f - (349f+28f)/512f, 30f/512f, 28f/512f);
		int logoWidth = (int)(pointCloudLogo.width * highres_scale);
		int logoHeight = (int)(pointCloudLogo.height * highres_scale);
		logoRect = new Rect(scale * 4f, Screen.height - ButtonHeight - logoHeight - 4f, logoWidth, logoHeight);
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
		if (screenRatio == 0.0f) {
			screenRatio = (float)Screen.width / (float)Screen.height;
		}
		
		if (toggle3dStereoVision) {
			if (Application.isEditor) {
				GUI.DrawTextureWithTexCoords(new Rect(0, 0, Screen.width / 2, Screen.height), renderTextureLeftEye, new Rect(0, 0, 0.5f * screenRatio, 1.0f));
				GUI.DrawTextureWithTexCoords(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), renderTextureRightEye, new Rect(0, 0, 0.5f * screenRatio, 1.0f));
			} else {
				Rect left = useHasbroMy3D ? new Rect(0, 0, Screen.width * leftViewPort.width, Screen.height) : new Rect(0, 0, Screen.width / 2, Screen.height);
				Rect right = useHasbroMy3D ? new Rect(Screen.width * rightViewPort.x, 0,Screen.width * rightViewPort.width, Screen.height) : new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height);
				GUI.DrawTextureWithTexCoords(left, renderTextureLeftEye, new Rect(0, 0, 0.5f, 1.0f));
				GUI.DrawTextureWithTexCoords(right, renderTextureRightEye, new Rect(0, 0, 0.5f, 1.0f));
			}	
		} else {
			GUI.DrawTexture(new Rect(0 , 0, Screen.width, Screen.height), renderTextureLeftEye);
		}
		
		if (GUI.Button (new Rect (10, 10, 30, 20), "")) {
			showGUI = !showGUI;
		}
		
		if (showGUI) {
			GUILayout.BeginArea(guiArea);
			GUILayout.BeginHorizontal();
//			if (GUILayout.Button (recordButtonLabel, GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/3))) {
//				if (recordButtonLabel == "Record")
//				{
//					Kamcord.StartRecording();
//					recordButtonLabel = "Stop";
//				}
//				else
//				{
//					Kamcord.StopRecording();
//					Kamcord.ShowView();
//					recordButtonLabel = "Record";
//				}
//			}
//			else
			if (GUILayout.Button("Reset", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/2)))
			{
				PointCloudBehaviour.Instance.Reset();
			}
			else if (GUILayout.Button(modeButtonLabel, GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/2)))
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
			GUILayout.BeginArea (new Rect (Screen.width - 320 - 10, 10, 320, 300));
			
			GUILayout.BeginVertical ();
			
			GUILayout.Label ("Position sensitivity: " + SceneTransform.Instance.positionSensitivity);
			SceneTransform.Instance.positionSensitivity = GUILayout.HorizontalSlider (SceneTransform.Instance.positionSensitivity, 0.01f, 1.0f);
			
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
				SceneTransform.Instance.positionSensitivity = 0.01f;
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
			
			toggle3dStereoVision = GUILayout.Toggle(toggle3dStereoVision, " Enable 3D Stero Vision");
			
			GUILayout.EndVertical ();
			
			GUILayout.EndArea ();	
		}
	}

}
