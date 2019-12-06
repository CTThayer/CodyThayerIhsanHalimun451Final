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
    // updated (included wind simulation on TreeNodePrimitives).
    public void DeformMesh()
    {
        int nIndex = 0;
        foreach (TreeNode tn in branchNodes)
        {
            DeformVertRingsHelper(nIndex, tn);
            ++nIndex;
        }
        // TODO: Handle end-point of branch
        // TODO: Recalculate Normals after verts move
    }

    private void DeformVertRingsHelper(int n, TreeNode tn)
    {
        int nIndex = GetVertexRingStartByNode(n);
        Matrix4x4 nMatrix = tn.PrimitiveList[0].TRS_matrix;
        
        // Center of the ring should match the node's pivot so the matrix
        // be applied directly to each vertex
        for (int i = 0; i < cirSubdivs; ++i)
        {
            Vector3 vert = branchMesh.vertices[nIndex + i];
            vert = nMatrix.MultiplyPoint(vert);
            branchMesh.vertices[nIndex + i] = vert;
        }
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

    // TODO: Implement mesh initialization for branch

    private Mesh InitializeBranchMesh()
    {
        Mesh bMesh = new Mesh();
        bMesh.Clear();
        //int totalVerts = (cirSubdivs * branchNodes.Count * linSubdivs) + 1;
        int totalVerts = (cirSubdivs * branchNodes.Count) + 1;
        bMesh.vertices = InitializeVertices(totalVerts);
        return bMesh;
    }

    private Vector3[] InitializeVertices(int totalVerts)
    {
        Vector3[] verts = new Vector3[totalVerts];
        float theta = 360.0f / cirSubdivs;

        // Create a ring for each node
        for (int n = 0; n < branchNodes.Count - 1; ++n)
        {
            float r = (branchNodes[n].transform.localScale.x + branchNodes[n].transform.localScale.z) / 2.0f;
            for (int i = 0; i < cirSubdivs; ++i)
            {
                float x = r * Mathf.Cos(theta);
                float y = 2.0f;
                float z = r * Mathf.Sin(theta);

                verts[i * n] = new Vector3(x, y, z);
            }
        }

        // Handle final point on branch
        Transform t = branchNodes[branchNodes.Count - 1].transform;
        Vector3 last = t.up * t.localScale.y * 0.75f;
        verts[totalVerts] = last;

        return verts;
    }

    //private int[] InitializeTriangles(int t)
    //{
    //    int[] tris = new int[(t * 6) + 1];

    //    for (int r = 0; r < branchNodes.Count - 1; ++r)
    //    {
    //        for (int c = 0; c < cirSubdivs - 1; ++c)
    //        {

    //        }
    //    }
    //}

    //private Vector3[] InitializeNormals(int t)
    //{

    //}

    //private Vector2[] InitializeUVs(int t)
    //{

    //}

}
