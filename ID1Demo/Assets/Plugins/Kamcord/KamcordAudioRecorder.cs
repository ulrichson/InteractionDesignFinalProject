using UnityEngine;

public class KamcordAudioRecorder : MonoBehaviour
{
#if UNITY_IPHONE
	void OnAudioFilterRead(float [] data, int numChannels)
	{
		if (Kamcord.IsRecording())
		{
			Kamcord.WriteAudioData(data, data.Length, numChannels);
		}
	}
#endif
}
