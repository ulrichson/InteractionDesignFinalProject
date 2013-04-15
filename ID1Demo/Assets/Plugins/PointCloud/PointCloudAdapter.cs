using System;
using System.Runtime.InteropServices;
using UnityEngine;
public class PointCloudAdapter {
	public int pointcloudsize;
	
	static IntPtr cameraMatrixPtr = IntPtr.Zero;
	static float[] cameraMatrixArray = new float[16];
	static IntPtr projectionMatrixPtr = IntPtr.Zero;
	static float[] projectionMatrixArray = new float[16];
	
	public static bool init(int viewport_width, int viewport_height, string app_key) {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (cameraMatrixPtr == IntPtr.Zero)
			{
				cameraMatrixPtr = Marshal.AllocHGlobal(sizeof(float) * 16);
			}
			if (projectionMatrixPtr == IntPtr.Zero) 
			{
				projectionMatrixPtr = Marshal.AllocHGlobal(sizeof(float) * 16);
			}
			
	    	_pointcloudInternal.pointcloud_unity_init(viewport_width, viewport_height, app_key);
			
			return true;
		}
		return false;
  	}

	public static int update(int textureId, float nearPlane, float farPlane, bool drawPoints, ref Matrix4x4 worldToCameraMatrix, ref Matrix4x4 projectionMatrix) {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{				
	    	int flags = _pointcloudInternal.pointcloud_unity_update(textureId, nearPlane, farPlane, drawPoints, ref cameraMatrixPtr, ref projectionMatrixPtr);
			
			if ((flags & 2) > 0) 
			{
				Marshal.Copy(cameraMatrixPtr, cameraMatrixArray, 0, 16);
				Marshal.Copy(projectionMatrixPtr, projectionMatrixArray, 0, 16);
				
				worldToCameraMatrix.SetRow(0,new Vector4(-cameraMatrixArray[0],cameraMatrixArray[4],cameraMatrixArray[8],cameraMatrixArray[12]));
				worldToCameraMatrix.SetRow(1,new Vector4(-cameraMatrixArray[1],cameraMatrixArray[5],cameraMatrixArray[9],cameraMatrixArray[13]));
				worldToCameraMatrix.SetRow(2,new Vector4(-cameraMatrixArray[2],cameraMatrixArray[6],cameraMatrixArray[10],cameraMatrixArray[14]));
				worldToCameraMatrix.SetRow(3,new Vector4(cameraMatrixArray[3],cameraMatrixArray[7],cameraMatrixArray[11],cameraMatrixArray[15]));
				
				projectionMatrix.SetRow(0, new Vector4(projectionMatrixArray[0], projectionMatrixArray[4], projectionMatrixArray[8], projectionMatrixArray[12]));
				projectionMatrix.SetRow(1, new Vector4(projectionMatrixArray[1], projectionMatrixArray[5], projectionMatrixArray[9], projectionMatrixArray[13]));
				projectionMatrix.SetRow(2, new Vector4(projectionMatrixArray[2], projectionMatrixArray[6], projectionMatrixArray[10], projectionMatrixArray[14]));
				projectionMatrix.SetRow(3, new Vector4(projectionMatrixArray[3], projectionMatrixArray[7], projectionMatrixArray[11], projectionMatrixArray[15]));
			}
			
			return flags;
		}
		return 0;
  	}

  	public static void pointcloud_destroy() {
		if (cameraMatrixPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(cameraMatrixPtr);
			cameraMatrixPtr = IntPtr.Zero;
		}
		
		if (projectionMatrixPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(projectionMatrixPtr);
			projectionMatrixPtr = IntPtr.Zero;
		}
		
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
    		_pointcloudInternal.pointcloud_unity_destroy();
		}
	}

  	public static void pointcloud_start_slam() {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
	    	_pointcloudInternal.pointcloud_unity_start_slam();
  	}
		
  	public static void pointcloud_reset() {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
	    	_pointcloudInternal.pointcloud_unity_reset();
  	}
	
	public static pointcloud_state pointcloud_get_state() {
	    return (pointcloud_state)_pointcloudInternal.pointcloud_unity_get_state();
  	}
	
	public static void pointcloud_destroy_point_cloud(IntPtr outArray) 
	{
	   Marshal.FreeCoTaskMem(outArray);
	}
	
	public static int pointcloud_get_video_width() {
		return 	_pointcloudInternal.pointcloud_unity_get_video_width();
	}
	
	public static int pointcloud_get_video_height() {
		return 	_pointcloudInternal.pointcloud_unity_get_video_height();
	}
	
	public static int pointcloud_get_video_crop_x() {
		return _pointcloudInternal.pointcloud_unity_get_video_crop_x();
	}
	
	public static int pointcloud_get_video_crop_y() {
		return _pointcloudInternal.pointcloud_unity_get_video_crop_y();
	}
	
	public static pointcloud_vector_3[] pointcloud_get_points()
	{
		int size;
		IntPtr outArray; 
		
		_pointcloudInternal.pointcloud_unity_get_points(out size, out outArray); 
		
		pointcloud_vector_3[] points = new pointcloud_vector_3[size];
		IntPtr current = outArray;
		
		//O(n) copy+paste, there may be a better way of marshalling this.
		for(int i = 0; i < size; i++)
		{
			points[i] = (pointcloud_vector_3)(Marshal.PtrToStructure(current, typeof(pointcloud_vector_3)));
			current = (IntPtr)((long)current + Marshal.SizeOf(points[i]));
		}
		outArray = System.IntPtr.Zero;
		current = System.IntPtr.Zero;
		return points;
	}
	
	// File should be included in a Unity Resources folder as image_target_name.bytes 
	public static void pointcloud_add_image_target(PointCloudImageTarget imageTarget) {
		TextAsset im = imageTarget.imageTarget;
		float w = imageTarget.physicalWidth;
		float h = imageTarget.physicalHeight;
		_pointcloudInternal.pointcloud_unity_add_image_target(im.name, im.bytes, im.bytes.Length, w, h);
	}
	
	public static void pointcloud_remove_image_target(string image_id) {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_pointcloudInternal.pointcloud_unity_remove_image_target(image_id);
	}
    
    public static void pointcloud_activate_image_target(string image_id) {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_pointcloudInternal.pointcloud_unity_activate_image_target(image_id);
	}
   
    public static void pointcloud_deactivate_image_target(string image_id) {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_pointcloudInternal.pointcloud_unity_deactivate_image_target(image_id);
	}
	
	public static void pointcloud_enable_map_expansion() {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_pointcloudInternal.pointcloud_unity_enable_map_expansion();
	}
	
	public static void pointcloud_disable_map_expansion() {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			_pointcloudInternal.pointcloud_unity_disable_map_expansion();
	}
}
