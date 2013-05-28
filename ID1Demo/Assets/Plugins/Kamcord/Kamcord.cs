using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
/// Version: 1.1.2 (2013-04-21)
//////////////////////////////////////////////////////////////////

public class Kamcord
{
#if UNITY_IPHONE
	//////////////////////////////////////////////////////////////////
	/// Method declarations
	//////////////////////////////////////////////////////////////////
	
	/* Interface to native implementation */
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordSetDeviceBlacklist(bool disableiPod4G,
														  bool disableiPod5G,
														  bool disableiPhone3GS,
														  bool disableiPhone4,
														  bool disableiPad1,
														  bool disableiPad2,
														  bool disableiPadMini);
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordIsEnabled();
	
	[DllImport ("__Internal")]
	private static extern void _KamcordInit(string devKey,
											string devSecret,
											string appName,
											string deviceOrientation, 
											string videoResolution);
	
	[DllImport ("__Internal")]
	private static extern void _KamcordSetDeviceOrientation(string deviceOrientation);
	
	[DllImport ("__Internal")]
	private static extern string _KamcordDeviceOrientation();
	
	//////////////////////////////////////////////////////////////////
    /// Share settings
    ///
	[DllImport ("__Internal")]
	private static extern void _KamcordSetDefaultTitle(string title);
	
	[DllImport ("__Internal")]
	private static extern void _KamcordSetFacebookAppID(string facebookAppID);
	
	[DllImport ("__Internal")]
	private static extern void _KamcordSetFacebookSettings(string title,
                                                           string caption,
                                                           string description);
	
	[DllImport ("__Internal")]
    private static extern void _KamcordSetYouTubeSettings(string description,
                                                          string tags);
	
	[DllImport ("__Internal")]
	private static extern void _KamcordSetYouTubeVideoCategory(string category);
    
    [DllImport ("__Internal")]
    private static extern void _KamcordSetDefaultFacebookMessage(string message);
	
	[DllImport ("__Internal")]
    private static extern void _KamcordSetDefaultTweet(string tweet);
	
	[DllImport ("__Internal")]
	private static extern void _KamcordSetTwitterDescription(string twitterDescription);
		
	[DllImport ("__Internal")]
    private static extern void _KamcordSetDefaultYouTubeMessage(string message);
    
	[DllImport ("__Internal")]
    private static extern void _KamcordSetDefaultEmailSubjectAndBody(string subject,
                                                                     string body);
    
	[DllImport ("__Internal")]
    private static extern void _KamcordSetLevelAndScore(string level,
                                                        double score);
	
	//////////////////////////////////////////////////////////////////
    /// Video recording 
    ///
	[DllImport ("__Internal")]
	private static extern bool _KamcordStartRecording();
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordStopRecording();
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordPause();
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordResume();
	
	[DllImport ("__Internal")]
	private static extern bool _KamcordIsRecording();

	[DllImport ("__Internal")]
	private static extern bool _KamcordCaptureFrame();
	
	//////////////////////////////////////////////////////////////////
    /// UI 
    ///
    [DllImport ("__Internal")]
    private static extern void _KamcordShowView();
	
	[DllImport ("__Internal")]
    private static extern void _KamcordShowWatchView();
    
	[DllImport ("__Internal")]
    private static extern void _KamcordSetShowVideoControlsOnReplay(bool showControls);
    
	[DllImport ("__Internal")]
    private static extern bool _KamcordShowVideoControlsOnReplay();
    
	//////////////////////////////////////////////////////////////////
    /// Notifications
    ///
	[DllImport ("__Internal")]
	private static extern void _KamcordSetNotificationsEnabled(bool notificationsEnabled);

	[DllImport ("__Internal")]
	private static extern void _KamcordHandleNotification();

	[DllImport ("__Internal")]
	private static extern void _KamcordFireTestNotification();
	
	//////////////////////////////////////////////////////////////////
    /// Sundry Methods
    ///
    [DllImport ("__Internal")]
    private static extern bool _KamcordCancelConversionForLatestVideo();
    
	[DllImport ("__Internal")]
    private static extern void _KamcordSetMaximumVideoLength(uint seconds);
    
	[DllImport ("__Internal")]
    private static extern uint _KamcordMaximumVideoLength();
    
    //////////////////////////////////////////////////////////////////
    /// Custom Sharing UI
    /// 
    
	[DllImport ("__Internal")]
	private static extern void _KamcordPresentVideoPlayerFullscreen();
    
    // TODO: setShareDelegate
    //       shareDelegate
        
	[DllImport ("__Internal")]
	private static extern void _KamcordSubscribeToCallbacks(bool subscribe);
	
	[DllImport ("__Internal")]
    private static extern void _KamcordShowFacebookLoginView();
    
