using System.Collections.Generic;

public class Node<T>
{
    public List<Edge<T>> Edges { get; set; } = new List<Edge<T>>();
    public T Value { get; set; }
}
