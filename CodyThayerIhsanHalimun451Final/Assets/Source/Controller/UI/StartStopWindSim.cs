using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopWindSim : MonoBehaviour
{
    public WModel windModel;
    public SelectionController selectionController;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(windModel.GetIsRunning() == false);
        Debug.Assert(selectionController.GetIsActive() == true);
    }

    public void SetWindSimStatus()
    {
        active = !active;
        if (active)
        {
            windModel.SetIsRunning(true);
            selectionController.SetIsActive(false);
        }
        else
        {
            windModel.SetIsRunning(false);
            selectionController.SetIsActive(true);
        }
    }
}
