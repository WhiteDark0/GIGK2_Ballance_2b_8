using UnityEditor;
using UnityEngine;

public static class TrianglePrimitive
{
    private static Mesh CreateMesh()
    {
        float s = 1.0f; // Edge length
        float r = s / Mathf.Sqrt(3f);
        float h = s * Mathf.Sqrt(6f) / 3f;

        // Vertices centered around the geometric centroid (0, 0, 0)
        float yBase = -h / 4f;
        float yApex = 3f * h / 4f;

        Vector3 apex = new Vector3(0, yApex, 0);
        Vector3 p0 = new Vector3(0, yBase, r);
        Vector3 p1 = new Vector3(-s * 0.5f, yBase, -r * 0.5f);
        Vector3 p2 = new Vector3(s * 0.5f, yBase, -r * 0.5f);

        // 4 faces * 3 vertices = 12 vertices (ensures flat shading and independent UVs)
        Vector3[] vertices = new Vector3[]
        {
            // Base (facing down)
            p0, p1, p2,

            // Side 1 (facing -X, +Z)
            apex, p1, p0,

            // Side 2 (facing -Z)
            apex, p2, p1,

            // Side 3 (facing +X, +Z)
            apex, p0, p2
        };

        Vector2 uvBottomLeft = new Vector2(0f, 0f);
        Vector2 uvBottomRight = new Vector2(1f, 0f);
        Vector2 uvTop = new Vector2(0.5f, 1f);

        Vector2[] uv = new Vector2[]
        {
            uvTop, uvBottomLeft, uvBottomRight, // Base
            uvTop, uvBottomLeft, uvBottomRight, // Side 1
            uvTop, uvBottomLeft, uvBottomRight, // Side 2
            uvTop, uvBottomLeft, uvBottomRight  // Side 3
        };

        int[] triangles = new int[]
        {
            0, 1, 2,     // Base
            3, 4, 5,     // Side 1
            6, 7, 8,     // Side 2
            9, 10, 11    // Side 3
        };

        var mesh = new Mesh
        {
            name = "Tetrahedron",
            vertices = vertices,
            uv = uv,
            triangles = triangles
        };

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

        Undo.RegisterCreatedObjectUndo(obj, "Create Tetrahedron");
        Selection.activeGameObject = obj;

        return obj;
    }

    [MenuItem("GameObject/3D Object/Tetrahedron", false, 0)]
    public static void Create()
    {
        CreateObject();
    }
}