	[DllImport ("__Internal")]
    private static extern void _KamcordShowTwitterAuthentication();
	
	// TODO:
	// [DllImport ("__Internal")]
    // void _KamcordShowYouTubeLoginViewInViewController(UIViewController * parentViewController);
    
    [DllImport ("__Internal")]
    private static extern bool _KamcordFacebookIsAuthenticated();
    
	[DllImport ("__Internal")]
    private static extern bool _KamcordTwitterIsAuthenticated();
    
	[DllImport ("__Internal")]
    private static extern bool _KamcordYouTubeIsAuthenticated();
    
    [DllImport ("__Internal")]
	private static extern void _KamcordPerformFacebookLogout();
    
	[DllImport ("__Internal")]
    private static extern void _KamcordPerformYouTubeLogout();
    
    [DllImport ("__Internal")]
    private static extern bool _KamcordShareVideoWithMessage(string message,
                                                             bool shareOnFacebook,
                                                             bool shareOnTwitter,
                                                             bool shareOnYouTube);
    
	[DllImport ("__Internal")]
	private static extern bool _KamcordDisable();
	
    // TODO:
	// [DllImport ("__Internal")]
    // void _KamcordPresentComposeEmailViewInViewController(UIViewController * parentViewController,
    //                                                      string bodyText)
#endif
	
	// Possible values of deviceOrientation:
	public enum DeviceOrientation
	{
		Portrait,
		LandscapeLeft,
		LandscapeRight,
		PortraitUpsideDown
	};
	
	// Possible values of videoResolution
	public enum VideoResolution
	{
		Smart,
		Medium,
		Trailer	// Do not release your game with this setting!
	};
	
	// Possible values of the YouTube video category
	public enum YouTubeVideoCategory
	{
		Comedy,
		Education,
		Entertainment,
		Games,
		Music
	};
	
#if UNITY_IPHONE
	private static bool orientationWasPreviouslySet = false;
#endif
	
	//////////////////////////////////////////////////////////////////
    /// Implementations
    //////////////////////////////////////////////////////////////////
	
	/* Public interface for use inside C# / JS code */
	
	// If this method is to be used, it **MUST** be the first Kamcord method called
	public static void SetDeviceBlacklist(bool disableiPod4G,
										  bool disableiPod5G,
										  bool disableiPhone3GS,
										  bool disableiPhone4,
										  bool disableiPad1,
										  bool disableiPad2,
										  bool disableiPadMini)
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDeviceBlacklist");
			_KamcordSetDeviceBlacklist(disableiPod4G,
									   disableiPod5G,
									   disableiPhone3GS,
									   disableiPhone4,
									   disableiPad1,
									   disableiPad2,
									   disableiPadMini);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDeviceBlacklist");
		}
	}
	
	public static bool IsEnabled()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _KamcordIsEnabled();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.IsEnabled");
			return false;
		}
	}
	
	public static void Init(string devKey,
						    string devSecret,
						    string appName,
						    DeviceOrientation deviceOrientation,
						    VideoResolution videoResolution)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.Init");
			if (orientationWasPreviouslySet)
			{
				deviceOrientation = Kamcord.GetDeviceOrientation();
				Debug.Log ("Setting device orientation to old orientation: " + deviceOrientation);
			}
			_KamcordInit(devKey, devSecret, appName, deviceOrientation.ToString(), videoResolution.ToString());
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.Init");
		}
	}
	
#if UNITY_IPHONE
	public static Kamcord.DeviceOrientation GetDeviceOrientation()
	{
		string orientation = _KamcordDeviceOrientation();
		Debug.Log ("Kamcord orientation is: " + orientation);
		if (orientation == "Portrait")
		{
			return Kamcord.DeviceOrientation.Portrait;
		} else if (orientation == "LandscapeLeft") {
			return Kamcord.DeviceOrientation.LandscapeLeft;
		} else if (orientation == "LandscapeRight") {
			return Kamcord.DeviceOrientation.LandscapeRight;
		} else if (orientation == "PortraitUpsideDown") {
			return Kamcord.DeviceOrientation.PortraitUpsideDown;
		} else {
			return Kamcord.DeviceOrientation.Portrait;
		}
	}
