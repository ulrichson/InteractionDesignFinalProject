using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloudBehaviour : MonoBehaviour
{
	
	public class S3DV
	{
		internal static	float eyeDistance = 0.02f;
		internal static	float focalDistance = 100.0f;
	};
	
	static public PointCloudBehaviour Instance; // singleton instance
	
	public bool drawPoints = true;
	public float sceneScale = 1f;
	public List<PointCloudImageTarget> imageTargets = new List<PointCloudImageTarget> ();
	public bool use3DSteroVision = true;
	public RenderTexture renderTextureLeftEye;
	public RenderTexture renderTextureRightEye;
	
	// Cooridnate transform
	private Matrix4x4 convert = new Matrix4x4 ();
	private Matrix4x4 pixelTransform;
	
	// Texture
	private Texture2D videoTexture;
	private int videoTextureID; // Native texture ID.
	
	private Rect screenRect;
	private Rect videoTextureCoordinates;
	private int textureSize;
	private float textureSizeInv;
	private Matrix4x4 frustum, cam;
	private bool pointcloudRequestedInitialization = false;
	
	static public pointcloud_state PreviousState { get; private set; }

	static public pointcloud_state State { get; private set; }

	static List<PointCloudSceneRoot> sceneRoots = new List<PointCloudSceneRoot> ();
	
//	private GameObject leftCamera;
//	private GameObject rightCamera;
	
	// WORKAROUND: it seems that it is not trivial to Update to RenderTextures in one Update() call, therefore it will be switches from left/right each frame.
	private bool toggleRenderTexture = false;
	
	public static bool HasTracking ()
	{
		return State == pointcloud_state.POINTCLOUD_TRACKING_IMAGES ||
			   State == pointcloud_state.POINTCLOUD_TRACKING_SLAM_MAP;
	}
	
	public static bool HasInitialized ()
	{
		return State == pointcloud_state.POINTCLOUD_RELOCALIZING ||
			   HasTracking ();
	}
	
	void Awake ()
	{
		if (Instance) {
			LogError ("Only one instance of PointCloudBehaviour allowed!");
		}
		Instance = this;
		Application.targetFrameRate = 60;
	}
	
	void Start ()
	{	
		if (PointCloudAppKey.AppKey == "") {
			LogError ("No PointCloud Application Key provided!");
		}
		
		convert.SetRow (0, new Vector4 (0, -1, 0, 0));
		convert.SetRow (1, new Vector4 (-1, 0, 0, 0));
		convert.SetRow (2, new Vector4 (0, 0, -1, 0));
		convert.SetRow (3, new Vector4 (0, 0, 0, 1));
		
		pixelTransform = Matrix4x4.identity;
	
		pixelTransform [0, 0] = 0;
		pixelTransform [0, 1] = 1;
		pixelTransform [1, 0] = 1;
		pixelTransform [1, 1] = 0;
		
		screenRect = new Rect (0, 0, Screen.height, Screen.width);
		
		Initialize ();
		
//		if (use3DSteroVision) {
//			string name = "" + gameObject.name;
//			gameObject.name = name + " (left)";
//			leftCamera = gameObject;
//			rightCamera = new GameObject (name + " (right)", typeof(Camera));
//			rightCamera.camera.CopyFrom (camera);
//			rightCamera.AddComponent<GUILayer> ();
//			rightCamera.transform.parent = transform;
//			
//			leftCamera.camera.targetTexture = renderTextureLeftEye;
//			rightCamera.camera.targetTexture = renderTextureRightEye;
//			
//			UpdateCameras();
//		}
	}
	
//	void UpdateCameras() {
//		leftCamera.transform.position = transform.position + transform.TransformDirection (-S3DV.eyeDistance, 0f, 0f);
//		rightCamera.transform.position = transform.position + transform.TransformDirection (S3DV.eyeDistance, 0f, 0f);
//		leftCamera.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance));
//		rightCamera.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance));
//	}
	
	void Initialize ()
	{	
		int bigDim = Mathf.Max (Screen.width, Screen.height);
		int smallDim = Mathf.Min (Screen.width, Screen.height);
		pointcloudRequestedInitialization = PointCloudAdapter.init (smallDim, bigDim, PointCloudAppKey.AppKey);
		
		if (!pointcloudRequestedInitialization) {
			// When running from editor, initialize scene imediatly
			State = pointcloud_state.POINTCLOUD_TRACKING_SLAM_MAP;	
		}
		NotifyStateChange (); // Make a notify with the POINTCLOUD_NOT_CREATED state.
	}
	
	void InitializeTexture ()
	{
		int videoWidth = PointCloudAdapter.pointcloud_get_video_width ();
		int videoHeight = PointCloudAdapter.pointcloud_get_video_height ();
		float videoCropX = PointCloudAdapter.pointcloud_get_video_crop_x ();
		float videoCropY = PointCloudAdapter.pointcloud_get_video_crop_y ();
		
		int bigDim = Mathf.Max (videoWidth, videoHeight);
		
		textureSize = GetPowerOfTwo (bigDim);
		textureSizeInv = 1.0f / textureSize;
		
		float cx = videoCropX / textureSize;
		float cy = videoCropY / textureSize;

		videoTexture = new Texture2D (textureSize, textureSize, TextureFormat.BGRA32, false);	
		videoTextureID = videoTexture.GetNativeTextureID ();
		
		videoTextureCoordinates = new Rect (videoWidth * textureSizeInv - cx,
			                               cy,
			                               -videoWidth * textureSizeInv + 2 * cx,
			                               videoHeight * textureSizeInv - 2 * cy);
	}
	
	void AddImageTargets ()
	{
		foreach (PointCloudImageTarget imageTarget in imageTargets) {
			PointCloudAdapter.pointcloud_add_image_target (imageTarget);
		}
	}
	
	public void Reset ()
	{
		if (Application.isEditor) {
			// From editor, simulate reset of point cloud
			State = pointcloud_state.POINTCLOUD_IDLE;
			NotifyStateChange ();
		}
		
		PointCloudAdapter.pointcloud_reset ();
		
		Initialize ();
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if (!pointcloudRequestedInitialization)
			return;
		
		int flags = PointCloudAdapter.update (videoTextureID, camera.nearClipPlane, camera.farClipPlane, drawPoints, ref cam, ref frustum);
		
		bool texture_updated = (flags & 1) > 0;
		bool transforms_updated = (flags & 2) > 0;
		
		if (!texture_updated && State == pointcloud_state.POINTCLOUD_NOT_CREATED) {
			// render solid color whlie waiting for camera and pointcloud to start
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.backgroundColor = Color.black;
		} else {
			if (!videoTexture) {
				InitializeTexture ();
				AddImageTargets ();
			}
			
			camera.clearFlags = CameraClearFlags.Depth;
			// camera.clearFlags = CameraClearFlags.Color;
			if (transforms_updated) {
				Matrix4x4 camera_matrix = convert * cam;
				Matrix4x4 camera_pose = camera_matrix.inverse;
			
				camera.transform.position = camera_pose.GetColumn (3) / sceneScale;
				camera.transform.rotation = QuaternionFromMatrixColumns (camera_pose);
			
				camera.projectionMatrix = frustum * convert;
				
				switch (Screen.orientation) {
				default:
				case ScreenOrientation.LandscapeLeft:
					RotateProjectionMatrix (90);
					break;
				case ScreenOrientation.LandscapeRight:
					RotateProjectionMatrix (-90);
					break;
				case ScreenOrientation.Portrait:
					break;
				case ScreenOrientation.PortraitUpsideDown:
					RotateProjectionMatrix (180);
					break;
				}
			}
			
			if (use3DSteroVision) {
				toggleRenderTexture = !toggleRenderTexture;
				// Vector3 focalPoint = transform.position + transform.forward * S3DV.focalDistance; 
				if (toggleRenderTexture) {
					transform.Translate(Vector3.up * S3DV.eyeDistance);
					// transform.LookAt(focalPoint);
				} else {
					transform.Translate(Vector3.down * S3DV.eyeDistance);
					// transform.LookAt(focalPoint);
				}
				
				RenderToTexture (toggleRenderTexture ? renderTextureLeftEye : renderTextureRightEye);
			} else {
				RenderToTexture (null);	
			}
		}
		
		MonitorStateChanges ();
	}
	
	void RenderToTexture (RenderTexture rt)
	{
		if (rt != null) {
			camera.targetTexture = rt;
			RenderTexture.active = camera.targetTexture;
		}
		GL.PushMatrix ();
		GL.LoadPixelMatrix ();
		switch (Screen.orientation) {
		default:
		case ScreenOrientation.LandscapeLeft:
			screenRect = new Rect (Screen.width, 0, -Screen.width, Screen.height);
			break;
		case ScreenOrientation.LandscapeRight:
			screenRect = new Rect (0, Screen.height, Screen.width, -Screen.height);
			break;
		case ScreenOrientation.PortraitUpsideDown:
			GL.MultMatrix (pixelTransform);
			screenRect = new Rect (Screen.height, Screen.width, -Screen.height, -Screen.width);
			break;
		case ScreenOrientation.Portrait:
			GL.MultMatrix (pixelTransform);
			screenRect = new Rect (0, 0, Screen.height, Screen.width);
			break;
		}
		Graphics.DrawTexture (screenRect, videoTexture, videoTextureCoordinates, 0, 0, 0, 0);
		GL.PopMatrix ();
		if (rt != null) {
			RenderTexture.active = null;
		}
	}

	static Quaternion QuaternionFromMatrixColumns (Matrix4x4 m)
	{
		return Quaternion.LookRotation (-m.GetColumn (2), m.GetColumn (1)); // forward, up
	}
	
	void MonitorStateChanges ()
	{
		pointcloud_state newState = PointCloudAdapter.pointcloud_get_state ();
		if (newState != State) {
			PreviousState = State;
			State = newState;
			NotifyStateChange ();
		}
	}
	
	void NotifyStateChange ()
	{
		foreach (PointCloudSceneRoot sceneRoot in sceneRoots) {
			sceneRoot.gameObject.SendMessage ("OnPointCloudStateChanged", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnDestroy ()
	{
		PointCloudAdapter.pointcloud_destroy ();
	}
	
	public static void AddSceneRoot (PointCloudSceneRoot sceneRoot)
	{
		sceneRoots.Add (sceneRoot);
	}

	private int GetPowerOfTwo (int pow2)
	{
		pow2--;
		pow2 |= pow2 >> 1;
		pow2 |= pow2 >> 2;
		pow2 |= pow2 >> 4;
		pow2 |= pow2 >> 8;
		pow2 |= pow2 >> 16;
		pow2++;
		
		return pow2;
	}
	
	private void LogError (string msg)
	{
		Debug.LogError (msg);
		Application.Quit ();
	}
	
	private void RotateProjectionMatrix (float angleDegrees)
	{
		Quaternion correction_q = Quaternion.AngleAxis (angleDegrees, new Vector3 (0, 0, 1));
		Matrix4x4 correction_rot = Matrix4x4.TRS (Vector3.zero, correction_q, new Vector3 (1, 1, 1));
		camera.projectionMatrix = correction_rot * camera.projectionMatrix;
	}
	
	public static void ActivateAllImageTargets ()
	{
		foreach (PointCloudImageTarget target in Instance.imageTargets) {
			target.Activate ();
		}
	}
	
	public static void DeactivateAllImageTargets ()
	{
		foreach (PointCloudImageTarget target in Instance.imageTargets) {
			target.Deactivate ();
		}
	}
}