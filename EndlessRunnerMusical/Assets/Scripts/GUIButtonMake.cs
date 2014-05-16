using UnityEngine;
using System.Collections;

public class GUIButtonMake : MonoBehaviour {

	public GUISkin customSkin;
	public string buttonText;
	public string objectName;
	private GameObject actualObject;
	public string scriptFunction;
	public int posX;
	public int posY;
	public int sizeX;
	public int sizeY;

	// Use this for initialization
	void Start () {
		actualObject = GameObject.Find(objectName);
	}

	void OnGUI() {
		if (GUI.Button(new Rect(posX, posY, sizeX, sizeY), buttonText, customSkin.button)) {
			if (actualObject) {
				//Do something
				actualObject.SendMessage(scriptFunction);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
