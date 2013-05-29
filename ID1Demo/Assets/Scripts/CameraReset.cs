using UnityEngine;
using System.Collections;

public class CameraReset : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!PointCloudBehaviour.HasTracking()) {
			camera.transform.position = new Vector3(0, -100, 0);
			camera.transform.LookAt(new Vector3(0, -200, 0));
		}
	}
}
