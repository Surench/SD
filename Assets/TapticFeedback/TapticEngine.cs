using UnityEngine;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TapticEngine : MonoBehaviour {

	[DllImport ("__Internal")]
	private static extern void _PlayTaptic(string type);

	public static void TriggerWarning(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("warning");
	}
	public static void TriggerError(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("error");
	}
	public static void TriggerSuccess(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("success");
	}
	public static void TriggerLight(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("light");
	}
	public static void TriggerMedium(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("medium");
	}
	public static void TriggerHeavy(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("heavy");
	}
	public static void TriggerSelectionChange(){
	#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
	#endif
		_PlayTaptic("selectionChange");
	}
}
