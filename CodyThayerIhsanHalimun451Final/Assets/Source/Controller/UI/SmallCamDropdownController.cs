using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCamDropdownController : MonoBehaviour
{
    public SmallCamController SCC;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(SCC != null);
    }

    public void SetSmallCamView(int selection)
    {
        switch(selection)
        {
            case 0:
                SCC.SetToTopView();
                break;
            case 1:
                SCC.SetToFrontView();
                break;
            case 2:
                SCC.SetToBackView();
                break;
            case 3:
                SCC.SetToRightView();
                break;
            case 4:
                SCC.SetToLeftView();
                break;
        }
    }

}
