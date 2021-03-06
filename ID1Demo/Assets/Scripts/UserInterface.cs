using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour
{

	public RenderTexture renderTextureLeftEye;
	public RenderTexture renderTextureRightEye;
	public bool useCustomViewPort = true;
	public bool showExtendedGUI = true;
	public int fontSize = 24;
	public GameObject sunLight;
//	private bool toggle3dStereoVision = true;
	public static Rect leftViewPort = new Rect (0.0f, 0.0f, 0.4685f, 1.0f);
	public static Rect rightViewPort = new Rect (0.5315f, 0.0f, 0.4685f, 1.0f);
	float screenRatio = 0.0f;
	private string labelVideo = "Capture";
	private FeedbackUserInterface feedbackUserInterface;
	public float toastDuration = 2.0f;
	private bool showToastMessage = false;
	private float timeForToastMessage;
	private string toastMessage = "";
	public GUIStyle toastStyle;
	
	
	private float captureTime = 0.0f;
	private float captureDuration = 30.0f;
	private bool captureVideo = false;
	
	private bool forceHideGui = false;
	
	void Start ()
	{
		feedbackUserInterface = GetComponent<FeedbackUserInterface> ();
	}
	
	void Update ()
	{
		if (PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
			PointCloudAdapter.pointcloud_reset ();
			PointCloudBehaviour.ActivateAllImageTargets ();
			PointCloudAdapter.pointcloud_enable_map_expansion ();
		}
		
		if (showToastMessage) {
			timeForToastMessage -= Time.deltaTime;
			if (timeForToastMessage < 0.0f) {
				timeForToastMessage = toastDuration;
				showToastMessage = false;
			}
		}
		
		if (captureVideo) {
			captureTime += Time.deltaTime;
			if (captureTime >= captureDuration) {
				Kamcord.StopRecording();
				Kamcord.ShowView();
				captureTime = 0.0f;
				captureVideo = false;
			}
		}
	}
	
	void DrawExtendedGui ()
	{		
		GUI.Label (new Rect (10, 60, 300, 30), "Position: " + SceneTransform.Instance.position);
		GUI.Label (new Rect (10, 100, 300, 30), "Rotation: " + SceneTransform.Instance.rotation);
		GUI.Label (new Rect (10, 140, 300, 30), "Scale: " + SceneTransform.Instance.scale);
		
		if (GUI.Button (new Rect (10, 180, 200, 100), labelVideo)) {
//			if (labelVideo == "Capture") {
//				Kamcord.StartRecording ();
//				labelVideo = "Stop";
//			} else {
//				if (!Kamcord.StopRecording ()) {
//					showToastMessage = true;
//					toastMessage = "Error while capturing video!";
//				} else {
//					Kamcord.ShowView ();
//				}
//				
//				labelVideo = "Capture";
//			}
			if (!captureVideo) {
				Kamcord.StartRecording ();
				captureVideo = true;
			}
		}
		
		if (Kamcord.IsRecording ()) {
			GUI.Label (new Rect (10, 280, 300, 40), "Recording...");
		} 
		
		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea (new Rect (Screen.width - 500 - 10, 10, 500, 600));
		
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
		
		PointCloudBehaviour.Instance.drawPoints = GUILayout.Toggle (PointCloudBehaviour.Instance.drawPoints, " Draw Points on Image Target");
		forceHideGui = GUILayout.Toggle (forceHideGui, " Force to hide GUI");
		
		GUILayout.Label ("Render Parameters");
		PointCloudBehaviour.Instance.useSplitView = GUILayout.Toggle (PointCloudBehaviour.Instance.useSplitView, " Use Split View");
		if (PointCloudBehaviour.Instance.useSplitView) {
			PointCloudBehaviour.Instance.render3dStereoVision = GUILayout.Toggle (PointCloudBehaviour.Instance.render3dStereoVision, " Render 3D Stereo Vision");
		}
		
		if (sunLight != null) {			
			GUILayout.Label ("Sun direction: " + sunLight.transform.rotation.eulerAngles.y.ToString ("0.00"));
			sunLight.transform.rotation = Quaternion.Euler (sunLight.transform.rotation.eulerAngles.x, GUILayout.HorizontalSlider (sunLight.transform.rotation.eulerAngles.y, 0.0f, 360.0f), sunLight.transform.rotation.eulerAngles.z);
				
			GUILayout.Label ("Sun height: " + sunLight.transform.rotation.eulerAngles.x.ToString ("0.00"));
			sunLight.transform.rotation = Quaternion.Euler (GUILayout.HorizontalSlider (sunLight.transform.rotation.eulerAngles.x, 0.0f, 90.0f), sunLight.transform.rotation.eulerAngles.y, sunLight.transform.rotation.eulerAngles.z);
			
			Light light = sunLight.GetComponent<Light> ();
			if (light != null && light.type == LightType.Directional) {
				GUILayout.Label ("Sun strength: " + sunLight.light.shadowStrength.ToString ("0.00"));
				sunLight.light.shadowStrength = GUILayout.HorizontalSlider (sunLight.light.shadowStrength, 0.0f, 1.5f);
			}
		}
		
		GUILayout.EndVertical ();
		
		GUILayout.EndArea ();
		
		if (showToastMessage) {
			ShowToast (toastMessage);
		}
	}
	
	void OnGUI ()
	{	
		GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.toggle.fontSize = fontSize;
		if (screenRatio == 0.0f) {
			screenRatio = (float)Screen.width / (float)Screen.height;
		}
		
		if (!Application.isEditor) {
			if (PointCloudBehaviour.Instance.useSplitView) {
				Rect left = useCustomViewPort ? new Rect (0, 0, Screen.width * leftViewPort.width, Screen.height) : new Rect (0, 0, Screen.width / 2, Screen.height);
				Rect right = useCustomViewPort ? new Rect (Screen.width * rightViewPort.x, 0, Screen.width * rightViewPort.width, Screen.height) : new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height);
				GUI.DrawTextureWithTexCoords (left, renderTextureLeftEye, new Rect ((1 - 0f - leftViewPort.width) / 2, 0, leftViewPort.width, 1.0f));
				GUI.DrawTextureWithTexCoords (right, renderTextureRightEye, new Rect ((1.0f - rightViewPort.width) / 2, 0, rightViewPort.width, 1.0f));
			} else {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), renderTextureLeftEye);
			}
		}	
		
		if (GUI.Button (new Rect (0, 0, 50, 50), "", GUIStyle.none)) {
			showExtendedGUI = !showExtendedGUI;
		}
			
		if (showExtendedGUI) {
			DrawExtendedGui ();
		}
		
		if (feedbackUserInterface) {
			if (forceHideGui) {
				feedbackUserInterface.drawGui = false;
			} else {
				feedbackUserInterface.drawGui = !showExtendedGUI;
			}
		}
	}
	
//	void OnPointCloudStateChange ()
//	{
//		if (PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
//			PointCloudBehaviour.ActivateAllImageTargets ();
//			PointCloudAdapter.pointcloud_enable_map_expansion ();
//		}
//	}
	
	void ShowToast (string msg)
	{
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
