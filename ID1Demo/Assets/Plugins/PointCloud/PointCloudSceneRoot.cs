using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloudSceneRoot : MonoBehaviour {
	
	public bool hideChildrenIfNoTracking = true;
	public List<GameObject> ignoreGameObjects = new List<GameObject> ();
		
	void Awake()
	{
		PointCloudBehaviour.AddSceneRoot(this);
	}
	
	public static void EnableRenderingRecursively(Transform go, bool enable, List<GameObject> ignoreGameObjects)
	{
		if (ignoreGameObjects.Contains(go.gameObject))
			return;
		
		Renderer rendererComponent = go.GetComponent<Renderer>();
		if (rendererComponent != null)
		{
			rendererComponent.enabled = enable;
		}
			
		foreach(Transform child in go)
		{
			EnableRenderingRecursively(child, enable, ignoreGameObjects);
		}
	}
	
	public virtual void OnPointCloudStateChanged()
	{
		if(hideChildrenIfNoTracking) {
			bool showChildren = PointCloudBehaviour.HasTracking();
			EnableRenderingRecursively(transform, showChildren, ignoreGameObjects);
		}
	}
}
