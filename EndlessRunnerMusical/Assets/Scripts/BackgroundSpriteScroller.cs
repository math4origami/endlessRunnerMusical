using UnityEngine;
using System.Collections;

public class BackgroundSpriteScroller : MonoBehaviour {

	public GameObject sprite;
	private GameObject sprite1;
	private GameObject sprite2;
	private float bgWidth;
	private float bgHeight;
	public float scrollSpeed;
	private float currentOffset;

	// Use this for initialization
	void Start () {

		sprite1 = Instantiate(sprite) as GameObject;
		sprite2 = Instantiate(sprite) as GameObject;

		resizeBGToFitY(sprite1);
		resizeBGToFitY(sprite2);

		currentOffset = 0;

		Debug.Log (bgWidth);

		sprite1.transform.position = new Vector3 (0, 0, 0);
		sprite2.transform.position = new Vector3 (bgWidth, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		currentOffset = currentOffset + scrollSpeed * Time.deltaTime;
		
		if (currentOffset > bgWidth) {
			currentOffset = currentOffset % bgWidth;
		}
		
		positionBasedOnOffset();
	}
	
	void positionBasedOnOffset() {
		sprite1.transform.position = new Vector3 ( -currentOffset, 0, 0);
		sprite2.transform.position = new Vector3 (bgWidth - currentOffset, 0, 0);
	}

	void resizeBGToFitY(GameObject currentSprite) {
		SpriteRenderer spriteRenderer = (SpriteRenderer)currentSprite.GetComponent(typeof(SpriteRenderer));
		if (spriteRenderer == null) {
			return;
		}

		float width = spriteRenderer.sprite.bounds.size.x;
		float height = spriteRenderer.sprite.bounds.size.y;
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float scale = worldScreenHeight / height;

		bgWidth = width * scale;
		bgHeight = height * scale;

		currentSprite.transform.localScale = new Vector3(scale, scale, 1.0f);
	}
}
