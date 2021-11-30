using System;
using System.Collections.Generic;

public class MyPriorityQueue<T> where T : IComparable
{
    List<T> queue;

    public MyPriorityQueue()
    {
        queue = new List<T>();
    }
    public void Enqueue(T item)
    {
        if (queue.Count == 0)
        {
            queue.Add(item);
        }
        else
        {
            if (queue[queue.Count - 1].CompareTo(item) <= 0)
            {
                queue.Add(item);
                return;
            }
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].CompareTo(item) >= 0)
                {
                    queue.Insert(i, item);
                    return;
                }
            }
        }
    }

    public void Deque()
    {
        queue.RemoveAt(0);
    }

    public T Peek()
    {
        return queue[0];
    }
    public void updateItem(T item)
    {
        queue.Remove(item);
        Enqueue(item);
    }

    public void Clear()
    {
        queue.Clear();
    }
    public bool isEmpty()
    {
        return queue.Count == 0;
    }
    public int Count()
    {
        return queue.Count;
    }
}
