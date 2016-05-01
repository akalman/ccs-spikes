using UnityEngine;
using System.Collections;

public class SpellReticule : MonoBehaviour {

    private State _state;
    private ISpell _spell;
    private SpellRegistry _registry;
    private long _registryId;

    private enum State
    {
        FREE,
        LOCKED
    }
    
    // Use this for initialization
    void Start() {}

    void Cast()
    {
        var thing = _registry.WithinRadius(_registryId, 1f);

        foreach (var t in thing)
        {
            Debug.Log(t);
        }

        Debug.Log(_spell.effect());
        _registry.Remove(_registryId);
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

    public static GameObject Create(GameObject reticule, Vector3 pos, Quaternion rot, ISpell spell, SpellRegistry registry)
    {
        GameObject newObject = Instantiate(reticule, pos, rot) as GameObject;
        SpellReticule ret = newObject.GetComponent<SpellReticule>();
        
        ret._spell = spell;
        ret._registry = registry;
        ret._registryId = registry.Register(ret);

        return newObject;
    }
}
