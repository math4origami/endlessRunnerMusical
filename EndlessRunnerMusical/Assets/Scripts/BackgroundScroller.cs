using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public GameObject background;
	public float scrollSpeed;
	private GameObject bgCopy1;
	private GameObject bgCopy2;
	private float bgWidth;
	private float bgHeight;
	private float currentOffset;
	private float offsetX;
	private float offsetY;

	// Use this for initialization
	void Start () {
		offsetX = -Screen.width / 2.0f;
		offsetY = -Screen.height / 2.0f;
		bgWidth = background.guiTexture.pixelInset.width;
		bgHeight = background.guiTexture.pixelInset.height;
		bgCopy1 = Instantiate(background) as GameObject;
		bgCopy1.guiTexture.pixelInset = new Rect (offsetX, offsetY, bgWidth, bgHeight);
		bgCopy2 = Instantiate(background) as GameObject;
		bgCopy2.guiTexture.pixelInset = new Rect (offsetX + bgWidth, offsetY, bgWidth, bgHeight);

		currentOffset = 0;
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
		bgCopy1.guiTexture.pixelInset = new Rect (-currentOffset + offsetX, offsetY, bgWidth, bgHeight);
		bgCopy2.guiTexture.pixelInset = new Rect (-currentOffset + bgWidth + offsetX, offsetY, bgWidth, bgHeight);
	}
}
