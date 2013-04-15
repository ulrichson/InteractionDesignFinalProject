using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloudSceneRoot : MonoBehaviour {
	
	public bool hideChildrenIfNoTracking = true;
		
	void Awake()
	{
		PointCloudBehaviour.AddSceneRoot(this);
	}
	
	public static void EnableRenderingRecursively(Transform go, bool enable)
	{
		Renderer rendererComponent = go.GetComponent<Renderer>();
		if (rendererComponent != null)
		{
			rendererComponent.enabled = enable;
		}
			
		foreach(Transform child in go)
		{
			EnableRenderingRecursively(child, enable);
		}
	}
	
	public virtual void OnPointCloudStateChanged()
	{
		if(hideChildrenIfNoTracking) {
			bool showChildren = PointCloudBehaviour.HasTracking();
			EnableRenderingRecursively(transform, showChildren);
		}
	}
}
