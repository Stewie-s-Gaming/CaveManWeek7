using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    //Used to init the cellular automata by spreading random dots on a grid,
    //and from these dots we will generate caves.
    //The higher the fill percentage, the smaller the caves.
    protected float randomFillPercent;

    //The height and length of the grid
    protected int gridSize;

    //The double buffer
    private int[,] bufferOld;
    private int[,] bufferNew;
    private int sizeOfTiles;

    private System.Random random;

    public MapGenerator(int objectNum, int gridSize = 100)
    {
        this.randomFillPercent = 1f/objectNum;
        this.gridSize = gridSize;
        sizeOfTiles = objectNum;
        this.bufferOld = new int[gridSize, gridSize];
        this.bufferNew = new int[gridSize, gridSize];

        random = new System.Random();
    }

    public int[,] GetMap()
    {
        return bufferOld;
    }



    /**
     * Generate a random map.
     * The map is not smoothed; call Smooth several times in order to smooth it.
     */
    public void RandomizeMap()
    {
        //Init the old values so we can calculate the new values
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                    //Random walls and caves
                    bufferOld[x, y] = random.Next(0, sizeOfTiles) ;
            }
        }
    }


    /**
     * Generate caves by smoothing the data
     * Remember to always put the new results in bufferNew and use bufferOld to do the calculations
     */
    public void SmoothMap()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                {
                    int rInt = random.Next(0, sizeOfTiles);
                    bufferNew[x, y] = 1;
                    continue;
                }
                //Uses bufferOld to get the wall count
                int[] maxObjectSurrounding = GetMaxSurroundingCount(x, y);

                //Use some smoothing rules to generate caves
                if (maxObjectSurrounding[0] > 3)
                {
                    bufferNew[x, y] = maxObjectSurrounding[1];
                }
                else
                {
                    bufferNew[x, y] = bufferOld[x, y];
                }
                
            }
        }

        //Swap the pointers to the buffers
        (bufferOld, bufferNew) = (bufferNew, bufferOld);
    }



    //Given a cell, how many of the 8 surrounding cells are walls?
    private int[] GetMaxSurroundingCount(int cellX, int cellY)
    {
        int []maxTile = new int[2];
        int[] arr = new int[sizeOfTiles];
        for (int neighborX = cellX - 1; neighborX <= cellX + 1; neighborX++)
        {
            for (int neighborY = cellY - 1; neighborY <= cellY + 1; neighborY++)
            {
                //We dont need to care about being outside of the grid because we are never looking at the border
                if (neighborX == cellX && neighborY == cellY)
                { //This is the cell itself and no neighbor!
                    continue;
                }
                int index = bufferOld[neighborX, neighborY];
                arr[index] += 1;

            }
        }

        for (int i = 0; i < sizeOfTiles ; i++)
        {
            if (arr[i] > maxTile[0])
            {
                maxTile[0] = arr[i];
                maxTile[1] = i;
            }
        }
            return maxTile;
    }
}
