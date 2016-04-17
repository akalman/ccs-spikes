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
                Reticule();
                break;
        }
    }

    private void Transition()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var pos = transform.position;
            pos.y = 0.1f;
            _reticule = (GameObject) Instantiate(reticule, pos, transform.rotation);
            _state = CharacterState.CASTING;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _reticule.SendMessage("Cast");
            Destroy(_reticule);
            _state = CharacterState.MOVING;
        }

        if (_reticule != null)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _reticule.SendMessage("Lock");
            }
            else if (Input.GetKeyUp(KeyCode.Y))
            {
                _reticule.SendMessage("Unlock");
            }
        }
    }

    private void Reticule()
    {
        _reticule.SendMessage("Move", CreateVec(0.1f));
    }

    private Vector3 CreateVec(float f)
    {
        var vector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            vector += new Vector3(0.0f, 0.0f, f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            vector += new Vector3(-f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            vector += new Vector3(0.0f, 0.0f, -f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector += new Vector3(f, 0, 0);
        }
        return vector;
    }

    private void Move()
    {
        _controller.SimpleMove(CreateVec(1.0f));
    }
}
