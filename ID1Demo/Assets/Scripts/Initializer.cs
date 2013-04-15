using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnPointCloudStateChange ()
	{
		if (PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
			PointCloudAdapter.pointcloud_start_slam ();
		}
	}
}
