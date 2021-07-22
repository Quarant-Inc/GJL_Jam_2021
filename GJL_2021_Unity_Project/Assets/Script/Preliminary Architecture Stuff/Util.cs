using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public static class Util
{
    public static string NormaliseString(this object obj)
    {
        return obj.ToString().Trim().ToLower();
    }

    public static void LoadScene(SCENE scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadSceneAsync((int)scene, mode);
    }

    public static bool GetCursorFromPlayerDir(out Vector3 dir)
    {
        RaycastHit hit;
        Ray ray = CameraFollow.Instance.Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 forward = (hit.point - Player.Instance.transform.position).normalized;
            dir = forward;
            return true;
        }
        dir = new Vector3();
        return false;
    }

    //https://answers.unity.com/questions/857827/pick-random-point-on-navmesh.html
    public static Vector3 GetRandomNavMeshLocation()
     {
         NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
 
         int maxIndices = navMeshData.indices.Length - 3;
         // Pick the first indice of a random triangle in the nav mesh
         int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
         int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);
         //Spawn on Verticies
         Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
 
         Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
         Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];
         //Eliminate points that share a similar X or Z position to stop spawining in square grid line formations
         if ((int)firstVertexPosition.x == (int)secondVertexPosition.x ||
             (int)firstVertexPosition.z == (int)secondVertexPosition.z
             )
         {
             point = GetRandomNavMeshLocation(); //Re-Roll a position - I'm not happy with this recursion it could be better
         }
         else
         {
             // Select a random point on it
             point = Vector3.Lerp(
                                             firstVertexPosition,
                                             secondVertexPosition, //[t + 1]],
                                             UnityEngine.Random.Range(0.05f, 0.95f) // Not using Random.value as clumps form around Verticies 
                                         );
         }
         //Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value); //Made Obsolete
 
         return point;
     }
    public static T GetRandomElement<T>(this IEnumerable<T> collection) where T : class
    {
        if (collection.Count() < 1)
        {
            return null;
        }
        return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
    }

    public static void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public static bool IsOfEnumType<T>(this object obj, out T returned) where T : struct
    {
        T[] values = GetValues<T>();
        foreach (T val in values)
        {
            if (obj.ToString().ToLower() == val.ToString().ToLower())
            {
                returned = val;
                return true;
            }
        }
        returned = default(T);
        return false;
    }

    public static T[] GetValues<T>() where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            Debug.LogFormat("{0} is not an enumeration.", typeof(T));
            return new T[] { };
        }
        return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }
}