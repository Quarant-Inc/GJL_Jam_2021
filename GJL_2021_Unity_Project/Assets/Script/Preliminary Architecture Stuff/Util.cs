using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}