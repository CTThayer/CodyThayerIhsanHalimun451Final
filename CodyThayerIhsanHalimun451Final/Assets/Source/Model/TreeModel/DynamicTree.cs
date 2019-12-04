using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTree : MonoBehaviour
{

    public TreeNode root;           // Root TreeNode of the tree's hierarchy
    public WindModel windModel;     // WindModel associated with this tree

    //public int branchSubdivs;       // Subdivs of branch circumference
    //public List<Mesh> branches;     // Contains a mesh for each branch
    //public List<BranchController> branchControllers;    // Contains a BranchController for each branch


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

        
        

        Matrix4x4 i = Matrix4x4.identity;

        // TODO: We could possibly optimize this now. Since xforms are stored, this only really needs to be run when nodes are manipulated.
        root.CompositeXform(ref i);

        // TODO: Implement and test the transform matrices being run
        
        // DirectLoadShader passes the transform matrix without applying wind
        //   i.e. passes the identity matrix to LoadShaderMatrix() which 
        //   effectively only applies the stored TRS.
        root.DirectLoadShader();

        // TODO: Add if/else to check if the WindModel is running and if not, run only the TreeNode transforms/shader loading

        // This runs the Wind Model updates and applies wind to tree
        //windModel.WindUpdate(root);

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
    //public void InitializeDynamicTree()
    //{
    //    // TODO: Create parameterized initializer so that UI can create trees
    //    // based on input params.
    //}

    /**************************** Mesh Generation *****************************/
    /*                                                                        */
    // TODO: GenerateMesh() recursive function
    // TODO: MeshGenHelper() function
    /*                                                                        */
    /************************** End Mesh Generation ***************************/

    /*                                                                        */
    /************************ END Tree Configuration **************************/
}
