﻿using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Wall_Tile : Scriptable_Tile {

	public override int GetIndex(byte mask){
		switch(mask){
			case 0: return 0; // 0 neighbours
			case 6: return 1; // neighbours at right and bottom
			case 14: return 2; // neighbours at left, right and bottom
			case 12: return 3; // neighbours at left and bottom
			case 7: return 4; // neighbours at right, top and bottom
			case 13: return 5; // neighbours at left, top and bottom
			case 3: return 6; // neighbours at right and top
			case 11: return 7; // neighbours at right, left, and top
			case 9: return 8; // neighbours at left and top
			case 15: return 9; // neighbours at right, left, top, and bottom
			case 5: return 10; // neighbours at top and bottom
			case 10: return 11; // neighbours at left and right
		}
		return -1;
	}

	#if UNITY_EDITOR
		[MenuItem("Assets/Scriptable Tiles/Wall_Tile")]
		public static void CreateScriptableTile(){
			string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
			if(path == ""){
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Wall_Tile>(), path);
		}
	#endif

}