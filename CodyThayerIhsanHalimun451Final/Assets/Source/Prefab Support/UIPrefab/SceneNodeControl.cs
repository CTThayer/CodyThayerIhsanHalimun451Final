using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNodeControl : MonoBehaviour {
    public Dropdown TheMenu = null;
    public SceneNode TheRoot = null;
    public XfromControl XformControl = null;

    public NodePrimitive AxisFrame;
    public NodePrimitive AxisFrameX;
    public NodePrimitive AxisFrameY;
    public NodePrimitive AxisFrameZ;
    SceneNode currentSceneNode;

    const string kChildSpace = "  ";
    List<Dropdown.OptionData> mSelectMenuOptions = new List<Dropdown.OptionData>();
    List<Transform> mSelectedTransform = new List<Transform>();    

    // Use this for initialization
    void Start () {
        Debug.Assert(TheMenu != null);
        Debug.Assert(TheRoot != null);
        Debug.Assert(XformControl != null);

        mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
        mSelectedTransform.Add(TheRoot.transform);
        GetChildrenNames("", TheRoot.transform);
        TheMenu.AddOptions(mSelectMenuOptions);
        TheMenu.onValueChanged.AddListener(SelectionChange);

        XformControl.SetSelectedObject(TheRoot.transform);

        Debug.Assert(AxisFrame != null);
        TheRoot.PrimitiveList.Add(AxisFrame);
        TheRoot.PrimitiveList.Add(AxisFrameX);
        TheRoot.PrimitiveList.Add(AxisFrameY);
        TheRoot.PrimitiveList.Add(AxisFrameZ);
        currentSceneNode = TheRoot;
    }

    void GetChildrenNames(string blanks, Transform node)
    {
        string space = blanks + kChildSpace;
        for (int i = node.childCount - 1; i >= 0; i--)
        {
            Transform child = node.GetChild(i);
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                mSelectMenuOptions.Add(new Dropdown.OptionData(space + child.name));
                mSelectedTransform.Add(child);
                GetChildrenNames(blanks + kChildSpace, child);
            }
        }
    }

    void SelectionChange(int index)
    {
        XformControl.SetSelectedObject(mSelectedTransform[index]);
        currentSceneNode.PrimitiveList.Remove(AxisFrame);
        currentSceneNode.PrimitiveList.Remove(AxisFrameX);
        currentSceneNode.PrimitiveList.Remove(AxisFrameY);
        currentSceneNode.PrimitiveList.Remove(AxisFrameZ);

        SceneNode selected = mSelectedTransform[index].GetComponent<SceneNode>();
        if (selected != null)
        {
            currentSceneNode = selected;
            currentSceneNode.PrimitiveList.Add(AxisFrame);
            currentSceneNode.PrimitiveList.Add(AxisFrameX);
            currentSceneNode.PrimitiveList.Add(AxisFrameY);
            currentSceneNode.PrimitiveList.Add(AxisFrameZ);
        }
    }

    //SceneNode FindSceneNode(Transform startTransform, Transform.Target)
    //{
    //    foreach (Transform child in TheRoot.transform)
    //    {
    //        SceneNode cn = child.GetComponent<SceneNode>();
    //        if (cn != null)
    //        {
    //            if ()
    //            FindSceneNode(cn.transform);

    //            cn.CompositeXform(ref mCombinedParentXform);
    //        }
    //    }
    //}

}
