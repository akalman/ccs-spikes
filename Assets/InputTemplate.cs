using UnityEngine;
using System.Collections;

public abstract class Parent : MonoBehaviour
{
    public GameObject reticule;

    protected SpellReticule _reticule;
    protected ReticuleFactory _reticuleFactory;

    public enum CharacterState
    {
        MOVING,
        CASTING
    }

    public enum CastingState
    {
        FREE,
        HOLD
    }

    protected CharacterController _controller;
    protected CharacterState _state;

    protected DirectionKeyConfig _dirConfig;
    protected SpellKeyConfig _spellConfig;

    protected ISpell _spell;

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

    public Vector3 CreateMoveVec(float f)
    {
        var vector = new Vector3(0f, 0f, 0f);
        if (UnityEngine.Input.GetKey(_dirConfig.up))
        {
            vector += new Vector3(0.0f, 0.0f, 1);
        }
        if (UnityEngine.Input.GetKey(_dirConfig.left))
        {
            vector += new Vector3(-1, 0.0f, 0.0f);
        }
        if (UnityEngine.Input.GetKey(_dirConfig.down))
        {
            vector += new Vector3(0.0f, 0.0f, -1);
        }
        if (UnityEngine.Input.GetKey(_dirConfig.right))
        {
            vector += new Vector3(1, 0, 0);
        }
        return vector.normalized * f;
    }

    public void Transition()
    {
        if (Input.GetKeyDown(_spellConfig.cast))
        {
            var pos = transform.position;
            pos.y = 0.1f;
            _reticule = _reticuleFactory.reticuleFor(_spell, pos, transform.rotation, this);
            _state = CharacterState.CASTING;
        }
        else if (Input.GetKeyUp(_spellConfig.cast) && _state == CharacterState.CASTING)
        {
            _reticule.Cast();
            _state = CharacterState.MOVING;
        }

        if (_reticule != null)
        {
            if (Input.GetKeyDown(_spellConfig.transition))
            {
                _reticule.Transition();
            }
        }
    }

    public void SpellFused()
    {
        _state = CharacterState.MOVING;
    }

    protected void Reticule()
    {
        _reticule.Move(CreateMoveVec(0.1f));
    }

    protected void Move()
    {
        _controller.SimpleMove(CreateMoveVec(1.0f));
    }
}

public struct DirectionKeyConfig
{
    public UnityEngine.KeyCode up { get; set; }
    public UnityEngine.KeyCode right { get; set; }
    public UnityEngine.KeyCode down { get; set; }
    public UnityEngine.KeyCode left { get; set; }
}

public struct SpellKeyConfig
{
    public UnityEngine.KeyCode cast { get; set; }
    public UnityEngine.KeyCode transition { get; set; }
}