using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour
{

	public RenderTexture renderTextureLeftEye;
	public RenderTexture renderTextureRightEye;
	public bool useCustomViewPort = true;
	public bool showExtendedGUI = true;
	private bool toggle3dStereoVision = true;
	public static Rect leftViewPort = new Rect (0.0f, 0.0f, 0.4685f, 1.0f);
	public static Rect rightViewPort = new Rect (0.529f, 0.0f, 0.4685f, 1.0f);
	float screenRatio = 0.0f;
	
	void Start ()
	{
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
		
		GUILayout.Label ("Sun Light");
		
		GUILayout.BeginHorizontal ();
		if (GUILayout.RepeatButton ("rotation (+)")) {
			SunLight.Instance.sceneOrientation += 1.0f;
		}
		if (GUILayout.RepeatButton ("rotation (-)")) {
			SunLight.Instance.sceneOrientation -= 1.0f;
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.Label ("3D Stero Vision");
		
		toggle3dStereoVision = GUILayout.Toggle (toggle3dStereoVision, " Enable 3D Stero Vision");
		
		GUILayout.EndVertical ();
		
		GUILayout.EndArea ();
	}
	
	void OnGUI ()
	{	
		if (screenRatio == 0.0f) {
			screenRatio = (float)Screen.width / (float)Screen.height;
		}
		
		if (toggle3dStereoVision) {
			if (Application.isEditor) {
				GUI.DrawTextureWithTexCoords (new Rect (0, 0, Screen.width / 2, Screen.height), renderTextureLeftEye, new Rect (0, 0, 0.5f * screenRatio, 1.0f));
				GUI.DrawTextureWithTexCoords (new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height), renderTextureRightEye, new Rect (0, 0, 0.5f * screenRatio, 1.0f));
			} else {
				Rect left = useCustomViewPort ? new Rect (0, 0, Screen.width * leftViewPort.width, Screen.height) : new Rect (0, 0, Screen.width / 2, Screen.height);
				Rect right = useCustomViewPort ? new Rect (Screen.width * rightViewPort.x, 0, Screen.width * rightViewPort.width, Screen.height) : new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height);
				GUI.DrawTextureWithTexCoords (left, renderTextureLeftEye, new Rect (0, 0, 0.5f, 1.0f));
				GUI.DrawTextureWithTexCoords (right, renderTextureRightEye, new Rect (0, 0, 0.5f, 1.0f));
			}	
		} else {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), renderTextureLeftEye);
		}
		
		if (GUI.Button (new Rect (10, 10, 40, 40), "")) {
			showExtendedGUI = !showExtendedGUI;
		}
		
		// GUI.Label (new Rect (10, 10, 400, 30), "PointCloud State: " + PointCloudBehaviour.State);
		
		if (showExtendedGUI) {
			DrawExtendedGui ();	
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
