using UnityEngine;
using System.Collections;

public class PlayerBehaviorScript : MonoBehaviour {

	public float swipeSpeedThreshold;
	public float swipeDirectionThreshold;

	Vector2 swipeStart = new Vector2(0, 0);
	Vector2 swipeEnd = new Vector2(0, 0);
	bool swipeWasActive = false;
	bool swipeXRegistered = false;
	bool swipeYRegistered = false;

	Vector3 lastMousePosition = new Vector3();
	bool wasTouched = false;
	bool wasClicked = false;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			processTouch();
			if (!wasTouched) {
				TapFunction();
			}
			wasTouched = true;
		} else if (wasTouched) {
			clearSwipe();
			wasTouched = false;
		}

		if (Input.GetMouseButton(0)) {
			processClick();
			if (!wasClicked) {
				TapFunction();
			}
			wasClicked = true;
		} else if (wasClicked) {
			clearSwipe();
			wasClicked = false;
		}
	}

	void clearSwipe() {
		swipeWasActive = false;
		swipeXRegistered = false;
		swipeYRegistered = false;
	}

	void processTouch () {
		Touch currentTouch = Input.touches [0];

		if ( currentTouch.deltaPosition == Vector2.zero ) {
			return;
		}

		Vector2 speedVector = currentTouch.deltaPosition / currentTouch.deltaTime;

		processSwipe(speedVector.magnitude, currentTouch.position);
	}

	void processClick() {
		Vector3 mousePosition = Input.mousePosition;

		Vector3 deltaPosition = mousePosition - lastMousePosition;
		Vector3 speedVector = deltaPosition / Time.deltaTime;
		processSwipe(speedVector.magnitude, new Vector2(mousePosition.x, mousePosition.y));

		lastMousePosition = mousePosition;
	}

	void processSwipe(float swipeSpeed, Vector2 position) {
		bool swipeIsActive = swipeSpeed > swipeSpeedThreshold;

		if (swipeIsActive) {
			//Swipe achieved
			if (!swipeWasActive) {
				//There was no swipe active at this time
				//Set the start position, since the swipe is now officially active
				swipeStart = position;
			}
			swipeEnd = position;
			swipeTypeLogic();
		} else {
			swipeXRegistered = false;
			swipeYRegistered = false;
		}
		
		//update active swipe boolean
		swipeWasActive = swipeIsActive;
	}

	void swipeTypeLogic () {

		//Runs when a swipe has ended
		float xDifference = swipeEnd.x - swipeStart.x;
		float yDifference = swipeEnd.y - swipeStart.y;

		GUIText textGUI = GameObject.Find("SwipeDebug").guiText;
		GUIText textGUI2 = GameObject.Find("SwipeDebug2").guiText;

		if (Mathf.Abs(xDifference) >= swipeDirectionThreshold && !swipeXRegistered) {
			//Difference > 0 = left swipe, Difference < 0 = right swipe
			if (xDifference < 0) {
				textGUI2.text = "Left Swipe! ";
			}
			else if (xDifference > 0) {
				textGUI2.text = "Right Swipe!";
			}
			swipeXRegistered = true;
		}

		if (Mathf.Abs(yDifference) >= swipeDirectionThreshold && !swipeYRegistered) {
			//Difference > 0 = up swipe, Difference < 0 = down swipe
			if (yDifference < 0) {
				textGUI.text = "Down Swipe!";
			}
			else if (yDifference > 0) {
				textGUI.text = "Up Swipe!";
			}
			swipeYRegistered = true;
		}
	}

	void TapFunction() {
		//Respond to tap event
		//Note that "TapFunction" can be called anything you like.
		//You could also omit TapFunction(), and just respond inside of Update(). But putting the response in a function is usually the way to go.
		Debug.Log ("This object will be tapping LOLZ");
	}
}
