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
            //W.ApplyWindMovement();
            W.ApplyWindMovementVector(WindVector);
        }
        else if (W.forward && W.currentR >= W.MAX_R)
        {
            W.forward = false;
        }
        else if (!W.forward && W.currentR > W.MIN_R)
        {
            //W.ApplyWindMovement();
            W.ApplyWindMovementVector(WindVector);
        }
        else if (!W.forward && W.currentR <= W.MIN_R)
        {
            W.forward = true;
        }
    }

    private void UpdateWindVector()
    {
        WindVector = wvHead.transform.position - wvTail.transform.position;

        wvLine.transform.position = wvTail.transform.position;
        wvLine.transform.localRotation = Quaternion.FromToRotation(wvLine.transform.up, WindVector);
        Vector3 scale = wvLine.transform.localScale;
        scale.y = WindVector.magnitude * 0.5f;
        wvLine.transform.localScale = scale;

    }

}