#endif
	
	public static void SetDeviceOrientation(DeviceOrientation orientation)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDeviceOrientation");
			orientationWasPreviouslySet = true;
			_KamcordSetDeviceOrientation(orientation.ToString());
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDeviceOrientation");
		}
	}
	
	//////////////////////////////////////////////////////////////////
    /// Share settings
    ///
	
	public static void SetDefaultTitle(string title)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDefaultTitle");
			_KamcordSetDefaultTitle(title);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultTitle");
		}
	}
	
	public static void SetFacebookAppID(string facebookAppID)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetFacebookAppID");
			_KamcordSetFacebookAppID(facebookAppID);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetFacebookAppID");
		}
	}
	
	public static void SetFacebookSettings(string title,
                                           string caption,
                                           string description)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetFacebookSettings");
			_KamcordSetFacebookSettings(title, caption, description);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetFacebookSettings");
		}
	}

    public static void SetYouTubeSettings(string description,
                                          string tags)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetYouTubeSettings");
			_KamcordSetYouTubeSettings(description, tags);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetYouTubeSettings");
		}
	}
	
	public static void SetYouTubeVideoCategory(YouTubeVideoCategory category)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetYouTubeVideoCategory");
			_KamcordSetYouTubeVideoCategory(category.ToString());
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetYouTubeSettings");
		}
	}
    
    public static void SetDefaultFacebookMessage(string message)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDefaultFacebookMessage");
			_KamcordSetDefaultFacebookMessage(message);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultFacebookMessage");
		}
	}
	
    public static void SetDefaultTweet(string tweet)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDefaultTweet");
			_KamcordSetDefaultTweet(tweet);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultTweet");
		}
	}
	
	public static void SetTwitterDescription(string twitterDescription)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetTwitterDescription");
			_KamcordSetTwitterDescription(twitterDescription);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultTweet");
		}
	}

		
    public static void SetDefaultYouTubeMessage(string message)
    {
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDefaultYouTubeMessage");
			_KamcordSetDefaultYouTubeMessage(message);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultYouTubeMessage");
		}
	}
		
    public static void SetDefaultEmailSubjectAndBody(string subject,
                                                     string body)
    {
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetDefaultEmailSubjectAndBody");
			_KamcordSetDefaultEmailSubjectAndBody(subject, body);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetDefaultEmailSubjectAndBody");
		}
	}
		
    public static void SetLevelAndScore(string level,
                                        double score)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetLevelAndScore");
			_KamcordSetLevelAndScore(level, score);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetLevelAndScore");
		}
	}
	
	//////////////////////////////////////////////////////////////////
    /// Video Recording
    ///
	
	// Start recording
	public static bool StartRecording()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.StartRecording");
			return _KamcordStartRecording();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.StartRecording");
			return false;
		}
	}
	
	// Stop recording
	public static bool StopRecording()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.StopRecording");
			return _KamcordStopRecording();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.StopRecording");
			return false;
		}
	}
	
	// Pause recording
	public static bool Pause()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.Pause");
			return _KamcordPause();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.Pause");
			return false;
		}
	}
	
	// Resume recording
	public static bool Resume()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.Resume");
			return _KamcordResume();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.Resume");
			return false;
		}
	}
	
	// Are we currently recording?
	public static bool IsRecording()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _KamcordIsRecording();
		}
		else
#endif
		{
			return false;
		}
	}
	
	// HUD-less recording
	public static bool CaptureFrame()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _KamcordCaptureFrame();
		}
		else
#endif
		{
			return false;
		}
	}

	// Enable notifications from Kamcord.
	// By default, we schedule 4 "Gameplay of the Week" notifications every week for 4 weeks.
	public static void SetNotificationsEnabled(bool notificationsEnabled)
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// Debug.Log ("Kamcord.SetNotificationsEnabled");
			_KamcordSetNotificationsEnabled(notificationsEnabled);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetNotificationsEnabled");
		}
	}
	
	// Notify Kamcord of a notification
	public static void HandleKamcordNotification()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// Debug.Log ("Kamcord.HandleKamcordNotification");
			_KamcordHandleNotification();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.HandleKamcordNotification");
		}
	}

	// Fire a test notification.
	public static void FireTestNotification()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// Debug.Log ("Kamcord.FireTestNotification");
			_KamcordFireTestNotification();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.FireTestNotification");
		}
	}

	//////////////////////////////////////////////////////////////////
    /// Subscribe to KamcordCallbackInterface callback
    ///	
	
	// Methods to take care of subscribing and unsubscribing to callbacks
	public static List<KamcordCallbackInterface> listeners = new List<KamcordCallbackInterface>();
	
	// Call this static method to have your object receive all of the
	// KamcordCallbackInterface callbacks.
	public static void AddListener(KamcordCallbackInterface listener)
	{
#if UNITY_IPHONE
		if (!listeners.Contains(listener))
		{
			listeners.Add(listener);
		}
#endif
	}
	
	public static void RemoveListener(KamcordCallbackInterface listener)
	{
#if UNITY_IPHONE
		listeners.Remove(listener);
#endif
	}
	
	//////////////////////////////////////////////////////////////////
    /// UI 
    ///	
	
	// Show Kamcord view
	public static void ShowView()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowView");
			_KamcordShowView();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowView");
		}
	}
	
	public static void ShowWatchView()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowWatchView");
			_KamcordShowWatchView();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowWatchView");
		}
	}
	
    public static void SetShowVideoControlsOnReplay(bool showControls)
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowView");
			_KamcordSetShowVideoControlsOnReplay(showControls);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowView");
		}
	}
    
    public static bool ShowVideoControlsOnReplay()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowView");
			return _KamcordShowVideoControlsOnReplay();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowView");
			return false;
		}
	}
	
	//////////////////////////////////////////////////////////////////
    /// Sundry Methods
    ///
    public static bool CancelConversionForLatestVideo()
    {
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.CancelConversionForLatestVideo");
			return _KamcordCancelConversionForLatestVideo();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.CancelConversionForLatestVideo");
			return false;
		}
	}
		
    public static void SetMaximumVideoLength(uint seconds)
    {
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SetMaximumVideoLength");
			_KamcordSetMaximumVideoLength(seconds);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SetMaximumVideoLength");
		}
	}
	
    public static uint MaximumVideoLength()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.MaximumVideoLength");
			return _KamcordMaximumVideoLength();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.MaximumVideoLength");
			return 0;
		}
	}
	
	
	//////////////////////////////////////////////////////////////////
    /// Subscribe to callbacks from Kamcord
    /// 
	
	public static void SubscribeToCallbacks(bool subscribe)
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.SubscribeToCallbacks");
			_KamcordSubscribeToCallbacks(subscribe);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.SubscribeToCallbacks");
		}
	}
	
	public static void Disable()
	{
#if UNITY_IPHONE
		// Call plugin only when running on real device
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.Disable");
			_KamcordDisable();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.Disable");
		}
	}
	
