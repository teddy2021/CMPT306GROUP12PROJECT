using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Wall_Tile : Scriptable_Tile {

	[SerializeField] private GameObject[] drops;
	public Sprite[] Top_Corners;
	public Sprite[] Sides;
	public Sprite[] Centers;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
		int mask = HasTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0; // bottom
		mask += HasTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 2 : 0; // right
		mask += HasTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 4 : 0; // top
		mask += HasTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 8 : 0; // left
		int index = GetIndex((byte)mask);
		int idx;
		if (index == 0){
			tileData.sprite = Centers[Random.Range(0, Centers.Length -1)];
		}
		else if (index == 9){
			tileData.sprite = Centers[Random.Range(0, (Centers.Length - 1))];
		}
		else if(index == 6 ){ // top left
			idx = Random.Range(0, Top_Corners.Length - 1);
			if(idx != 0 & idx % 2 == 0){
				idx -= 1;
			}
			tileData.sprite = Top_Corners[idx];
		}
		else if(index == 8){ // top right
			idx = Random.Range(0, Top_Corners.Length - 1);
			if(idx % 2 == 0){
				if(idx == 0){
					idx += 1;
				}
				else{
					idx -= 1;
				}
			}
			tileData.sprite = Top_Corners[idx];
		}
		else if (index == 4){  // left side
			idx = Random.Range(0, Sides.Length - 1);
			if(idx!= 0 & idx %2 != 0){
				idx -= 1;
			}
			tileData.sprite = Sides[idx];
		}
		else if (index == 5){
			idx = Random.Range(0, Sides.Length - 1);
			if(idx %2 == 0){
				if(idx == 0){
					idx += 1;
				}
				else{
					idx -= 1;
				}
				tileData.sprite = Sides[idx];
			}
		}

		else if(index >= 0 & index < sprites.Length - 4){
			tileData.sprite = sprites[index];
		}
			tileData.color = Color.white;
			tileData.flags = TileFlags.LockTransform;
			tileData.colliderType = ColliderType.Sprite;
	}

	public override int GetIndex(byte mask){
		switch(mask){
			case 0: return 0; // 0 neighbours
			case 6: return 1; // neighbours at		right and 	top
			case 11: return 2; // neighbours at		left, 		right and 	bottom
			case 12: return 3; // neighbours at		left and 	top
			case 7: return 4; // neighbours at		right, 		top and 	bottom
			case 13: return 5; // neighbours at		left, 		top and 	bottom
			case 3: return 6; // neighbours at		right and 	bottom
			case 14: return 7; // neighbours at		right, 		left, and 	top
			case 9: return 8; // neighbours at		left and 	bottom
			case 15: return 9; // neighbours at		on all sides
			case 5: return 10; // neighbours at		top and 	bottom
			case 10: return 11; // neighbours at	left and 	right
		}
		return -1;
	}

	public void DropItems(Vector3Int location){
		for(int i = 0; i < Random.Range(0,3); i += 1){
			Vector3 position = new Vector3(location.x, location.y, 1);
			Instantiate(drops[Random.Range(0, drops.Length)], position , Quaternion.identity);
		}
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