using UnityEngine;
using System.Collections;

public class SceneTransform : MonoBehaviour
{
	static public SceneTransform Instance;
	
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public float scale = 1.0f;
	public float positionSensitivity = 10.0f;
	
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
	}
}
