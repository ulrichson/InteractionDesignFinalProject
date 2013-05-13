using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
	
	public int zoom = 20;
	public int normal = 60;
	public float smooth = 5;
	private bool isZoomed = false;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			isZoomed = !isZoomed;
		}
 
		if (isZoomed)
		{
			PointCloudBehaviour.Instance.camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, zoom, Time.deltaTime * smooth);
		}
		else
		{
			PointCloudBehaviour.Instance.camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, normal, Time.deltaTime * smooth);
		}
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), isZoomed ? "Zoomed in" : "Zoomed out");
    }
}
