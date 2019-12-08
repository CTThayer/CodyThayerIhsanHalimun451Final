using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRef : MonoBehaviour
{
    public TreeNode treeNode;

    void Start()
    {
        Debug.Assert(treeNode != null);
    }

}
