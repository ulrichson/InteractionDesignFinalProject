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
	
	private FeedbackUserInterface feedbackUserInterface;
	
	void Start ()
	{
		feedbackUserInterface = GetComponent<FeedbackUserInterface> ();
	}
	
	void Update ()
	{
		if (PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
			PointCloudAdapter.pointcloud_reset();
			PointCloudBehaviour.ActivateAllImageTargets();
        	PointCloudAdapter.pointcloud_enable_map_expansion();
		}
	}
	
	void DrawExtendedGui ()
	{		
		GUI.Label (new Rect (10, 60, 300, 30), "Position: " + SceneTransform.Instance.position);
		GUI.Label (new Rect (10, 100, 300, 30), "Rotation: " + SceneTransform.Instance.rotation);
		GUI.Label (new Rect (10, 140, 300, 30), "Scale: " + SceneTransform.Instance.scale);
		
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
		
		GUILayout.Label ("Render Parameters");
		PointCloudBehaviour.Instance.useSplitView = GUILayout.Toggle (PointCloudBehaviour.Instance.useSplitView, " Use Split View");
		if (PointCloudBehaviour.Instance.useSplitView) {
		
			// toggle3dStereoVision = GUILayout.Toggle (toggle3dStereoVision, " Enable 3D Stero Vision");
			PointCloudBehaviour.Instance.render3dStereoVision = GUILayout.Toggle (PointCloudBehaviour.Instance.render3dStereoVision, " Render 3D Stereo Vision");
		}
		
		if (sunLight != null) {			
			GUILayout.Label("Sun direction: " + sunLight.transform.rotation.eulerAngles.y.ToString("0.00"));
			sunLight.transform.rotation = Quaternion.Euler(sunLight.transform.rotation.eulerAngles.x, GUILayout.HorizontalSlider(sunLight.transform.rotation.eulerAngles.y, 0.0f, 360.0f), sunLight.transform.rotation.eulerAngles.z);
				
			GUILayout.Label("Sun height: " + sunLight.transform.rotation.eulerAngles.x.ToString("0.00"));
			sunLight.transform.rotation = Quaternion.Euler(GUILayout.HorizontalSlider(sunLight.transform.rotation.eulerAngles.x, 0.0f, 90.0f), sunLight.transform.rotation.eulerAngles.y, sunLight.transform.rotation.eulerAngles.z);
			
			Light light = sunLight.GetComponent<Light> ();
			if (light != null && light.type == LightType.Directional) {
				GUILayout.Label("Sun strength: " + sunLight.light.shadowStrength.ToString("0.00"));
				sunLight.light.shadowStrength = GUILayout.HorizontalSlider(sunLight.light.shadowStrength, 0.0f, 1.5f);
			}
		}
		
		GUILayout.EndVertical ();
		
		GUILayout.EndArea ();
	}
	
	void OnGUI ()
	{	
		GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.toggle.fontSize = fontSize;
		if (screenRatio == 0.0f) {
			screenRatio = (float)Screen.width / (float)Screen.height;
		}
		
		if (PointCloudBehaviour.Instance.useSplitView) {
			if (PointCloudBehaviour.Instance.useSplitView) {
				if (Application.isEditor) {
					GUI.DrawTextureWithTexCoords (new Rect (0, 0, Screen.width / 2, Screen.height), renderTextureLeftEye, new Rect (0, 0, 0.5f * screenRatio, 1.0f));
					GUI.DrawTextureWithTexCoords (new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height), renderTextureRightEye, new Rect (0, 0, 0.5f * screenRatio, 1.0f));
				} else {
					Rect left = useCustomViewPort ? new Rect (0, 0, Screen.width * leftViewPort.width, Screen.height) : new Rect (0, 0, Screen.width / 2, Screen.height);
					Rect right = useCustomViewPort ? new Rect (Screen.width * rightViewPort.x, 0, Screen.width * rightViewPort.width, Screen.height) : new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height);
					GUI.DrawTextureWithTexCoords (left, renderTextureLeftEye, new Rect ( (1-0f - leftViewPort.width) / 2, 0, leftViewPort.width, 1.0f));
					GUI.DrawTextureWithTexCoords (right, renderTextureRightEye, new Rect ( (1.0f - rightViewPort.width) / 2, 0, rightViewPort.width, 1.0f));
				}	
			} else {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), renderTextureLeftEye);
			}	
		}
		
		if (GUI.Button (new Rect (10, 10, 40, 40), "")) {
			showExtendedGUI = !showExtendedGUI;
		}
		
		// GUI.Label (new Rect (10, 10, 400, 30), "PointCloud State: " + PointCloudBehaviour.State);
		
		if (showExtendedGUI) {
			DrawExtendedGui ();
		}
		
		if (feedbackUserInterface) {
			feedbackUserInterface.drawGui = !showExtendedGUI;
		}
	}
	
//	void OnPointCloudStateChange ()
//	{
//		if (PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
//			PointCloudBehaviour.ActivateAllImageTargets ();
//			PointCloudAdapter.pointcloud_enable_map_expansion ();
//		}
//	}

}
