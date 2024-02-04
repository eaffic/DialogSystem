using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class GridBase : MonoBehaviour
{
    [SerializeField, Tooltip("クリードのサイズ")]
    private Vector2 _gridSize = Vector2.zero;

    [SerializeField, Tooltip("ノードの半径(障害物検知用)")]
    private float _nodeRadius = 0;

    [SerializeField, Tooltip("障害物のレイヤー")]
    private LayerMask _layer = default;

    // ノードマップ
    private Node[,] _grid = null;
    // ノード直径
    private float _nodeDiameter = 0;
    // マップ長さ
    private int _gridCountX = 0;
    private int _gridCountY = 0;

    // エディター表示用
    public Stack<Node> NodePath = new Stack<Node>();

    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = _nodeRadius * 2;
        _gridCountX = Mathf.RoundToInt(_gridSize.x / _nodeDiameter);
        _gridCountY = Mathf.RoundToInt(_gridSize.y / _nodeDiameter);
        _grid = new Node[_gridCountX, _gridCountY];
        CreateGrid();
    }

    // ノードマップ作成
    void CreateGrid()
    {
        Vector3 startPos = transform.position;
        startPos.x = startPos.x - _gridSize.x / 2;
        startPos.z = startPos.z - _gridSize.y / 2;
        for (int i = 0; i < _gridCountX; ++i)
        {
            for (int j = 0; j < _gridCountY; j++)
            {
                Vector3 worldPos = startPos;
                worldPos.x = worldPos.x + i * _nodeDiameter + _nodeRadius;
                worldPos.z = worldPos.z + j * _nodeDiameter + _nodeRadius;
                bool canWalk = !Physics.CheckSphere(worldPos, _nodeRadius, _layer);
                _grid[i, j] = new Node(canWalk, worldPos, i, j);
            }
        }
    }

    // 空間位置から対応ノードを取得
    public Node GetFromPosition(Vector3 pos)
    {
        float percentX = (pos.x + _gridSize.x / 2) / _gridSize.x;
        float percentZ = (pos.z + _gridSize.y / 2) / _gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((_gridCountX - 1) * percentX);
        int z = Mathf.RoundToInt((_gridCountY - 1) * percentZ);
        return _grid[x, z];
    }

    // 隣接のノードを取得
    public List<Node> GetNeighbor(Node node)
    {
        List<Node> neighborList = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // 中心点をスキップ
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int tempX = node.X + i;
                int tempY = node.Y + j;
                if (tempX < _gridCountX && tempX > 0 && tempY > 0 && tempY < _gridCountY)
                {
                    neighborList.Add(_grid[tempX, tempY]);
                }
            }
        }
        return neighborList;
    }

    // 経路探索の可視化
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridSize.x, 1, _gridSize.y));
        if (_grid == null)
        {
            return;
        }

        // ノードの可視化
        foreach (var node in _grid)
        {
            Gizmos.color = node.CanWalk ? Color.white : Color.red;
            Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
        }

        // 経路の可視化
        if (NodePath != null)
        {
            foreach (var node in NodePath)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
            }
        }
    }
}