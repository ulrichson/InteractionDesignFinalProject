using UnityEngine;
using System.Collections;

public class SceneOrientation : MonoBehaviour {
	
	public float sceneOrientation = 0.0f;

	// Use this for initialization
	void Start () {
		transform.Rotate(Vector3.up * sceneOrientation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
