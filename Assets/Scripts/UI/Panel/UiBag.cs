using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBag : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void Clear ()
	{

	}

	public void AddClick (BasicEntity entity)
	{
		List<ItemType> m_item = entity.GetComponent<ItemComponent> ().item;
		for (int i = 0; i < m_item.Count; i++) {
			ItemType item = m_item [i];
			GameObject bag = new GameObject (((int)item).ToString ());

			bag.transform.parent = transform;
			Button button = bag.AddComponent<Button> ();
			ItemButton itemButton = bag.AddComponent<ItemButton> ();
			switch (item) {
			case ItemType.Bottle: {
					bag.AddComponent<Image> ().sprite = Resources.Load<Sprite> ("Bottle");
					break;
				}
			case ItemType.Bomb: {
					bag.AddComponent<Image> ().sprite = Resources.Load<Sprite> ("Bomb");
					break;
				}
			case ItemType.HealthPotion: {
					bag.AddComponent<Image> ().sprite = Resources.Load<Sprite> ("HealthPotion");
					break;
				}
			}
			button.onClick.AddListener (delegate () { itemButton.OnClick (); });
			bag.transform.localScale = Vector3.one;

		}
	}
}
