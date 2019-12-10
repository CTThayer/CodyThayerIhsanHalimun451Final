using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WModel : MonoBehaviour
{
    public bool IsRunning;

    public Vector3 WindVector;
    public WNode Root;

    public GameObject wvHead;
    public GameObject wvTail;
    public GameObject wvLine;

    Vector3 PrevHeadLoc;
    Vector3 PrevTailLoc;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(wvHead != null);
        Debug.Assert(wvTail != null);
        Debug.Assert(wvLine != null);
        PrevHeadLoc = wvHead.transform.position;
        PrevTailLoc = wvTail.transform.position;
        UpdateWindVector();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            // If WindVector controllers have changed, update WindVector
            if (wvHead.transform.position != PrevHeadLoc
                || wvTail.transform.position != PrevTailLoc)
            {
                UpdateWindVector();
                PrevHeadLoc = wvHead.transform.position;
                PrevTailLoc = wvTail.transform.position;
            }

            // Run UpdateNodes to execute wind simulation
            UpdateNodes(Root);

        }
    }

    void UpdateNodes(WNode w)
    {
        MoveWNode(w);
        foreach (Transform child in w.transform)
        {
            WNode wn = child.GetComponent<WNode>();
            if (wn != null)
            {
                UpdateNodes(wn);
            }
        }
    }

    void MoveWNode(WNode W)
    {
        // Can simplify this if-else
        if (W.forward && W.currentR < W.MAX_R)
        {
            W.ApplyWindMovementVector(WindVector);
        }
        else if (W.forward && W.currentR >= W.MAX_R)
        {
            W.forward = false;
        }
        else if (!W.forward && W.currentR > W.MIN_R)
        {
            W.ApplyWindMovementVector(WindVector);
        }
        else if (!W.forward && W.currentR <= W.MIN_R)
        {
            W.forward = true;
        }
    }

    private void UpdateWindVector()
    {
        // Update actual Vector3 based on the representative U.graphical objects
        WindVector = wvHead.transform.position - wvTail.transform.position;
    }

    public void SetIsRunning(bool b)
    {
        IsRunning = b;
        if (IsRunning == true)
        {
            SaveOriginalNodeStates(Root);
        }
        else
        {
            ResetNodes(Root);
        }
    }

    public bool GetIsRunning()
    {
        return IsRunning;
    }

    private void ResetNodes(WNode wn)
    {
        wn.transform.up = wn.GetOriginalupVector();
        foreach(Transform t in wn.transform)
        {
            WNode child = t.GetComponent<WNode>();
            if (child != null)
            {
                ResetNodes(child);
            }
        }
    }

    private void SaveOriginalNodeStates(WNode wn)
    {
        wn.SetOriginalUpVector();
        foreach (Transform t in wn.transform)
        {
            WNode child = t.GetComponent<WNode>();
            if (child != null)
            {
                SaveOriginalNodeStates(child);
            }
        }
    }

}
