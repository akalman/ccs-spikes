using UnityEngine;
using System.Collections;

public class SpellReticule : MonoBehaviour {

    private State _state;
    private ISpell _spell;

    private enum State
    {
        FREE,
        LOCKED
    }
    
    // Use this for initialization
    void Start() {
        _spell = new PerfectSpell();
    }

    void Cast()
    {
        Debug.Log(_spell.effect());
    }
	
	// Update is called once per frame
    void Update()
    {
        Transition();
        switch (_state)
        {
            case State.FREE:
                Move();
                break;
            case State.LOCKED:
                break;
        }
    }

    void Transition()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _state = State.LOCKED;
        }
        else if (Input.GetKeyUp(KeyCode.Y))
        {
            _state = State.FREE;
        }
    }

	void Move() {
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
