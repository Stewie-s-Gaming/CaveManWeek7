using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap = null;

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField] TileBase[] tiles=null;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField] int simulationSteps = 20;

    [Tooltip("For how long will we pause between each simulation step so we can look at the result?")]
    [SerializeField] float pauseTime = 1f;

    private MapGenerator mapGenerator;

    void Start()
    {
        //To get the same random numbers each time we run the script
        Random.InitState(100);
        mapGenerator = new MapGenerator(tiles.Length, gridSize);
        mapGenerator.RandomizeMap();

        //For testing that init is working
        GenerateAndDisplayTexture(mapGenerator.GetMap());

        //Start the simulation
        StartCoroutine(SimulateMapPattern());
    }


    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateMapPattern()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            mapGenerator.SmoothMap();

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(mapGenerator.GetMap());
            Debug.Log("Simulation completed!");

        }
    }



    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data)
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y];
                tilemap.SetTile(position, tiles[tile]);
            }
        }
    }
}
