using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour
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

        if (Input.GetKey(KeyCode.W))
        {
            vector += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            vector += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            vector += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector += new Vector3(1, 0, 0);
        }

        _controller.SimpleMove(vector);
    }
}
