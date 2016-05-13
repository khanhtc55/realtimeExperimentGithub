using UnityEngine;
using System.Collections;
using nFury.Utils.Core;
using rot.main.datamanager;

public class MainUIManager : MonoBehaviour {

    VisualSystem visualSystem;
    public float jumpCountdown;

	// Use this for initialization
	void Start () {
        visualSystem = Service.Get<VisualSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (jumpCountdown > 0) jumpCountdown -= Time.deltaTime;
	}

    void OnGUI()
    {
        if (visualSystem.isGameStarted)
        {
            
            if (GUI.Button(new Rect(10, 10, 50, 50), jumpCountdown>0? jumpCountdown.ToString(): "jump"))
            {
                if (jumpCountdown <= 0)
                {
                    jumpCountdown = 2.5f;
                    Service.Get<SignalManager>().sendUserInputSignal.Dispatch();
                }
            }
        }
        else
        {

            if (GUI.Button(new Rect(10, 10, 150, 50), "review"))
            {
                visualSystem.ReviewGame();
                Debug.Log("-111111111222 replay game");
            }

            if (GUI.Button(new Rect(10, 70, 150, 50), "play new game"))
            {
                Service.Get<SignalManager>().startNewOfflineGameSignal.Dispatch();
            }
        }
    }
}
