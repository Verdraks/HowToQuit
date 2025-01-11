using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class Utils
{
    #region IENUMERABLE
    public static T GetRandom<T>(this IEnumerable<T> elems)
    {
        if (elems.Count() == 0)
        {
            Debug.LogError("Try to get random elem from empty IEnumerable");
        }
        return elems.ElementAt(new System.Random().Next(0, elems.Count()));
    }
    #endregion

    #region COROUTINE
    public static IEnumerator Delay(float delay, Action ev)
    {
        yield return new WaitForSeconds(delay);
        ev?.Invoke();
    }
    #endregion

    #region SCENE
    /// <summary>
    /// Load Scene Asyncronely and call action at the end
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="loadMode"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerator LoadSceneAsync(string sceneIndex, LoadSceneMode loadMode, Action action)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, loadMode);

        yield return new WaitUntil(() => asyncLoad.isDone);

        action.Invoke();
    }

    /// <summary>
    /// Unload Scene Asyncronely and call action at the end
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerator UnloadSceneAsync(int sceneIndex, Action action)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneIndex);

        yield return new WaitUntil(() => asyncLoad.isDone);

        action.Invoke();
    }
    #endregion

    #region Others

    public static Quaternion SmoothDampRotation(Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime)
    {
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        Quaternion deltaRotation = target * Quaternion.Inverse(current);
        deltaRotation.ToAngleAxis(out angle, out axis);

        if (angle > 180.0f)
        {
            angle -= 360.0f;
        }

        float smoothedAngle = Mathf.SmoothDampAngle(0.0f, angle, ref smoothTime, Time.deltaTime);

        return Quaternion.AngleAxis(smoothedAngle, axis) * current;
    }

    #endregion
}