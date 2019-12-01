using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    int cirSubdivs;             // Subdivs of branch circumference
    //int linSubdivs;             // Linear subdivs between nodes
    Mesh branchMesh;            // Branch mesh object
    List<TreeNode> branchNodes; // List of nodes that control the branch mesh
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeBranchController( int cSubdivs,
                                            //int lSubdivs,
                                            ref Mesh mesh, 
                                            ref List<TreeNode> nodes)
    {
        cirSubdivs = (cSubdivs >= 3) ? cSubdivs : 3;
        //linSubdivs = (lSubdivs >= 0) ? lSubdivs + 1 : 1;

        // TODO: Eliminate this if we decide mesh should get created by BranchController
        //branchMesh = (mesh != null) ? mesh : throw new System.ArgumentNullException("Invalid Mesh: mesh is null");
        if (mesh != null)
            branchMesh = mesh;
        else
            throw new System.ArgumentNullException("Invalid Mesh: mesh is null");

        //branchNodes = (nodes != null && nodes.Count <= 1) ? nodes : throw new System.ArgumentException("Invalid List: List contains too few nodes.");
        if (nodes != null && nodes.Count <= 1)
            branchNodes = nodes;
        else
            throw new System.ArgumentException("Invalid List: List contains too few nodes.");

    }


    // Should only be called AFTER all node matrices have been calculated and
    // updated.
    public void DeformMesh()
    {
        int nIndex = 0;
        foreach (TreeNode tn in branchNodes)
        {
            DeformationHelper(nIndex, tn);
            ++nIndex;
        }
    }

    private void DeformationHelper(int n, TreeNode tn)
    {
        int nIndex = GetVertexRingStartByNode(n);
        Matrix4x4 nMatrix = tn.PrimitiveList[0].TRS_matrix;
        //Vector3 P = GetRingPivot(nIndex);     // Ring Pivot (shouldn't be necessary because it should match the nodePrimitive pivot)
        //Quaternion Q = nMatrix.rotation;      // Shouldn't need to decompose TRS, should just apply

    }

    private int GetVertexRingStartByNode(int n)
    {
        return (n * cirSubdivs); // * linSubdivs;
    }

    private Vector3 GetRingPivot(int n)
    {
        Vector3 sum = branchMesh.vertices[n];
        for(int i = 0; i < cirSubdivs; ++i)
        {
            sum += branchMesh.vertices[n + i];
        }
        float denom = 1 / cirSubdivs;
        return sum * denom;
    }

    private Mesh InitializeBranchMesh()
    {
        Mesh bMesh = new Mesh();
        bMesh.Clear();
        //int totalVerts = (cirSubdivs * branchNodes.Count * linSubdivs) + 1;
        int totalVerts = (cirSubdivs * branchNodes.Count) + 1;

        bMesh.vertices = InitializeVertices(totalVerts);


        return bMesh;
    }

    private Vector3[] InitializeVertices(int t)
    {
        Vector3[] v = new Vector3[t];


        return v;
    }

    //private int[] InitializeTriangles(int t)
    //{

    //}

    //private Vector3[] InitializeNormals(int t)
    //{

    //}

    //private Vector2[] InitializeUVs(int t)
    //{

    //}

}
