using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Path : IEquatable<Path>, ICloneable
{
    public float PathCost { get; set; }
    public List<Vertex> Vertices { get; set; }

    public Path()
    {
        PathCost = -1;
        Vertices = new List<Vertex>();
    }

    public Path(float pathCost, List<Vertex> vertices)
    {
        PathCost = pathCost;
        Vertices = vertices;
    }

    public bool Equals(Path other)
    {
        if (Vertices.Count!= other.Vertices.Count)
        {
            return false;
        }
        else
        {
            bool isEqual = true;
            Vertex[] vertArray1 = Vertices.ToArray();
            Vertex[] vertArray2 = other.Vertices.ToArray();
            for(var i= 0; i< vertArray1.Length; i++)
            {
                isEqual = isEqual && vertArray1[i].Equals(vertArray2[i]);
            }
            return isEqual;
        }
    }

    public int LengthOfEquality(Path other)
    {
        int equalityLength = 0;
        Vertex[] vertArray1 = Vertices.ToArray();
        Vertex[] vertArray2 = other.Vertices.ToArray();
        for (var i = 0; i < vertArray1.Length; i++)
        {
            if(vertArray2.Length>i)
            {
                if (vertArray1[i].Equals(vertArray2[i]))
                    equalityLength++;
                else
                {
                    return equalityLength;
                }
            }
            else
            {
                return equalityLength;
            }
        }
        return equalityLength;
    }

    public List<Vertex> GetPath()
    {
        List<Vertex> path = new List<Vertex>();
        foreach (var vertex in Vertices)
        {
            path.Add(vertex);
        }
        return path;
    }

    public object Clone()
    {
        return new Path(PathCost, GetPath());
    }

}

