using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public Move move;
    public int valuation;
    public int depth;
    public TreeNode parent;
    public List<TreeNode> unviewed_children;
    public List<TreeNode> viewed_children;

    public TreeNode() //��� �����
    {
        move = null;
        valuation = 0;
        depth = 0;
        parent = null;
        unviewed_children = null;
        viewed_children = null;
    }

    public TreeNode(Move move, int valuation, TreeNode parent)
    {
        this.move = move;
        this.valuation = valuation;
        this.depth = parent.depth + 1;
        this.parent = parent;
        this.unviewed_children = null;
        this.viewed_children = null;
    }
}
