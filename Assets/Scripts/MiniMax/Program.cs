using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Program : MonoBehaviour
{
    private Node top;
    private bool isMaxGraph; // check if the highest node is finding MaxVal or MinVal
    private int n;

    private bool isMax;
    [SerializeField] private TextAsset _dataFile;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void LoadDatabase(TextAsset dataFile)
    {
        if(dataFile != null)
        {
            string[] line = dataFile.text.Split("\n");
            Read(line);
        }
        
        
    }
    
    public void Read(string[] data)
    {
        n = int.Parse(data[0]);
        if (data[1] == "1")
            isMax = true;
        else isMax = false;

        Queue<Node> q = new Queue<Node>();
        q.Enqueue(top);
        
        

    }

    public void Solve()
    {
        int alpha = int.Parse(Mathf.NegativeInfinity.ToString());
        int beta = int.Parse(Mathf.Infinity.ToString());

        foreach (Node w in top.children)
        {
            if (alpha <= MinVal(w, ref alpha, ref beta))
            {
                alpha = MaxVal(w, ref alpha, ref beta);
                

            }
        }
    }

    public int MaxVal(Node u, ref int alpha, ref int beta)
    {
        if (u.hasChildren() == false)
            return u.val;

        int val = int.Parse(Mathf.NegativeInfinity.ToString());

        foreach (Node v in u.children)
        {
            val = Mathf.Max(val, MinVal(v, ref alpha, ref beta));

            //Cut the children of the remaining v-Node   
            if (val >= beta)
                return val;
            alpha = Math.Max(alpha, val);
        }


        return val;
    }

    public int MinVal(Node u, ref int alpha, ref int beta)
    {

        if (u.hasChildren() == false)
        {
            return u.val;
        }
        
        int val = int.Parse(Mathf.Infinity.ToString());
        foreach (Node v in u.children)
        {
            val = Mathf.Min(val, MaxVal(v, ref alpha, ref beta));
            if (val <= alpha)
                return val;

            beta = Mathf.Min(beta, val);
        }
        
        return val;
    }
}
