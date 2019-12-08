using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public TreeNode TN_prefab;              // NOTE: MUST have WNode on prefab
    public TreeNodePrimitive TNP_prefab;
    public GameObject CO_prefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TN_prefab != null);
        Debug.Assert(TNP_prefab != null);
        Debug.Assert(CO_prefab != null);
    }

    //public TreeNode GenerateTree()
    //{

    //}

    //private TreeNode GenerateBranch(int nodeDepth, Quaternion branchingAngle, float scaleFactor)
    //{

    //}

    //private TreeNode GenerateBranchHelper(ref int count,
    //                                        int nodeDepth,
    //                                        int branchFreq,
    //                                        Vector3 branchingAngle,
    //                                        float scaleFactor,
    //                                        float prevNodeScale
    //                                        )
    //{
    //    count++;

    //    if (count <= nodeDepth)
    //    {

    //    }

    //}

    private TreeNode InitializeNode(Vector3 nodeOrigin, Quaternion rotation, Vector3 scale)
    {
        TreeNode TN = Instantiate(TN_prefab);
        TreeNodePrimitive TNP = Instantiate(TNP_prefab);
        GameObject C = Instantiate(CO_prefab);

        TN.PrimitiveList.Add(TNP);
        TN.NodeOrigin = nodeOrigin;
        TN.transform.rotation = rotation;

        TNP.transform.localScale = scale;
        TNP.primCollider = C;

        return TN;
    }

    private void WeightTree()
    {

    }

    private void WeightBranch()
    {

    }

}
