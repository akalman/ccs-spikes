using UnityEngine;
using System.Collections;

public class SpellReticule : MonoBehaviour {

    private State _state;
    private ISpell _spell;
    private SpellRegistry _registry;
    private long _registryId;
    private Parent _parent;

    private enum State
    {
        FREE,
        LOCKED
    }
    
    // Use this for initialization
    void Start() {}

    public void Cast()
    {
        var thing = _registry.WithinRadius(_registryId, 1f);

        foreach (var t in thing)
        {
            t.Value.Fuse();
        }

        Debug.Log(_spell.effect());
        CleanUp();
    }

    public void Fuse()
    {
        Debug.Log(_spell.ToString() + " -- fused");
        _parent.SpellFused();
        CleanUp();
    }

    private void CleanUp()
    {
        _registry.Remove(_registryId);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
    void Update()
    {
    }

    public void Transition()
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

	public void Move(Vector3 vector) {
        if (_state == State.FREE)
        {
            transform.position += vector;
        }
    }

    public static SpellReticule Create(GameObject reticule, Vector3 pos, Quaternion rot, ISpell spell, SpellRegistry registry, Parent parent)
    {
        GameObject newObject = Instantiate(reticule, pos, rot) as GameObject;
        SpellReticule ret = newObject.GetComponent<SpellReticule>();
        
        ret._spell = spell;
        ret._registry = registry;
        ret._registryId = registry.Register(ret);
        ret._parent = parent;

        return ret;
    }
}
