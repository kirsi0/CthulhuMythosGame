using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewTileMapMenu
{

	[MenuItem ("GameObject/Tdddile Map")]
	public static void CreateTileMap ()
	{
		GameObject go = new GameObject ("Tile Map");
		go.AddComponent<TileMap> ();
	}
}
