using UnityEngine;
using UnityEngine.UI;

public class ExampleScript : MonoBehaviour {

	public Slider slider;
	public ScrollRect scrollRect;

	void Awake(){
		//Set callback method like this to add more functions to listener
		slider.onValueChanged.AddListener (delegate {SliderTaptic ();});
		//Set callback like this if no other functions needed
		scrollRect.onValueChanged.AddListener (delegate {TapticEngine.TriggerSelectionChange ();});
	}

	//Listener
	private void SliderTaptic(){
		TapticEngine.TriggerSelectionChange();
		Debug.Log("Slider triggered Selection Change");
	}
	
	//Methods called by buttons OnClick
	public void Warning(){
		Debug.Log("Triggered Warning Haptic");
		TapticEngine.TriggerWarning();
	}
	public void Error(){
		Debug.Log("Triggered Error Haptic");
		TapticEngine.TriggerError();
	}
	public void Success(){
		Debug.Log("Triggered Success Haptic");
		TapticEngine.TriggerSuccess();
	}
	public void Light(){
		Debug.Log("Triggered Light Haptic");
		TapticEngine.TriggerLight();
	}
	public void Medium(){
		Debug.Log("Triggered Medium Haptic");
		TapticEngine.TriggerMedium();
	}
	public void Heavy(){
		Debug.Log("Triggered Heavy Haptic");
		TapticEngine.TriggerHeavy();
	}
	public void SelectionChange(){
		Debug.Log("Triggered Selection Change Haptic");
		TapticEngine.TriggerSelectionChange();
	}
}
