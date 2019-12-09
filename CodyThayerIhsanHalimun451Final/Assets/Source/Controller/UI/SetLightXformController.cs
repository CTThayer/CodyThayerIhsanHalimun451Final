using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightXformController : MonoBehaviour
{
    public LightController lightController;
    public GameObject lightObject;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(lightController != null);
        Debug.Assert(lightObject != null);

        lightController.SetSelectedObject(lightObject.transform);
    }
}
