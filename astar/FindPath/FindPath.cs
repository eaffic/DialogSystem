using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.iOS;
using UnityEngine;

[RequireComponent(typeof(GridBase))]
public class FindPath : MonoBehaviour
{
    [SerializeField, Tooltip("開始位置")]
    private Transform _startNode;
    [SerializeField, Tooltip("終了位置")]
    private Transform _endNode;

    private GridBase _grid;

    private List<Node> _openList = new List<Node>();
    private List<Node> _closeList = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponent<GridBase>();
    }

    // Update is called once per frame
    void Update()
    {
        FindingPath(_startNode.position, _endNode.position);
    }

    private void FindingPath(Vector3 start, Vector3 end)
    {
        Node startNode = _grid.GetFromPosition(start);
        Node endNode = _grid.GetFromPosition(end);
        _openList.Clear();
        _closeList.Clear();

        int count = 0;
        // 開始点追加
        _openList.Add(startNode);
        while (_openList.Count > 0)
        {
            Debug.Log(++count);
            // F値の昇順ソート
            _openList.Sort((x, y) => x.F.CompareTo(y.F));
            _openList.Sort((x, y) =>
            {
                int result = x.F.CompareTo(y.F);                    // F値の昇順
                return result != 0 ? result : x.H.CompareTo(y.H);   // F値等しい場合、H値の昇順
            });

            // 最小F値のノードを取得
            Node currentNode = _openList[0];

            // 取得のノードをCloseListに移行
            _openList.Remove(currentNode);
            _closeList.Add(currentNode);

            // 目的地の場合終了
            if (currentNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }

            var neighborNodes = _grid.GetNeighbor(currentNode);
            // 周囲ノードのチェック
            foreach (var node in neighborNodes)
            {
                // 探索済み、移動不可のノードはスキップ
                if (node.CanWalk == false || _closeList.Contains(node))
                {
                    continue;
                }

                int gCost = currentNode.G + GetNodeDistance(currentNode, endNode);
                if (_openList.Contains(node))
                {
                    // 新しい経路のG値は小さい場合更新する
                    if (gCost < node.G)
                    {
                        // ノードのデータを更新
                        node.G = gCost;
                        node.H = GetNodeDistance(node, endNode);
                        // 親ノード設定
                        node.ParentNode = currentNode;
                    }
                }
                else
                {
                    // OpenList内に存在しない場合追加
                    node.G = gCost;
                    node.H = GetNodeDistance(node, endNode);
                    node.ParentNode = currentNode;
                    _openList.Add(node);
                }
            }
        }
    }

    // 経路生成
    private void GeneratePath(Node startNode, Node endNode)
    {
        Stack<Node> path = new Stack<Node>();
        Node node = endNode;
        while (node.ParentNode != null)
        {
            path.Push(node);
            node = node.ParentNode;
        }
        _grid.NodePath = path;
    }

    // 目標位置との距離(マンハッタン距離)
    private int GetNodeDistance(Node nowNode, Node targetNode)
    {
        int deltaX = Mathf.Abs(nowNode.X - targetNode.X);
        int deltaY = Mathf.Abs(nowNode.Y - targetNode.Y);
        if (deltaX > deltaY)
        {
            return deltaY * 14 + 10 * (deltaX - deltaY);
        }
        else
        {
            return deltaX * 14 + 10 * (deltaY - deltaX);
        }
    }
}