#if UNITY_IPHONE
	public static int GetNumChannelsFromSpeakerMode(AudioSpeakerMode speakerMode)
	{
		switch (AudioSettings.speakerMode)
		{
		case AudioSpeakerMode.Mono:
			return 1;
			
		case AudioSpeakerMode.Stereo:
			return 2;
			
		case AudioSpeakerMode.Quad:
			return 4;
			
		case AudioSpeakerMode.Surround:
		case AudioSpeakerMode.Mode5point1:
			return 5;
			
		case AudioSpeakerMode.Mode7point1:
			return 7;
		
		case AudioSpeakerMode.Prologic:
			return 2;
			
		case AudioSpeakerMode.Raw:
		default:
			return 2;
		}
	}
#endif
	
	
	//////////////////////////////////////////////////////////////////
    /// Custom Sharing UI
    /// 
    
	/*
	public static void PresentVideoPlayerFullscreen()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.PresentVideoPlayerFullscreen");
			_KamcordPresentVideoPlayerFullscreen();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.PresentVideoPlayerFullscreen");
		}
	}
	*/
    
    // TODO: setShareDelegate
    //       shareDelegate
	
    public static void ShowFacebookLoginView()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowFacebookLoginView");
			_KamcordShowFacebookLoginView();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowFacebookLoginView");
		}
	}
    
    public static void ShowTwitterAuthentication()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShowTwitterAuthentication");
			_KamcordShowTwitterAuthentication();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShowTwitterAuthentication");
		}
	}
	
	// TODO:
    // public static void ShowYouTubeLoginViewInViewController(UIViewController * parentViewController);
    
    public static bool FacebookIsAuthenticated()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.FacebookIsAuthenticated");
			return _KamcordFacebookIsAuthenticated();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.FacebookIsAuthenticated");
			return false;
		}
	}
    
    public static bool TwitterIsAuthenticated()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.TwitterIsAuthenticated");
			return _KamcordTwitterIsAuthenticated();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.TwitterIsAuthenticated");
			return false;
		}
	}
    
    public static bool YouTubeIsAuthenticated()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.YouTubeIsAuthenticated");
			return _KamcordYouTubeIsAuthenticated();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.YouTubeIsAuthenticated");
			return false;
		}
	}
    
	public static void PerformFacebookLogout()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.PerformFacebookLogout");
			_KamcordPerformFacebookLogout();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.PerformFacebookLogout");
		}
	}
    
    public static void PerformYouTubeLogout()
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.PerformYouTubeLogout");
			_KamcordPerformYouTubeLogout();
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.PerformYouTubeLogout");
		}
	}
    
    public static bool ShareVideoWithMessage(string message,
                                             bool shareOnFacebook,
                                             bool shareOnTwitter,
                                             bool shareOnYouTube)
	{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log ("Kamcord.ShareVideoWithMessage");
			return _KamcordShareVideoWithMessage(message, shareOnFacebook, shareOnTwitter, shareOnYouTube);
		}
		else
#endif
		{
			Debug.Log ("[NOT CALLED] Kamcord.ShareVideoWithMessage");
			return false;
		}
	}
}


