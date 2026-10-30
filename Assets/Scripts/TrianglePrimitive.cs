#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrimitiveType))]
public class TrianglePrimitive : Editor
{
    private static Mesh CreateMesh()
    {
        float s = 1.0f; // Edge length
        float r = s / Mathf.Sqrt(3f);
        float h = s * Mathf.Sqrt(6f) / 3f;

        // Centered around centroid (0, 0, 0)
        float yBase = -h / 4f;
        float yApex = 3f * h / 4f;

        Vector3 apex = new Vector3(0, yApex, 0);
        Vector3 p0 = new Vector3(0, yBase, r);
        Vector3 p1 = new Vector3(-s * 0.5f, yBase, -r * 0.5f);
        Vector3 p2 = new Vector3(s * 0.5f, yBase, -r * 0.5f);

        // 4 faces * 3 vertices = 12 vertices for flat shading
        Vector3[] vertices = {
            // Base (facing down)
            p0, p1, p2,

            // Side 1
            apex, p1, p0,

            // Side 2
            apex, p2, p1,

            // Side 3
            apex, p0, p2
        };

        Vector2 uvBottomLeft = new Vector2(0f, 0f);
        Vector2 uvBottomRight = new Vector2(1f, 0f);
        Vector2 uvTop = new Vector2(0.5f, 1f);

        Vector2[] uv = {
            uvTop, uvBottomLeft, uvBottomRight, // Base
            uvTop, uvBottomLeft, uvBottomRight, // Side 1
            uvTop, uvBottomLeft, uvBottomRight, // Side 2
            uvTop, uvBottomLeft, uvBottomRight  // Side 3
        };

        int[] triangles = {
            0, 1, 2,     // Base
            3, 4, 5,     // Side 1
            6, 7, 8,     // Side 2
            9, 10, 11    // Side 3
        };

        var mesh = new Mesh();
        mesh.name = "Tetrahedron";
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    private static GameObject CreateObject()
    {
        var obj = new GameObject("Tetrahedron");
        var mesh = CreateMesh();
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        collider.convex = true;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    [MenuItem("GameObject/3D Object/Tetrahedron", false, 0)]
    public static void Create()
    {
        CreateObject();
    }
}
#endif