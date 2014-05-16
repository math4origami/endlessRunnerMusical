using UnityEngine;
using System.Collections;


public class PlayerBehaviorScript : MonoBehaviour {

	float swipeSpeedThreshold = 30000.0f;
	float swipeDirectionThreshold = 100.0f;
	Vector2 swipeStart = new Vector2(0, 0);
	Vector2 swipeEnd = new Vector2(0, 0);
	bool swipeWasActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			processSwipe();
		}

//		if(Input.GetButton("Fire1")) {
//			TapFunction();
//		}
	}

	void OnGUI () {
		processSwipe ();
	}


	void processSwipe () {
		if ( Input.touchCount != 1 ) {
			return;
		}

		Touch currentTouch = Input.touches [0];

		if ( currentTouch.deltaPosition == Vector2.zero ) {
			return;
		}

		Vector2 speedVector = currentTouch.deltaPosition / currentTouch.deltaTime;
		float swipeSpeed = speedVector.magnitude;

		bool swipeIsActive = (swipeSpeed > swipeSpeedThreshold);

		if (swipeIsActive) {
			//Swipe achieved
			if (!swipeWasActive) {
				//There was no swipe active at this time
				//Set the start position, since the swipe is now officially active
				swipeStart = currentTouch.position;
			}
		}
		else {
			//The user's not moving touch fast enough to count as swiping, so end the swipe here
			swipeEnd = currentTouch.position;
			swipeTypeLogic();
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

		if (Mathf.Abs(xDifference) >= swipeDirectionThreshold) {
			//Difference > 0 = left swipe, Difference < 0 = right swipe
			if (xDifference < 0) {
				textGUI2.text = "Left Swipe!";
			}
			else if (xDifference > 0) {
				textGUI2.text = "Right Swipe!";
			}
		}
		else {
			textGUI2.text = "Horizontal Swipe Failed";
		}

		if (Mathf.Abs(yDifference) >= swipeDirectionThreshold) {
			//Difference > 0 = up swipe, Difference < 0 = down swipe
			if (yDifference < 0) {
				textGUI.text = "Down Swipe!";
			}
			else if (yDifference > 0) {
				textGUI.text = "Up Swipe!";
			}
		}
		else {
			textGUI.text = "Vertical Swipe Failed";
		}
	}

	void TapFunction() {
		//Respond to tap event
		//Note that "TapFunction" can be called anything you like.
		//You could also omit TapFunction(), and just respond inside of Update(). But putting the response in a function is usually the way to go.
		Debug.Log ("This object will be tapping LOLZ");
	}
}
