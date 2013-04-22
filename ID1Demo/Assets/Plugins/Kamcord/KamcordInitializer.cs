// ------------------------------------------------------------------------------
// KamcordInitializer lets you set Kamcord parameters from within the Unity IDE.
//
// To use this prefab, drag it from Prefabs/Kamcord into the scene (Hierarchy tab)
// ------------------------------------------------------------------------------

using UnityEngine;

public class KamcordInitializer : MonoBehaviour 
{	
#if UNITY_IPHONE
	
	// Public properties
	public string developerKey    			   		  = "Kamcord developer key";
	public string developerSecret 			   		  = "Kamcord developer secret";
	public string appName   				   		  = "Application name";
	public Kamcord.DeviceOrientation deviceOrientation = Kamcord.DeviceOrientation.Portrait;
	public Kamcord.VideoResolution videoResolution     = Kamcord.VideoResolution.Medium;
	
	// Can be used to disable Kamcord on certain devices
	public bool disableIpod4Gen				= false;
	public bool disableIpod5Gen				= false;
	public bool disableIphone3GS			= false;
	public bool disableIphone4				= false;
	public bool disableIpad1				= false;
	public bool disableIpad2				= false;
	public bool disableIpadMini				= false;
	
	// Public methods
	void Awake()
	{
		// Ensure this object's name
		this.gameObject.name = "KamcordPrefab";
		
		// Never destroy
		DontDestroyOnLoad(this);
		
		// Set device blacklist
		Kamcord.SetDeviceBlacklist(disableIpod4Gen,
								   disableIpod5Gen,
								   disableIphone3GS,
								   disableIphone4,
								   disableIpad1,
								   disableIpad2,
								   disableIpadMini);
		
		// Init
		Kamcord.Init(developerKey, developerSecret, appName, deviceOrientation, videoResolution);
		Kamcord.SubscribeToCallbacks(true);
		
		// Get the buffer size and num buffers
		int bufferSize;
		int numBuffers;
		AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
		
		// Get the number of channels
		int numChannels = Kamcord.GetNumChannelsFromSpeakerMode(AudioSettings.speakerMode);
		
		// Tell Kamcord
		Kamcord.SetAudioSettings(AudioSettings.outputSampleRate, bufferSize, numChannels);
	}
	
	void Start()
	{
		Kamcord.AddKamcordAudioRecorderToAudioListenerObjects();
	}

	void OnApplicationPause(bool pause)
	{
		if (pause)
			Kamcord.Pause();
		else
			Kamcord.Resume();
	}
	
	void OnLevelWasLoaded(int level)
	{
		Kamcord.AddKamcordAudioRecorderToAudioListenerObjects();
	}
	
	//////////////////////////////////////////////////////////////////
    /// Handling callbacks from Objective-C
    /// 
	
	// The Kamcord share view appeared
	private void KamcordViewDidAppear(string empty)
	{
		Debug.Log("KamcordViewDidAppear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordViewDidAppear();
		}
	}
	
	private void KamcordViewWillDisappear(string empty)
	{
		Debug.Log ("KamcordViewWillDisappear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordViewWillDisappear();
		}
	}
	
	// The Kamcord share view disappeared
	private void KamcordViewDidDisappear(string empty)
	{
		Debug.Log ("KamcordViewDidDisappear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordViewDidDisappear();
		}
	}
	
	// The Kamcord watch view appeared
	private void KamcordWatchViewDidAppear(string empty)
	{
		Debug.Log("KamcordWatchViewDidAppear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordWatchViewDidAppear();
		}
	}
	
	private void KamcordWatchViewWillDisappear(string empty)
	{
		Debug.Log ("KamcordWatchViewWillDisappear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordWatchViewWillDisappear();
		}
	}
	
	// The Kamcord watch view disappeared
	private void KamcordWatchViewDidDisappear(string empty)
	{
		Debug.Log ("KamcordWatchViewDidDisappear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.KamcordWatchViewDidDisappear();
		}
	}
	
	
	// The video replay view appeared
	private void MoviePlayerDidAppear(string empty)
	{
		Debug.Log ("MoviePlayerDidAppear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.MoviePlayerDidAppear();
		}
	}
	
	// The video replay view disappeared
	private void MoviePlayerDidDisappear(string empty)
	{
		Debug.Log ("MoviePlayerDidDisappear");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.MoviePlayerDidDisappear();
		}
	}
	
	// The share button was pressed
	private void ShareButtonPressed(string empty)
	{
		Debug.Log("ShareButtonPressed");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.ShareButtonPressed();
		}
	}
	
	// The thumbnail for the latest video is ready at this
	// absolute file path
	private void VideoThumbnailReadyAtFilePath(string filepath)
	{
		Debug.Log ("VideoThumbnailReadyAtFilePath: " + filepath);
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.VideoThumbnailReadyAtFilePath(filepath);
		}
	}
	
	// When the video begins and finishes uploading
	private void VideoWillUploadToURL(string url)
	{
		Debug.Log ("VideoWillBeginUploading: " + url);
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.VideoWillBeginUploading(url);
		}
	}
	
	private void VideoUploadedWithSuccess(string success)
	{
		Debug.Log ("VideoFinishedUploading: " + success);
		bool truthValue = (success == "true" ? true : false);
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.VideoFinishedUploading(truthValue);
		}
	}
	
	// When the call to action button on the notification view was pressed
	private void PushNotifCallToActionButtonPressed()
	{
		Debug.Log ("PushNotifCallToActionButtonPressed");
		foreach (KamcordCallbackInterface listener in Kamcord.listeners)
		{
			listener.PushNotifCallToActionButtonPressed();
		}
	}
#endif
}
