using UnityEngine;
using System.Collections;

public class WinLevelScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().Play();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void startLevel () {
		Application.LoadLevel("Running");
	}
}
