using UnityEngine;
using System.Collections;
using nFury.Utils.Core;
using rot.main.datamanager;

public class StartUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "new offline game"))
        {
            Debug.Log("-1111111 start new offline game button clicked");
            Service.Get<SignalManager>().startNewOfflineGameSignal.Dispatch();
        }
    }
}
