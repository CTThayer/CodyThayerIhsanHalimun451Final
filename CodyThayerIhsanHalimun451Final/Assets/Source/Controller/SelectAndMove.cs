using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndMove : MonoBehaviour
{

    public Camera cam;
    public GameObject manipulator;


    public float translateSpeed;

    GameObject SelectedObject;
    GameObject ControlledObject;
    Vector3 LastMousePosition;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            HandleMouseEvents();
        }
    }

        GameObject GetSelection()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit!");
            return hit.collider.gameObject;
        }
        else
        {
            Debug.Log("No Hit!");
            return null;
        }
    }

    void HandleMouseEvents()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click!");

            GameObject NewSelection = GetSelection();
            // New Object Selected
            if (NewSelection != null
                && NewSelection != SelectedObject
                && NewSelection.transform.parent != manipulator
                && NewSelection.tag == "mController")
            {
                SelectedObject = NewSelection;
                manipulator.transform.position = SelectedObject.transform.position;
                manipulator.transform.rotation = SelectedObject.transform.rotation;
            }
            //if (CurrentSelection.transform.parent == manipulator)

            if(NewSelection.tag == "X-Manipulator"
               || NewSelection.tag == "Y-Manipulator"
               || NewSelection.tag == "Z-Manipulator")
            {
                // If last selection was NOT a manipulator
                if (SelectedObject.tag != "X-Manipulator"
                   && SelectedObject.tag != "Y-Manipulator"
                   && SelectedObject.tag != "Z-Manipulator")
                {
                    ControlledObject = SelectedObject;  // Store previous selection in 
                    SelectedObject = NewSelection;
                    LastMousePosition = Input.mousePosition;
                }
                else  // If last selection WAS a maipulator only change selection
                {
                    SelectedObject = NewSelection;
                    LastMousePosition = Input.mousePosition;
                }
            }
        }
        if (Input.GetMouseButton(0) && SelectedObject != null)
        {
            if (SelectedObject.tag == "X-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;
                float x = ControlledObject.transform.parent.parent.localScale.x + (delta.x * translateSpeed * Time.deltaTime);
                ControlledObject.transform.parent.parent.localScale = new Vector3(x, ControlledObject.transform.parent.parent.localScale.y, ControlledObject.transform.parent.parent.localScale.z);
            //    GameObject.Find("AxisFrame_XCylinder").transform.localScale = new Vector3(manipulator.transform.localScale.y, x, manipulator.transform.localScale.z);
             //   manipulator.transform.localScale *= x;
                LastMousePosition = Input.mousePosition;
                Debug.Log("x: " + x);
            }
            else if (SelectedObject.tag == "Y-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;
                float y = ControlledObject.transform.parent.parent.localScale.y + (delta.y * translateSpeed * Time.deltaTime);
                ControlledObject.transform.parent.parent.localScale = new Vector3(ControlledObject.transform.parent.parent.localScale.x, y, ControlledObject.transform.parent.parent.localScale.z);
              //  GameObject.Find("AxisFrame_YCylinder").transform.localScale = new Vector3(manipulator.transform.localScale.x, y, manipulator.transform.localScale.z);
                //manipulator.transform.localScale *= y;
                LastMousePosition = Input.mousePosition;
                Debug.Log("y: " + y);
            }
            else if (SelectedObject.tag == "Z-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;
                float z = ControlledObject.transform.parent.parent.localScale.x + (delta.x * translateSpeed * Time.deltaTime);
                ControlledObject.transform.parent.parent.localScale = new Vector3(ControlledObject.transform.parent.parent.localScale.x, ControlledObject.transform.parent.parent.localScale.y, z);
                //GameObject.Find("AxisFrame_ZCylinder").transform.localScale = new Vector3(manipulator.transform.localScale.x, z, manipulator.transform.localScale.z);
             //   manipulator.transform.localScale *= z;
                LastMousePosition = Input.mousePosition;
                Debug.Log("z: " + z);
            }
        }
    }
}
