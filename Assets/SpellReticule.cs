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
    }

    void Transition()
    {
        switch (_state)
        {
            case State.FREE:
                _state = State.LOCKED;
                break;
            case State.LOCKED:
                _state = State.FREE;
                break;
        }
    }

	void Move(Vector3 vector) {
        if (_state == State.FREE)
        {
            transform.position += vector;
        }
    }
}
