using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode : IComparable
{
    Vector3Int location;
    AstarNode father;
    int weight;
    int steps;
    public AstarNode(Vector3Int location)
    {
        steps = 0;
        this.location = location;
        weight = 0;
    }
    public void setWeight(int weight)
    {
        this.weight = weight;
    }
    public void setSteps(int steps)
    {
        this.steps = steps;
    }

    public int getSteps()
    {
        return steps;
    }
    public int getWeight()
    {
        return weight;
    }

    public void setFather(AstarNode father)
    {
        this.father = father;
    }

    public AstarNode getFather()
    {
        return father;
    }

    public Vector3Int getLocation()
    {
        return location;
    }
    public int CompareTo(object obj)
    {
        if (!(obj is AstarNode))
        {
            throw new ArgumentException("Compared Object is not of car");
        }
        AstarNode node = obj as AstarNode;
        return weight.CompareTo(node.weight);
    }
}
