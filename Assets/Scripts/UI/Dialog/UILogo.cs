using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using DG.Tweening;

public class UILogo : UIDialog
{
	private Image background;
	private Color color = new Color (255, 255, 255, 0);
	private float time = 2.0f;
	// Use this for initialization
	void Start ()
	{
		background = transform.Find ("Logo").GetComponent<Image> ();
		background.color = color;

		background.DOFade (1.0f, time).OnComplete (
			() => {
				Skyunion.UIManager.Instance ().CloseUI ();

				Skyunion.UIManager.Instance ().ShowPanel<UIMainMenuPanel> ();

			});
		//background.color.DOFlip();
	}

	// Update is called once per frame
	void Update ()
	{
		//if (color.a < 1.0f)
		//{
		//    color.a += Time.deltaTime * 1.0f / time;
		//    background.color = color;
		//}
		//else
		//{
		//    Debug.Log("UILogoShow");
		//    enabled = false;
		//    //UIManager.Instance().ShowUI<UILoading>();
		//}
	}
}