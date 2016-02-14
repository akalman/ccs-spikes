using UnityEngine;
using System.Collections;

public class InputScript2 : MonoBehaviour
{
    private CharacterController _controller;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var vector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.I))
        {
            vector += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            vector += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.K))
        {
            vector += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.L))
        {
            vector += new Vector3(1, 0, 0);
        }

        _controller.SimpleMove(vector);
    }
}
