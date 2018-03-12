using System.Collections;
using UnityEngine;
using UnityEditor;

public class NewDialogWriter : MonoBehaviour
{

	[MenuItem ("GameObject/DialogWriter")]
	public static void CreateDialogWriter ()
	{
		GameObject go = new GameObject ("Dialog Writer");
		MonoDialogWriter monoDialogWriter = go.AddComponent<MonoDialogWriter> ();
		monoDialogWriter.Init (new DialogWriter ());
	}
}
