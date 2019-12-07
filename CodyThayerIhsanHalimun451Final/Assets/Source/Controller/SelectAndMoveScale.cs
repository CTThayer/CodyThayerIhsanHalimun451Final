using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndMoveScale : MonoBehaviour
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
            if (Input.GetKey(KeyCode.A))
            {
                HandleMouseEvents();
            }
        }
        if (SelectedObject != null) Debug.Log(SelectedObject.name + SelectedObject.tag);
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
            manipulator.transform.localScale = new Vector3(1, 1, 1);

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

            if (NewSelection.tag == "X-Manipulator"
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
                    Debug.Log("Selected:" + SelectedObject.name);
                    Debug.Log("Selected:" + ControlledObject.transform.parent.name);

                    Vector3 delta = Input.mousePosition - LastMousePosition;
                    Debug.Log("delta:" + delta);
                    float newX = manipulator.transform.localScale.x + delta.x * .2f; 
                    manipulator.transform.localScale = new Vector3(newX, manipulator.transform.localScale.y, manipulator.transform.localScale.z);

                    newX = ControlledObject.transform.parent.parent.localScale.x + delta.x * .2f;
                    ControlledObject.transform.parent.parent.localScale = new Vector3(newX, ControlledObject.transform.parent.parent.localScale.y, ControlledObject.transform.parent.parent.localScale.z);

                    LastMousePosition = Input.mousePosition;

                }
                else if (SelectedObject.tag == "Y-Maanipulator")
                {
                Vector3 delta = Input.mousePosition - LastMousePosition;

                float newY = manipulator.transform.localScale.z + delta.y * .2f;
                manipulator.transform.localScale = new Vector3(manipulator.transform.localScale.x, newY, manipulator.transform.localScale.z);

                newY = ControlledObject.transform.parent.localScale.z + delta.y * .2f;
                ControlledObject.transform.parent.localScale = new Vector3(ControlledObject.transform.parent.localScale.x, newY, ControlledObject.transform.parent.localScale.z);
                LastMousePosition = Input.mousePosition;

                }
                else if (SelectedObject.tag == "Z-Manipulator")
                {
                    Vector3 delta = Input.mousePosition - LastMousePosition;

                    float newZ = manipulator.transform.localScale.z + delta.x * .2f;
                    manipulator.transform.localScale = new Vector3(manipulator.transform.localScale.x, manipulator.transform.localScale.y, newZ);

                    newZ = ControlledObject.transform.parent.localScale.z + delta.x * .2f;
                    ControlledObject.transform.parent.localScale = new Vector3(ControlledObject.transform.parent.localScale.x, ControlledObject.transform.parent.localScale.y, newZ);

                    LastMousePosition = Input.mousePosition;

                }
                if (ControlledObject != null)
                {
                    Debug.Log(ControlledObject.transform.parent.name);
                }
            
        }
    }
}