using UnityEngine;
using System;
using System.Collections;


public static class Utils
{
    public static void Invoke(this MonoBehaviour mono, Action theDelegate, float delay)
    {
        mono.StartCoroutine(ExecuteAfterTime(theDelegate, delay));
    }

    private static IEnumerator ExecuteAfterTime(Action theDelegate, float delay)
    {
        yield return new WaitForSeconds(delay);
        theDelegate();
    }
}