using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string name;
    
    public List<Node> children = null;
    public Node parent = null;

    public int val;
    public int alpha ;
    public int beta;
    
    public bool hasParent()
    {
        if (parent == null) return false;
        return true;
    }

    public bool hasChildren()
    {
        if (children == null) return false;
        return true;
    }
    
    
}
