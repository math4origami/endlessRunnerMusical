using UnityEngine;
using System.Collections;

public class TitleScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startLevel() {
		Application.LoadLevel("Running");
	}
}
