using UnityEngine;
using System.Collections;

public class RedView : MonoBehaviour {
	
	private float duration = 0.5f;
	private float start;
	private bool flashing;

	// Use this for initialization
	void Start () {
		Color c = GetComponent<SpriteRenderer>().material.color;
		c.a = 0;
		GetComponent<SpriteRenderer>().material.color = c;
		GetComponent<SpriteRenderer>().sortingLayerName = "Overlay";
	}
	
	// Update is called once per frame
	void Update () {
		if (flashing) {
			if (Time.time > start + duration) {
				flashing = false;
			}

			Color c = GetComponent<SpriteRenderer>().material.color;
			c.a = Mathf.Clamp(1 - (Time.time - start) / duration, 0, 1);
			GetComponent<SpriteRenderer>().material.color = c;
		}
	}

	public void Flash() {
		flashing = true;
		start = Time.time;
	}
}
