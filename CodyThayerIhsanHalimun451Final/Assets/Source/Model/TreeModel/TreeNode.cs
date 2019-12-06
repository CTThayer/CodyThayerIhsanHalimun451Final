using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    protected Matrix4x4 mCombinedParentXform;
    public Vector3 InitialNodeOrigin;
    private Vector3 NodeOrigin = Vector3.zero;
    public List<TreeNodePrimitive> PrimitiveList;

    // Use this for initialization
    protected void Start()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        NodeOrigin = InitialNodeOrigin;
    }
    

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    void Update()
    {
        UpdateNodeOrigin();
    }

    void UpdateNodeOrigin()
    {
        Vector3 pScale = Vector3.one;
        float y = 1;
        TreeNode parentNode = transform.GetComponentInParent<TreeNode>();
        if (parentNode != null)
        {
            pScale = parentNode.PrimitiveList[0].transform.localScale;
            y = InitialNodeOrigin.y * pScale.y;
        }
        NodeOrigin = new Vector3(NodeOrigin.x, y, NodeOrigin.z);
    }


    /**************************** CompositeXform ***************************//**
     *   Called by World (or maybe Tree) in Update to pass transform info before
     *   wind forces are calculated and applied. This is so that primitives are
     *   translated, scaled, and rotated before the wind calculation is made.
     *   
     *       @param parentXform    The Matrix4x4 for the transform passed down 
     *                             from the parent. (Concatenated from all
     *                             preceding parent nodes)
     *                             
     **************************************************************************/
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);

        // Original
        //Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        // New Version - doesn't pass scale (passes Vector3.one) to allow for scaling of onlt the selected node
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.one);

        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to children
        foreach (Transform child in transform)
        {
            TreeNode tn = child.GetComponent<TreeNode>();
            if (tn != null)
            {
                tn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (TreeNodePrimitive p in PrimitiveList)
        {
            //p.SetTransformMatrix(ref mCombinedParentXform);
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }



    /**************************** Add/Delete Node **************************//**
     *   AddTreeNode() adds a new TreeNode "inline" in front of the selected
     *   TreeNode.
     *   DeleteTreeNode() Deletes the selected TreeNode and re-parents its
     *   children to the TreeNode preceding this node.
     **************************************************************************/
    public void AddTreeNode()
    {
        GameObject NewSN = new GameObject();
        Instantiate(NewSN);
        NewSN.AddComponent<TreeNode>();
        NewSN.transform.parent = gameObject.transform;
    }

    public void DeleteTreeNode(GameObject g)
    {
        foreach (Transform child in g.transform)
        {
            if (child.GetComponent<TreeNode>() != null)
            {
                child.GetComponent<TreeNode>().NodeOrigin = g.GetComponent<TreeNode>().NodeOrigin;
                child.transform.parent = g.transform.parent;
            }
        }
        Destroy(g);
    }

}
