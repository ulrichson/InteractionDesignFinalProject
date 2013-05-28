using UnityEngine;
using System.Collections;

public class RecordingGUI : MonoBehaviour
{	
	public Font buttonFont;

	private bool firstVideoRecorded;
	private Rect recordingButtonRect;
	private Rect showViewButtonRect;

	void Start()
	{
		firstVideoRecorded = false;
		recordingButtonRect = new Rect(20, 20, 200, 60);
		showViewButtonRect = new Rect(recordingButtonRect.x,
									  (2*recordingButtonRect.y) + recordingButtonRect.height,
									  recordingButtonRect.width,
									  recordingButtonRect.height);
	}

	void OnGUI()
	{
		GUI.skin.button.font = this.buttonFont;

		if (Kamcord.IsRecording())
		{
			firstVideoRecorded = true;
			if (GUI.Button(recordingButtonRect, "Stop Recording"))
			{
				Kamcord.StopRecording();
			}
		} else if (Kamcord.IsEnabled()) {
			if (GUI.Button(recordingButtonRect, "Start Recording"))
			{
				Kamcord.StartRecording();
			}

			if (firstVideoRecorded)
			{
				if (GUI.Button(showViewButtonRect, "Show Last Video"))
				{
					Kamcord.ShowView();
				}
			}
		}
	}
}
