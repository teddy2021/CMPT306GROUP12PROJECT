using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTileAnimationComplete()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
