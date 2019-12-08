using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodeScaler : MonoBehaviour
{
    public TreeNode Root;
    public float ScaleFactor;           // Less than 1.0 for harmonic scaling
    public float BranchReductionFactor; // Usually 0.75 to 0.5 is good

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(Root != null);
        InitializeAllNodes(Root, Root.PrimitiveList[0].transform.localScale);
    }

    // Initializes the nodes using the user specified ScaleFactor
    // NOTE: does NOT scale the first node that is passed in
    public void InitializeNodes(TreeNode tn)
    {
        // Main Branch Only (Trunk-Only) Scaling
        TreeNode child = tn.GetComponentInChildren<TreeNode>();
        if (child != null)
            ScaleBranchNodes(child, tn.PrimitiveList[0].transform.localScale);
    }

    // Full-tree (or subtree) recursive scaling
    public void InitializeAllNodes(TreeNode tn, Vector3 prevScale)
    {
        // Scale this node by the ScaleFactor (as long as it isn't the root)
        if (tn != Root)
        {
            float x = prevScale.x * ScaleFactor;
            float y = prevScale.y;
            float z = prevScale.z * ScaleFactor;
            prevScale = new Vector3(x, y, z);

            tn.PrimitiveList[0].transform.localScale = prevScale;
        }

        // Scale any branches connected to this node
        if (tn.transform.childCount > 1)
        { 
            for (int c = 1; c < tn.transform.childCount + 1; ++c)
            {
                TreeNode child = tn.transform.GetChild(c).GetComponent<TreeNode>();
                InitalizeBranch(child, tn.PrimitiveList[0].transform.localScale);
            }
        }

        TreeNode tnext = tn.transform.GetChild(0).GetComponent<TreeNode>();
        if(tnext != null)
            InitializeAllNodes(tnext, prevScale);
    }


    // Initializes branch nodes using the user specified ScaleFactor
    // NOTE: DOES scale the first node that is passed in
    // NOTE: Must pass prevScale from node that owns this branch
    public void InitalizeBranch(TreeNode tn, Vector3 prevScale)
    {
        prevScale *= BranchReductionFactor;
        if (tn != null)
            ScaleBranchNodes(tn, prevScale);
    }

    // Recursively scales the node primitives of the children of the 
    // supplied tree node by the ScaleFactor.
    private void ScaleBranchNodes(TreeNode tn, Vector3 prevScale)
    {
        // Simplified version - scales in all dimensions
        //prevScale *= ScaleFactor;

        // XZ scaling only
        float x = prevScale.x * ScaleFactor;
        float y = prevScale.y;
        float z = prevScale.z * ScaleFactor;
        prevScale = new Vector3(x, y, z);

        tn.PrimitiveList[0].transform.localScale = prevScale;
        TreeNode child = tn.GetComponentInChildren<TreeNode>();
        if (child != null)
        {
            ScaleBranchNodes(child, prevScale);
        }
    }

}
