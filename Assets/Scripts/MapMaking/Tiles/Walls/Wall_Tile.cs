using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Scriptable_Tile : Tile {

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
		return tilemap.GetTile(position) != null;
	}

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
		int mask = HasTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 2 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 4 : 0;
		mask += HasTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 8 : 0;
		int index = GetIndex((byte)mask);
		if(index >= 0 & index < sprites.Length){
			tileData.sprite = sprites[index];
			tileData.flags = TileFlags.LockTransform;
			tileData.colliderType = ColliderType.None;
		}
		if(index == 15){
			tileData.sprite = sprites[5];
		}
		else if(index == 16){
			tileData.sprite = sprites[8];
		}
		else if(index == 22){
			tileData.sprite = sprites[11];
		}
		else if(index == 24){
			tileData.sprite = sprites[12];
		}
	}

	private int GetIndex(byte mask){
		switch(mask){
			case 0: return 12; // 0 neighbours
			case 6: return 22; // neighbours at right and bottom
			case 14: return 1; // neighbours at left, right and bottom
			case 12: return 11; // neighbours at left and bottom
			case 7: return 5; // neighbours at right, top and bottom
			case 13: return 15; // neighbours at left, top and bottom
			case 3: return 8; // neighbours at right and top
			case 11: return 10; // neighbours at right, left, and top
			case 9: return 16; // neighbours at left and top
			case 15: return 24; // neighbours at right, left, top, and bottom
		}
		return -1;
	}

	#if UNITY_EDITOR
		[MenuItem("Assets/Scriptable_Tile")]
		public static void CreateScriptableTile(){
			string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
			if(path == ""){
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Scriptable_Tile>(), path);
		}
	#endif

}