using UnityEngine;
using System.Collections;

public class SpellReticule : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    void Cast ()
    {
        Debug.Log("boom!");
    }
	
	// Update is called once per frame
	void Update () {
        var vector = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.W))
        {
            vector += new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            vector += new Vector3(-0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            vector += new Vector3(0.0f, 0.0f, -0.1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector += new Vector3(0.1f, 0, 0);
        }

        transform.position += vector;
    }
}
