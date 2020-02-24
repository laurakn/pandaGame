using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour {

    private Mesh mesh;
    private MeshFilter filter;

    public float moveSpeed = 1;
    private Vector3 inputVelocity;
    public int[] controlVertices = { 0, 15 };

    public int numVertices = 30;

    private float[, ] springSizes;
    public float springConstant = 10;
    public float dampingConstant = 1f;
    public bool gravityEnabled = false;

    private Vector3[] velocities;

    // Start is called before the first frame update
    void Start() {
        velocities = new Vector3[numVertices];
        springSizes = new float[numVertices, numVertices];
        for (int i = 0; i < numVertices; i++) {
            for (int j = 0; j < numVertices; j++) {
                springSizes[i, j] = -1;
            }
        }

        var vertices2D = new Vector2[numVertices];
        for (int i = 0; i < numVertices; i++) {
            var theta = 2 * Mathf.PI / numVertices * i;
            vertices2D[i] = new Vector2(Mathf.Cos(theta + Mathf.PI / 2), Mathf.Sin(theta + Mathf.PI / 2));
        }

        for (int i = 0; i < numVertices; i++) {
            springSizes[i, (i + 1) % numVertices] = Vector2.Distance(vertices2D[i], vertices2D[(i + 1) % numVertices]);
            var previous = i == 0 ? numVertices - 1 : i - 1;
            springSizes[i, previous] = Vector2.Distance(vertices2D[i], vertices2D[previous]);
            //var opposite = (i + numVertices / 2) % numVertices;
            //springSizes[i, opposite] = Vector2.Distance(vertices2D[i], vertices2D[opposite]);
        }

        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => Color.black) // Random.ColorHSV())
            .ToArray();

        mesh = new Mesh();
        mesh.vertices = vertices3D;
        mesh.triangles = indices;
        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    void Update() {
        inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate() {
        var newVertices = mesh.vertices;

        for (int i = 0; i < controlVertices.Length; i++) {
            velocities[controlVertices[i]] = inputVelocity;
        }

        for (int i = 0; i < numVertices; i++) {
            if (controlVertices.Contains(i)) {
                continue;
            }

            Vector3 force = Vector3.zero;
            for (int j = 0; j < numVertices; j++) {
                if (j == i) {
                    continue;
                }

                // Distance vectors point to vertex 'i'
                var distance = newVertices[i] - newVertices[j];
                var springSize = springSizes[i, j];
                if (springSize == -1) {
                    continue;
                    //if (distance.magnitude > .2f) {
                    //    continue;
                    //}
                    //springSize = .2f;
                }

                // If distance exceeds spring size, then compression will be negative.
                // This means the force will be away from vertex i, pulling it back in place.
                var compression = springSize - distance.magnitude;

                // To get new vertex velocity, F = -kx, so ma = -kx, so mv = ma*delta(t) = -kx * delta(t)
                force += (distance.normalized * compression) * springConstant;
            }

            if (gravityEnabled) {
                force += (Vector3) Physics2D.gravity;
            }

            // Mass of 1
            var velocity = force * Time.fixedDeltaTime - (velocities[i] * (dampingConstant * Time.fixedDeltaTime));
            velocities[i] = velocities[i] + velocity;

            // Damping is of the form F = -Cv where C is some damping constant.
            // mv' = -Cv*delta(t), where v' = v + delta(v). So delta(v) = (-C*delta(t) - 1)*v 
        }

        for (int i = 0; i < numVertices; i++) {
            var curPos = newVertices[i];
            newVertices[i] = curPos + (velocities[i] * Time.fixedDeltaTime);
        }

        mesh.vertices = newVertices;
        var vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVertices, v => v);
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();
        mesh.triangles = indices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}