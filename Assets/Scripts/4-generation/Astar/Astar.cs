using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar
{
    Vector3Int[] neighbors = { new Vector3Int(0, 1, 0),
    new Vector3Int(0, -1, 0),new Vector3Int(1, 0, 0),
    new Vector3Int(-1, 0, 0),};
    AstarNode src, dst;
    AllowedTiles allowedTiles; 
    MyPriorityQueue<AstarNode> queue;
    Tilemap tilemap;
    int maxIterations, currIter;
    HashSet<Vector3Int> openSet;
    TileBase tilebase;
    public Astar(Vector3Int src, Vector3Int dst, Tilemap tilemap, int maxIterations, TileBase tilebase, AllowedTiles allowedTiles)
    {
        this.allowedTiles = allowedTiles;
        this.tilebase = tilebase;
        openSet = new HashSet<Vector3Int>();
        currIter = 0;
        this.src = new AstarNode(src);
        this.dst = new AstarNode(dst);
        this.tilemap = tilemap;
        queue = new MyPriorityQueue<AstarNode>();
        this.maxIterations = maxIterations;
    }

    public List<Vector3Int> GetPath()
    {
        List<Vector3Int> path = new List<Vector3Int>();
        if (src.getLocation().Equals(dst.getLocation()))
        {
            path.Add(src.getLocation());
            return path;
        }
        queue.Enqueue(src);
        openSet.Add(src.getLocation());
        while (!queue.isEmpty() && currIter < maxIterations)
        {
            currIter++;
            AstarNode temp = queue.Peek();
            queue.Deque();
            foreach (var neighbor in neighbors)
            {
                Vector3Int currLocation = neighbor + temp.getLocation();
                if (!openSet.Contains(currLocation) && isContained(allowedTiles.Get(), currLocation))
                {
                    AstarNode neighborNode = new AstarNode(currLocation);
                    neighborNode.setSteps(temp.getSteps() + 1);
                    neighborNode.setWeight((neighborNode.getSteps() + calculateF(neighborNode, dst)));
                    if (isContained(allowedTiles.getSlow(), neighborNode.getLocation()))
                    {
                        neighborNode.setWeight((neighborNode.getWeight() + calculateF(neighborNode, dst)));
                    }
                    openSet.Add(neighborNode.getLocation());
                    queue.Enqueue(neighborNode);
                    neighborNode.setFather(temp);
/*                    tilemap.SetTile(neighborNode.getLocation(), tilebase);
*/                    if (neighborNode.getLocation().Equals(dst.getLocation()))
                    {
                        dst = neighborNode;
                        currIter = maxIterations;
                        break;
                    }
                }
            }
        }
        AstarNode tempReverse = dst;
        if (dst.getFather() == null)
        {
            path.Add(src.getLocation());
            return path;
        } 
        while (tempReverse.getLocation() != src.getLocation())
        {
            path.Add(tempReverse.getLocation());
            tempReverse = tempReverse.getFather();
        }
        path.Add(tempReverse.getLocation());
        path.Reverse();
        return path;
    }

    public bool isContained(TileBase[] tilebase,Vector3Int pos)
    {
        foreach(var tile in tilebase)
        {
            TileBase posTile = tilemap.GetTile(pos);
            if (posTile.Equals(tile)) return true;
        }
        return false;
    }
    public int calculateF(AstarNode a, AstarNode b)
    {
        Vector3Int dist = a.getLocation() - b.getLocation();
        return (System.Math.Abs(dist.x) + System.Math.Abs(dist.y));
    }

}