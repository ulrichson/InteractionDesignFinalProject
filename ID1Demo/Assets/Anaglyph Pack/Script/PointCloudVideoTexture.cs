using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class PointCloudVideoTexture : MonoBehaviour
{
	public enum View { Left, Right };
	public View view;
	public Texture2D texture;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
//		if (texture != null) {
//			camera.clearFlags = CameraClearFlags.Depth;
//			GL.PushMatrix();
//	    	GL.LoadPixelMatrix();
//			//Graphics.DrawTexture(view == View.Left ? AnaglyphizerC.leftViewPort : AnaglyphizerC.rightViewPort, PointCloudBehaviour.Instance.videoTexture, PointCloudBehaviour.Instance.videoTextureCoordinates, 0, 0, 0, 0);
//			Graphics.DrawTexture(new Rect(0.0f, 0.0f, 1.0f, 1.0f), texture);
//			GL.PopMatrix();
//		}
	}
}

