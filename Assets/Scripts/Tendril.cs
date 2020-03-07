using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tendril : MonoBehaviour
{
    private LineRenderer line;
    private float[] previousPositions;
    private float[] currentPositions;
    private int numLinePoints;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        numLinePoints = line.positionCount;

        previousPositions = new float[numLinePoints];
        currentPositions = new float[numLinePoints];

        for (int i = 0; i < numLinePoints; i++) {
            previousPositions[i] = line.GetPosition(i).x;
            currentPositions[i] = line.GetPosition(i).x;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 1; i < numLinePoints; i++) {
            float curPos = currentPositions[i];
            float prevPos = previousPositions[i];
            float wind = (Random.value - .5f) * Mathf.Sign(curPos - prevPos);
            float newPos = (2.0f * curPos) - prevPos + wind*Time.fixedDeltaTime*Time.fixedDeltaTime;

            previousPositions[i] = currentPositions[i];
            currentPositions[i] = newPos;

            Vector3 curVec = line.GetPosition(i);
            curVec.x = newPos;
            line.SetPosition(i, curVec);
        }
    }
}
