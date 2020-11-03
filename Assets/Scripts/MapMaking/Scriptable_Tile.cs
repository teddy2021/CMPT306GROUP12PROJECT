using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Wall_Tile : Tile {

	public Sprite[] sprites;
	public Sprite preview;

	public override void RefreshTile(Vector3Int location, ITilemap tilemap){
		for(int y = -1; y <= 1; y += 1){
			for(int x = -1; x <= 1; x += 1){
				Vector3Int position = new Vector3Int(location.x + x, location.y + y, location.z);
				if(HasTile(tilemap, position)){
					tilemap.RefreshTile(position);
				}
			}
		}
	}

	private bool HasTile(ITilemap tilemap, Vector3Int position){
		return tilemap.GetTile(position) == this;
	}

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
		int mask = HasTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 2 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 4 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 8 : 0;
		int index = GetIndex((byte)mask);
		if(index >= 0 & index < sprites.Length){
			tileData.sprite = sprites[index];
			tileData.color = Color.white;
			tileData.flags = TileFlags.LockTransform;
			tileData.colliderType = ColliderType.None;
		}
	}

	private int GetIndex(byte mask){
		switch(mask){
			case 0: return 0;
			case 6: return 1;
			case 14: return 2;
			case 12: return 3;
			case 7: return 4;
			case 13: return 5;
			case 3: return 6;
			case 11: return 7;
			case 9: return 8;
			case 15: return 0;
		}
		return -1;
	}

	#if UNITY_EDITOR
		[MenuItem("Assets/Wall_Tile")]
		public static void CreateScriptableTile(){
			string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
			if(path == ""){
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Scriptable_Tile>(), path);
		}
	#endif

}