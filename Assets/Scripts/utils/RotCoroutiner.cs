using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Author: 		Sebastiaan Fehr (Seb@TheBinaryMill.com)
/// Date: 			March 2013
/// Summary:		Creates MonoBehaviour instance through which 
///                 static classes can call StartCoroutine.
/// Description:    Classes that do not inherit from MonoBehaviour, or static 
///                 functions within MonoBehaviours are inertly unable to 
///                 call StartCoroutene, as this function is not static and 
///                 does not exist on Object. This Class creates a proxy though
///                 which StartCoroutene can be called, and destroys it when 
///                 no longer needed.
/// </summary>
public class RotCoroutiner
{
    private static List<GameObject> cache = new List<GameObject>();

    public static Coroutine StartCoroutine(IEnumerator iterationResult)
    {
        //Create GameObject with MonoBehaviour to handle task.
        GameObject routeneHandlerGo = new GameObject("Coroutiner");
        CoroutinerInstance routeneHandler
            = routeneHandlerGo.AddComponent(typeof (CoroutinerInstance))
                as CoroutinerInstance;
        cache.Add(routeneHandlerGo);
        EventDelegate del = new EventDelegate(delegate() { cache.Remove(routeneHandlerGo); });
        return routeneHandler.ProcessWork(iterationResult, del);
    }

    public static void PauseAllCoroutine()
    {
        for (int i = 0; i < cache.Count; i++)
        {
            if (cache[i] != null)
            {
                CoroutinerInstance coroutinerInstance = cache[i].GetComponent<CoroutinerInstance>();
                if (coroutinerInstance.coroutineController.state == CoroutineState.Running)
                    coroutinerInstance.coroutineController.Pause();
            }
        }
    }

    public static void ResumeAllCoroutine()
    {
        for (int i = 0; i < cache.Count; i++)
        {
            if (cache[i] != null)
            {
                CoroutinerInstance coroutinerInstance = cache[i].GetComponent<CoroutinerInstance>();
                if (coroutinerInstance.coroutineController.state == CoroutineState.Paused)
                    coroutinerInstance.coroutineController.Resume();
            }
        }
    }
}

public class CoroutinerInstance : MonoBehaviour
{
    public CoroutineController coroutineController;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
    }

    public Coroutine ProcessWork(IEnumerator routine, EventDelegate onFinish)
    {
        if (routine == null)
        {
            throw new System.ArgumentNullException("routine");
        }

        coroutineController = new CoroutineController(routine);
        coroutineController.onFinish += delegate(CoroutineController controller)
        {
            onFinish.Execute();
            if (gameObject != null)
                Destroy(gameObject);
        };
        return StartCoroutine(coroutineController.Start());
    }
}