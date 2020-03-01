using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour {

    private Mesh mesh;
    private MeshFilter filter;

    public float moveSpeed = 1;
    private Vector3 inputVelocity;
    private int[] controlVertices;

    public int subdivisions = 10;
    public float outerDistance = .1f;
    private int numVertices;

    private float[, ] springSizes;
    public float springConstant = 10;
    public float dampingConstant = 1f;
    public bool gravityEnabled = false;

    private GameObject meshObject;
    private Vector3[] velocities;

    private Rigidbody2D rigidbody;
    private PolygonCollider2D collider;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();

        collider = GetComponent<PolygonCollider2D>();
        var colliderPoints = collider.points;
        numVertices = collider.GetTotalPointCount() * subdivisions * 2;

        controlVertices = new int[numVertices / 2];

        velocities = new Vector3[numVertices];
        springSizes = new float[numVertices, numVertices];
        for (int i = 0; i < numVertices; i++) {
            for (int j = 0; j < numVertices; j++) {
                springSizes[i, j] = -1;
            }
        }

        Vector2 colliderCenter = collider.bounds.center;
        var vertices = new Vector3[numVertices];
        SetVerticesToCollider(vertices);

        for (int i = 0; i < numVertices; i += 2) {
            int current = i;
            int outerPoint = Mod(i + 1, numVertices);
            int nextOnCollider = Mod(i + 2, numVertices);
            int nextOuter = Mod(i + 3, numVertices);
            int previousOuter = Mod(i - 1, numVertices);
            int previousOnCollider = Mod(i - 2, numVertices);

            setSpringSize(vertices, current, outerPoint);
            setSpringSize(vertices, current, nextOnCollider);
            setSpringSize(vertices, current, previousOnCollider);
            setSpringSize(vertices, outerPoint, current);
            setSpringSize(vertices, outerPoint, nextOuter);
            setSpringSize(vertices, outerPoint, previousOuter);

            controlVertices[i / 2] = i;
        }

        var indices = new List<int>();
        for (int i = 0; i < numVertices; i++) {
            for (int j = 0; j < 3; j++) {
                indices.Add(Mod(i + j, numVertices));
            }
        }

        var colors = Enumerable.Range(0, vertices.Length)
            .Select(i => Color.black) // Random.ColorHSV())
            .ToArray();

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices.ToArray();
        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshObject = new GameObject();
        meshObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        var meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        filter = meshObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    private void setSpringSize(Vector3[] vertices, int from, int to) {
        springSizes[from, to] = Vector2.Distance(vertices[from], vertices[to]);
    }

    private int Mod(int i, int mod) {
        if (i < 0) {
            return mod + i;
        }
        return i % mod;
    }

    void Update() {
        inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed;
        rigidbody.velocity = inputVelocity;
    }

    void FixedUpdate() {
        var newVertices = mesh.vertices;

        // Don't need to do this since rigidbody moves the mesh
        UpdateFixedVertices(newVertices);

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

            var prevMag = velocities[i].magnitude;
            var prevDir = velocities[i].normalized;
            // Mass of 1
            var velocity = (force * Time.fixedDeltaTime) - (prevDir * Mathf.Pow(prevMag, 2) * dampingConstant * Time.fixedDeltaTime);
            //velocity = velocity + (Vector3) (-.5f * rigidbody.velocity);
            //velocity = velocity + new Vector3(Random.value - .5f, Random.value -.5f, 0);
            velocities[i] = velocities[i] + velocity;

            // Damping is of the form F = -Cv where C is some damping constant.
            // mv' = -Cv*delta(t), where v' = v + delta(v). So delta(v) = (-C*delta(t) - 1)*v 
        }

        for (int i = 0; i < numVertices; i++) {
            if (controlVertices.Contains(i)) {
                continue;
            }
            var curPos = newVertices[i];
            newVertices[i] = curPos + (velocities[i] * Time.fixedDeltaTime);
        }

        mesh.vertices = newVertices;
        //var vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVertices, v => v);
        //var triangulator = new Triangulator(vertices2D);
        //var indices = triangulator.Triangulate();
        //mesh.triangles = indices;

        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
    }

    private void UpdateFixedVertices(Vector3[] vertices) {
        var colliderPoints = collider.points;
        Vector2 rigidBodyTranslation = rigidbody.transform.position;
        for (int i = 0; i < colliderPoints.Length; i++) {
            Vector2 current = colliderPoints[i] + rigidBodyTranslation;
            Vector2 next = colliderPoints[(i + 1) % colliderPoints.Length] + rigidBodyTranslation;

            for (int j = 0; j < subdivisions; j++) {
                // Distance to next point on collider subdivided
                Vector2 toNext = (next - current) / subdivisions;
                Vector2 subdividedPoint = current + (toNext * j);
                vertices[(i * subdivisions * 2) + (2 * j)] = subdividedPoint;
            }
        }
    }

    private void SetVerticesToCollider(Vector3[] vertices) {
        var colliderPoints = collider.points;
        Vector2 colliderCenter = collider.bounds.center;
        for (int i = 0; i < colliderPoints.Length; i++) {
            Vector2 current = colliderPoints[i];
            Vector2 next = colliderPoints[(i + 1) % colliderPoints.Length];

            for (int j = 0; j < subdivisions; j++) {
                // Distance to next point on collider subdivided
                Vector2 toNext = (next - current) / subdivisions;
                Vector2 subdividedPoint = current + (toNext * j);
                vertices[(i * subdivisions * 2) + (2 * j)] = subdividedPoint;

                Vector2 awayFromCenter = (current - colliderCenter).normalized;
                Vector2 outerPoint = subdividedPoint + awayFromCenter * outerDistance;
                vertices[(i * subdivisions * 2) + (2 * j) + 1] = outerPoint;
            }
        }
    }
}