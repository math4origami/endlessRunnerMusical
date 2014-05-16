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
		ClearAnimations();

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

		if (Mathf.Abs(xDifference) >= swipeDirectionThreshold && !swipeXRegistered) {
			//Difference > 0 = left swipe, Difference < 0 = right swipe
			if (xDifference < 0) {
				HandleInput(NoteType.LEFT);
			}
			else if (xDifference > 0) {
				HandleInput(NoteType.RIGHT);
			}
			swipeXRegistered = true;
		}

		if (Mathf.Abs(yDifference) >= swipeDirectionThreshold && !swipeYRegistered) {
			//Difference > 0 = up swipe, Difference < 0 = down swipe
			if (yDifference < 0) {
				HandleInput(NoteType.DOWN);
			}
			else if (yDifference > 0) {
				HandleInput(NoteType.UP);
			}
			swipeYRegistered = true;
		}
	}

	void TapFunction() {
		//Respond to tap event
		HandleInput(NoteType.TAP);
	}

	void ClearAnimations() {
		GetComponentInChildren<Animator>().SetBool("swipeDown", false);
		GetComponentInChildren<Animator>().SetBool("swipeUp", false);
		GetComponentInChildren<Animator>().SetBool("swipeLeft", false);
		GetComponentInChildren<Animator>().SetBool("swipeRight", false);
		GetComponentInChildren<Animator>().SetBool("attackTap", false);
		GetComponentInChildren<Animator>().SetBool("hurt", false);
	}

	void HandleInput(NoteType direction) {
		GameObject.Find("GameController").GetComponent<GameController>().HandleInput(direction);

		switch (direction) {
		case NoteType.DOWN:
			GetComponentInChildren<Animator>().SetBool("swipeDown", true);
			break;
		case NoteType.UP:
			GetComponentInChildren<Animator>().SetBool("swipeUp", true);
			break;
		case NoteType.RIGHT:
			GetComponentInChildren<Animator>().SetBool("swipeRight", true);
			break;
		case NoteType.LEFT:
			GetComponentInChildren<Animator>().SetBool("swipeLeft", true);
			break;
		case NoteType.TAP:
			GetComponentInChildren<Animator>().SetBool("attackTap", true);
			break;
		}
	}

	public void HandleHurt() {
		GetComponentInChildren<Animator>().SetBool("hurt", true);
	}
}
