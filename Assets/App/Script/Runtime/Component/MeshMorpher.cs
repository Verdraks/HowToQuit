using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshMorpher : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float duration = 2f;
    [SerializeField] private AnimationCurve fadeCurve;
    
    [Header("References")] 
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh meshTarget1;
    [SerializeField] private Mesh meshTarget2;
    
    private Mesh morphedMesh; 
    private Vector3[] verticesMeshTarget1; 
    private Vector3[] verticesMeshTarget2; 
    private Vector3[] interpolatedVertices; 

    private Vector3[] normalsMeshTarget1; 
    private Vector3[] normalsMeshTarget2; 
    private Vector3[] interpolatedNormals; 

    private int[] trianglesMesh1; // Triangles du mesh1
    private int[] trianglesMesh2; // Triangles du mesh2

    void Awake()
    {
        verticesMeshTarget1 = meshTarget1.vertices;
        verticesMeshTarget2 = AdjustVertexCount(meshTarget2.vertices, meshTarget1.vertexCount);
        interpolatedVertices = new Vector3[verticesMeshTarget1.Length];

        normalsMeshTarget1 = meshTarget1.normals;
        normalsMeshTarget2 = AdjustVertexCount(meshTarget2.normals, meshTarget1.vertexCount);
        interpolatedNormals = new Vector3[normalsMeshTarget1.Length];
        
        trianglesMesh1 = meshTarget1.triangles;
        trianglesMesh2 = AdjustTriangleCount(meshTarget2.triangles, meshTarget1.vertexCount);

        morphedMesh = new Mesh();
    }

    public void TransitionMesh(bool forward)
    {
        StopAllCoroutines();
        StartCoroutine(TransitionCoroutine(forward));
    }

    IEnumerator TransitionCoroutine(bool forward)
    {
        float elapsedTime = 0f;
        Vector3[] startVertices = forward ? verticesMeshTarget1 : verticesMeshTarget2;
        Vector3[] targetVertices = forward ? verticesMeshTarget2 : verticesMeshTarget1;
        Vector3[] startNormals = forward ? normalsMeshTarget1 : normalsMeshTarget2;
        Vector3[] targetNormals = forward ? normalsMeshTarget2 : normalsMeshTarget1;
        int[] targetTriangles = forward ? trianglesMesh2 : trianglesMesh1;
        morphedMesh.Clear();
        meshFilter.mesh = morphedMesh;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float fade = fadeCurve.Evaluate(t);

            for (int i = 0; i < verticesMeshTarget1.Length; i++)
            {
                
                interpolatedVertices[i] = Vector3.Lerp(startVertices[i], targetVertices[i], fade);
                
                interpolatedNormals[i] = Vector3.Lerp(startNormals[i], targetNormals[i], fade).normalized;
            }
            
            morphedMesh.vertices = interpolatedVertices;
            morphedMesh.normals = interpolatedNormals;
            morphedMesh.triangles = trianglesMesh1;
            morphedMesh.RecalculateBounds();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        meshFilter.mesh = forward ? meshTarget2 : meshTarget1;
    }

    /// <summary>
    /// Ajuste le nombre de sommets pour correspondre à un autre mesh.
    /// </summary>
    private Vector3[] AdjustVertexCount(Vector3[] sourceVertices, int targetCount)
    {
        List<Vector3> adjustedVertices = new List<Vector3>(sourceVertices);

        // Ajouter des sommets si nécessaire
        while (adjustedVertices.Count < targetCount)
        {
            adjustedVertices.Add(adjustedVertices[Random.Range(0, adjustedVertices.Count)]);
        }

        // Supprimer des sommets si nécessaire
        while (adjustedVertices.Count > targetCount)
        {
            adjustedVertices.RemoveAt(Random.Range(0, adjustedVertices.Count));
        }

        return adjustedVertices.ToArray();
    }

    /// <summary>
    /// Ajuste le nombre de triangles pour correspondre au nombre de sommets.
    /// </summary>
    private int[] AdjustTriangleCount(int[] sourceTriangles, int targetVertexCount)
    {
        List<int> adjustedTriangles = new List<int>(sourceTriangles);

        // Filtrer les indices hors des limites
        adjustedTriangles.RemoveAll(index => index >= targetVertexCount);

        return adjustedTriangles.ToArray();
    }
}