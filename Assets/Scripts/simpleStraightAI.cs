using UnityEngine;
using System.Collections;

public class simpleStraightAI : MonoBehaviour {

    public float velocity = 7;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += new Vector3(velocity * Time.deltaTime, 0, 0);
	}


}
