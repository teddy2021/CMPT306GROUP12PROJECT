using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ground_Tile : Scriptable_Tile{


    private int GetIndex(byte mask){
        return 0;
    }
	#if UNITY_EDITOR
	[MenuItem("Assets/Scriptable Tiles/Ground_Tile")]
    	public static void CreateScriptableTile(){
		string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
		if(path == ""){
			return;
		}
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Ground_Tile>(), path);
	}
	#endif
}
