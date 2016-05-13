using UnityEngine;
using System.Collections;
using Artemis;
using Artemis.Interface;

public class ViewComponent : IComponent {

    public GameObject go;
    public Vector3 lastPos;
    public Vector3 lastForward;
    public int lastFrame = -1;

    public ViewComponent(string path, Vector3 pos, Vector3 forward)
    {
        go = Object.Instantiate(Resources.Load(path)) as GameObject;
        go.transform.position = pos;
        go.transform.forward = forward;
    }

    public void SetPosition(Vector3 pos)
    {
        go.transform.position = pos;
    }

    public void SetForward(Vector3 forward)
    {
        go.transform.forward = forward;
    }

}
