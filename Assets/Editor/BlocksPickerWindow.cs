﻿using System.Collections;
using UnityEditor;
using UnityEngine;

//获取voxelMap中储存的方块数据，提供block的选择，返回blockID
public class BlocksPickerWindow : EditorWindow
{
	Editor gameObjectEditor;
	//图片的尺寸
	const int imgSize = 100;
	//显示数据的范围
	const int columnWidth = 500;
	//图片的左、右、下的偏移
	const int offset = 20;

	//滚动条的位置
	Vector2 scrollPosition = Vector2.zero;

	BlockType blockType;

	VoxelMap voxelmap;
	VoxelCharacter voxelCharacter;

	[MenuItem ("Window/Blcok Picker")]
	public static void OpenBlockPickerWindow ()
	{
		EditorWindow window = EditorWindow.GetWindow (typeof (BlocksPickerWindow));
		GUIContent title = new GUIContent ();
		title.text = "Block Picker";
		window.titleContent = title;
	}

	private void OnGUI ()
	{
		if (Selection.activeGameObject == null)
			return;


		if (Selection.activeGameObject.GetComponent<VoxelMap> () != null) {
			voxelCharacter = null;
			voxelmap = Selection.activeGameObject.GetComponent<VoxelMap> ();
			//只会显示一组
		} else {
			voxelmap = null;
			voxelCharacter = Selection.activeGameObject.GetComponent<VoxelCharacter> ();
		}
		if (voxelmap != null) {
			//窗口在屏幕中的位置，GUI定位与窗口没有直接关系
			Rect viewPort = new Rect (0, 0, position.width - 5, position.height - 5);
			//内容物的尺寸，长度是两个偏移加上图片大小和说明大小
			var contentSize = new Rect (0, 0, columnWidth + offset * 2 + imgSize, (imgSize + offset) * voxelmap.allBlocks.Count);
			//开始滚动GUI
			scrollPosition = GUI.BeginScrollView (viewPort, scrollPosition, contentSize);

			for (int i = 0; i < voxelmap.allBlocks.Count; i++) {
				if (voxelmap.allBlocks [i].m_gameobject == null) {
					EditorGUILayout.HelpBox ("你缺少对GameObject(序号 " + i + "）的赋值\nyou loss the reference of gameobject " + i, MessageType.Warning);
					continue;
				}
				//绘制缩略图
				EditorGUI.DrawPreviewTexture (new Rect (offset, i * (imgSize + offset), imgSize, imgSize), AssetPreview.GetAssetPreview (voxelmap.allBlocks [i].m_gameobject));
				GUILayout.BeginArea (new Rect (offset * 2 + imgSize, i * (imgSize + offset), columnWidth, imgSize));

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.ObjectField (
					voxelmap.allBlocks [i].m_name + " " + voxelmap.allBlocks [i].BlockType,
					voxelmap.allBlocks [i].m_gameobject,
					typeof (GameObject),
					false
				);
				voxelmap.allBlocks [i].m_tag = EditorGUILayout.TagField (voxelmap.allBlocks [i].m_tag);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("当前方块类型：" + voxelmap.allBlocks [i].BlockType);
				blockType = (BlockType)EditorGUILayout.EnumPopup ("重新选择方块类型：", blockType);
				if (GUILayout.Button ("确定修改")) {
					voxelmap.allBlocks [i].BlockType = blockType;
					Debug.Log ("change block" + voxelmap.allBlocks [i].m_name + " type to " + voxelmap.allBlocks [i].BlockType);
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("请注意，重新打开Unity会导致类型失效，请重新手动确保当前方块类型正确");
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				//获取单一方块的网格数据
				Vector3 blockSize = voxelmap.allBlocks [i].m_gameobject.GetComponentInChildren<MeshFilter> ().sharedMesh.bounds.size;
				EditorGUILayout.LabelField ("Size : " + blockSize.x.ToString ()
													+ " x " + blockSize.y.ToString ()
													+ " x " + blockSize.z.ToString ());
				EditorGUILayout.EndHorizontal ();
				GUILayout.EndArea ();
			}
			//绘制颜色
			Texture2D boxTex = new Texture2D (1, 1);
			boxTex.SetPixel (0, 0, new Color (0, 0.5f, 1f, 0.4f));
			boxTex.Apply ();
			//加入自定义皮肤
			var style = new GUIStyle (GUI.skin.customStyles [0]);
			style.normal.background = boxTex;
			//绘制被选中的显示
			if (voxelmap.blockID != -1) {
				GUI.Box (new Rect (imgSize + offset * 2, voxelmap.blockID * (imgSize + offset), columnWidth, imgSize + offset), "", style);
			}


			//鼠标点击确定选中的方块ID
			Event cEvent = Event.current;
			Vector2 mousePos = new Vector2 (cEvent.mousePosition.x, cEvent.mousePosition.y);
			if (cEvent.type == EventType.MouseDown && cEvent.button == 0) {
				int order = (int)Mathf.Floor (mousePos.y / (imgSize + offset));
				if (order <= voxelmap.allBlocks.Count) {

					voxelmap.blockID = order;
				} else {
					voxelmap.blockID = -1;
				}

				Debug.Log ("selection.blockID : " + voxelmap.blockID);
				Repaint ();
			}

			GUI.EndScrollView ();
		}

		if (voxelCharacter != null) {
			//窗口在屏幕中的位置，GUI定位与窗口没有直接关系
			Rect viewPort = new Rect (0, 0, position.width - 5, position.height - 5);
			//内容物的尺寸，长度是两个偏移加上图片大小和说明大小
			var contentSize = new Rect (0, 0, columnWidth + offset * 2 + imgSize, (imgSize + offset) * voxelCharacter.allCharacter.Count);
			//开始滚动GUI
			scrollPosition = GUI.BeginScrollView (viewPort, scrollPosition, contentSize);

			for (int i = 0; i < voxelCharacter.allCharacter.Count; i++) {
				if (voxelCharacter.allCharacter [i].m_gameobject == null) {
					EditorGUILayout.HelpBox ("你缺少对GameObject(序号 " + i + "）的赋值\nyou loss the reference of gameobject " + i, MessageType.Warning);
					continue;
				}
				//绘制缩略图
				EditorGUI.DrawPreviewTexture (new Rect (offset, i * (imgSize + offset), imgSize, imgSize), AssetPreview.GetAssetPreview (voxelCharacter.allCharacter [i].m_gameobject));
				GUILayout.BeginArea (new Rect (offset * 2 + imgSize, i * (imgSize + offset), columnWidth, imgSize));

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.ObjectField (
					voxelCharacter.allCharacter [i].m_name + " " + voxelCharacter.allCharacter [i].BlockType,
					voxelCharacter.allCharacter [i].m_gameobject,
					typeof (GameObject),
					false
				);
				voxelCharacter.allCharacter [i].m_tag = EditorGUILayout.TagField (voxelCharacter.allCharacter [i].m_tag);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("当前方块类型：" + voxelCharacter.allCharacter [i].BlockType);
				blockType = (BlockType)EditorGUILayout.EnumPopup ("重新选择方块类型：", blockType);
				if (GUILayout.Button ("确定修改")) {
					voxelCharacter.allCharacter [i].BlockType = blockType;
					Debug.Log ("change block" + voxelCharacter.allCharacter [i].m_name + " type to " + voxelCharacter.allCharacter [i].BlockType);
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("请注意，重新打开Unity会导致类型失效，请重新手动确保当前方块类型正确");
				EditorGUILayout.EndHorizontal ();

				//EditorGUILayout.BeginHorizontal ();
				//获取单一方块的网格数据
				//Vector3 blockSize = voxelmap.allBlocks [i].m_gameobject.GetComponentInChildren<MeshFilter> ().sharedMesh.bounds.size;
				//EditorGUILayout.LabelField ("Size : " + blockSize.x.ToString ()
				//+ " x " + blockSize.y.ToString ()
				//+ " x " + blockSize.z.ToString ());
				//EditorGUILayout.EndHorizontal ();
				GUILayout.EndArea ();
			}
			//绘制颜色
			Texture2D boxTex = new Texture2D (1, 1);
			boxTex.SetPixel (0, 0, new Color (0, 0.5f, 1f, 0.4f));
			boxTex.Apply ();
			//加入自定义皮肤
			var style = new GUIStyle (GUI.skin.customStyles [0]);
			style.normal.background = boxTex;
			//绘制被选中的显示
			if (voxelCharacter.characterID != -1) {
				GUI.Box (new Rect (imgSize + offset * 2, voxelCharacter.characterID * (imgSize + offset), columnWidth, imgSize + offset), "", style);
			}


			//鼠标点击确定选中的方块ID
			Event cEvent = Event.current;
			Vector2 mousePos = new Vector2 (cEvent.mousePosition.x, cEvent.mousePosition.y);
			if (cEvent.type == EventType.MouseDown && cEvent.button == 0) {
				int order = (int)Mathf.Floor (mousePos.y / (imgSize + offset));
				if (order <= voxelCharacter.allCharacter.Count) {

					voxelCharacter.characterID = order;
				} else {
					voxelCharacter.characterID = -1;
				}

				Debug.Log ("selection.characterID : " + voxelCharacter.characterID);
				Repaint ();
			}

			GUI.EndScrollView ();
		}

	}

}


