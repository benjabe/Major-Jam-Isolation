using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

[Preserve, MoonSharpUserData]
public class AStar
{
    public static List<Node<T>> FindPath<T>(Node<T> start, Node<T> goal, Func<Node<T>, float> heuristic)
    {
        var openSet = new HashSet<Node<T>>() { start };
        var closedSet = new HashSet<Node<T>>();
        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        var gScore = new Dictionary<Node<T>, float>();
        gScore[start] = 0;
        var fScore = new Dictionary<Node<T>, float>();
        fScore[start] = heuristic(start);

        while (openSet.Count > 0)
        {
            // Find the node in the open set with the lowest fScore.
            Node<T> current = null;
            foreach (var node in openSet)
            {
                if (current == null || fScore[node] < fScore[current])
                {
                    current = node;
                }
            }

            if (current == goal)
            {
                var path = new List<Node<T>> { current };
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    path.Add(current);
                }
                path.Reverse();
                return path;
            }

            closedSet.Add(current);
            openSet.Remove(current);
            foreach (var edge in current.Edges)
            {
                var neighbor = edge.To;
                var tentativeGScore = gScore[current] + edge.Cost;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + heuristic(neighbor);
                    if (!closedSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }
}
