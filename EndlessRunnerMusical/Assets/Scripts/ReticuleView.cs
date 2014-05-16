using UnityEngine;
using System.Collections;

public class ReticuleView : MonoBehaviour {

	public float baseScale;
	public float maxScale;
	public float baseAlpha;
	public float maxAlpha;
	public float alphaDistance;
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		ScoreController score = GameObject.Find("Score").GetComponent<ScoreController>();

		ScoreNote note = score.getNote(gc.gameTime(), true);
		float alpha = baseAlpha;
		Color color = GetComponent<SpriteRenderer>().color;
		float scale = baseScale;

		if (note != null) {
			float interval = note.scriptNote.interval(gc.gameTime());

			if (interval < alphaDistance) {
				float interpolate = (alphaDistance - interval) / alphaDistance;
				alpha += (maxAlpha - baseAlpha) * interpolate;
				scale += (maxScale - baseScale) * interpolate;
			}
		}

		color.a = alpha;
		GetComponent<SpriteRenderer>().color = color;
		transform.localScale = Vector3.one * scale;

		float angle = gc.gameTime() * rotationSpeed;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
