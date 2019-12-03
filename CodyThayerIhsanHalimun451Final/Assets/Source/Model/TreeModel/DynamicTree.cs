using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTree : MonoBehaviour
{
    public int branchSubdivs;       // Subdivs of branch circumference

    public TreeNode root;           // Root TreeNode of the tree's hierarchy
    public WindModel windModel;     // WindModel associated with this tree
    public List<Mesh> branches;     // Contains a mesh for each branch
    public List<BranchController> branchControllers;    // Contains a BranchController for each branch


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Initialize tree nodes
        // TODO: Initialize meshes

        Debug.Assert(windModel != null);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Handle manipulator input and changes

        // TODO: Implement and test the transform matrices being run
        // TODO: Add if/else to check if the WindModel is running and if not, run only the TreeNode transforms/shader loading
        Matrix4x4 i = Matrix4x4.identity;
        root.CompositeXform(ref i);         // TODO: We can optimize this now. Since xforms are stored, this only really needs to be run when nodes are manipulated.
        windModel.WindUpdate(root);
    }

    /*************************** Tree Configuration ***************************/
    /*                                                                        */

    /************************* InitializeDynamicTree ***********************//**
     * Parameterized initialization of the tree's attributes.
     *      @Param
     *      @Param
     *      @Param
     * 
     **************************************************************************/
    public void InitializeDynamicTree()
    {
        // TODO: Create parameterized initializer so that UI can create trees
        // based on input params.
    }

    /*                                                                        */
    /************************ END Tree Configuration **************************/


    /**************************** Mesh Generation *****************************/
    /*                                                                        */
    // TODO: GenerateMesh() recursive function
    // TODO: MeshGenHelper() function
    /*                                                                        */
    /************************** End Mesh Generation ***************************/

}
