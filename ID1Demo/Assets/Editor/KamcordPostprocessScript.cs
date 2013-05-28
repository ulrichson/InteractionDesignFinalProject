#if UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using System;
using System.Diagnostics;

public class KamcordPostprocessScript : MonoBehaviour
{

	// Replaces PostprocessBuildPlayer functionality
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
	{
		UnityEngine.Debug.Log ("--- Kamcord --- Executing post process build phase.");
		
		Process p = new Process();
        p.StartInfo.FileName = "perl";
        p.StartInfo.Arguments = string.Format("Assets/Editor/KamcordPostprocessbuildPlayer1 \"{0}\"", pathToBuildProject);
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = false;
        p.Start();
		
        p.WaitForExit();
		
		UnityEngine.Debug.Log("--- Kamcord --- Finished executing post process build phase."); 
	}
}

#endif
