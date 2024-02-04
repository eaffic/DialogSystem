using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // 親ノード
    public Node ParentNode { get; set; }

    // 移動可能か
    public bool CanWalk { get; set; }

    // 空間の位置
    public Vector3 WorldPosition { get; set; }

    // リスト内の位置
    public int X { get; set; }
    public int Y { get; set; }

    // コスト
    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }


    public Node() { }

    public Node(bool canWalk, Vector3 position, int x, int y)
    {
        this.CanWalk = canWalk;
        this.WorldPosition = position;
        this.X = x;
        this.Y = y;
    }
}
