using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class bubble : MonoBehaviour {
    private SpriteShapeController shapeController;
    private Spline spline;

    private Transform head;

    private Vector2 currentPosition;
    private Vector2 newPosition;

    private Vector2 direction = Vector2.zero;
    private float speed = 5;

    // Start is called before the first frame update
    void Start() {
        shapeController = GetComponentInChildren<SpriteShapeController>();
        currentPosition = shapeController.spline.GetPosition(shapeController.spline.GetPointCount() - 1);
        newPosition = currentPosition;

        head = transform.Find("bubble_head");

        spline = shapeController.spline;
    }

    // Update is called once per frame
    void Update() {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction = direction + (directionalInput * Time.deltaTime * 20);
        direction = direction.normalized;
        Vector2 velocity = direction * speed;
        newPosition = newPosition + (velocity * Time.deltaTime);

        if ((newPosition - currentPosition).magnitude >.5) {
            int numPoints = spline.GetPointCount();
            spline.InsertPointAt(numPoints, newPosition);
            spline.SetTangentMode(numPoints, ShapeTangentMode.Continuous);
            if (numPoints > 10) {
                spline.RemovePointAt(0);
            }
            head.Translate(newPosition - currentPosition);

            currentPosition = newPosition;
        }
    }
}