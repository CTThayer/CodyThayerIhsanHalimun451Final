using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWindVector_XformController : MonoBehaviour
{
    public XfromControl WV_XformController;
    public GameObject WindVector_GUI_Object;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(WV_XformController != null);
        Debug.Assert(WindVector_GUI_Object != null);

        WV_XformController.SetSelectedObject(WindVector_GUI_Object.transform);
    }

}
