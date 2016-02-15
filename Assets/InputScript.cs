using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour
{
    public GameObject reticule;
    private GameObject _reticule;

    private enum CharacterState
    {
        MOVING,
        CASTING
    }

    private CharacterController _controller;
    private CharacterState _state;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _state = CharacterState.MOVING;
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
        switch (_state)
        {
            case CharacterState.MOVING:
                Move();
                break;
            default:
                Cast();
                break;
        }
    }

    private void Transition()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _reticule = (GameObject) Instantiate(reticule, transform.position, transform.rotation);
            _state = CharacterState.CASTING;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _reticule.SendMessage("Cast");
            //_reticule = null;
            Destroy(_reticule);
            _state = CharacterState.MOVING;
        }
    }

    private void Cast()
    {

    }

    private void Move()
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
