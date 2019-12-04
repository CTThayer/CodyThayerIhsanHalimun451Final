using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindModel_P : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartWSHelper(WindNode_P Wp)
    {
        
        foreach (Transform child in Wp.transform)
        {
            WindNode_P p = child.GetComponent<WindNode_P> ();
            if (p != null)
            {

            }
        }
    }
}
