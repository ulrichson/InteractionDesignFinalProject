using UnityEngine;
using System.Collections;

public class SceneTransform : MonoBehaviour
{
	static public SceneTransform Instance;
	
	/** Show/hide the user interface */
	public bool showUserInterface = true;
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public float scale = 1.0f;
	private float positionSensitivity = 10.0f;
	
	void Awake ()
	{
		if (Instance) {
			Debug.LogError ("Only one instance of SceneTransform allowed!");
		}
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = position;
		transform.rotation = Quaternion.Euler (rotation);
		transform.localScale = new Vector3 (scale, scale, scale);
		
//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//		{
//			showUserInterface = !showUserInterface;
//		}
	}
	
	void OnGUI ()
	{
		if (showUserInterface) {
			GUI.Label (new Rect (10, 10, 200, 20), "Position: " + transform.position);
			GUI.Label (new Rect (10, 30, 200, 20), "Rotation: " + transform.localEulerAngles);
			GUI.Label (new Rect (10, 50, 200, 20), "Scale: " + transform.localScale);
			
			// Wrap everything in the designated GUI Area
			GUILayout.BeginArea (new Rect (Screen.width - 300 - 10, 10, 300, 300));
			
			GUILayout.BeginVertical ();
			
			GUILayout.Label ("Position sensitivity: " + positionSensitivity);
			positionSensitivity = GUILayout.HorizontalSlider (positionSensitivity, 1.0f, 50.0f);
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("p.x+")) {
				position.x += positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.x-")) {
				position.x -= positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.y+")) {
				position.y += positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.y-")) {
				position.y -= positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.z+")) {
				position.z += positionSensitivity;
			}
			if (GUILayout.RepeatButton ("p.z-")) {
				position.z -= positionSensitivity;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("r.x+")) {
				rotation.x += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.x-")) {
				rotation.x -= 1.0f;
			}
			if (GUILayout.RepeatButton ("r.y+")) {
				rotation.y += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.y-")) {
				rotation.y -= 1.0f;
			}
			if (GUILayout.RepeatButton ("r.z+")) {
				rotation.z += 1.0f;
			}
			if (GUILayout.RepeatButton ("r.z-")) {
				rotation.z -= 1.0f;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();
			if (GUILayout.RepeatButton ("s+")) {
				scale += 0.01f;
			}
			if (GUILayout.RepeatButton ("s-")) {
				scale -= 0.01f;
			}
			GUILayout.EndHorizontal ();
			
			if (GUILayout.Button ("Reset")) {
				position = Vector3.zero;
				rotation = Vector3.zero;
				scale = 1.0f;
				positionSensitivity = 10.0f;
			}
			
			GUILayout.EndVertical ();
			
			GUILayout.EndArea ();
		}
	}
}
