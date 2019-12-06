using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_World : MonoBehaviour
{
    public TreeNode TheRoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
    }
}
