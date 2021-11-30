using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    [SerializeField]private Tilemap tilemap;
    [SerializeField] private TileBase Build;
    [SerializeField] private TileBase Destroyed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int offset;
        if (Input.GetKey(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                offset = new Vector3Int(0, -1, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                offset = new Vector3Int(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                offset = new Vector3Int(1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                offset = new Vector3Int(-1, 0, 0);
            }
            else
            {
                return;
            }
            Vector3Int myPos = tilemap.WorldToCell(transform.position);
            Vector3Int tilePos = myPos + offset;
            Debug.Log("Destroying");
            if (tilemap.GetTile(tilePos)==Destroyed)
            {
                tilemap.SetTile(tilePos,Build);
            }
        }
    }
}
