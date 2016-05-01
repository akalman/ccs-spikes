using UnityEngine;
using System.Collections;

public abstract class Input : MonoBehaviour
{
    public GameObject reticule;
    protected GameObject _reticule;

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

    public Vector3 CreateMoveVec(float f)
    {
        var vector = new Vector3(0, 0, 0);
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
        if (UnityEngine.Input.GetKeyDown(_spellConfig.cast))
        {
            var pos = transform.position;
            pos.y = 0.1f;
            _reticule = SpellReticule.Create(reticule, pos, transform.rotation, _spell);
            _state = CharacterState.CASTING;
        }
        else if (UnityEngine.Input.GetKeyUp(_spellConfig.cast))
        {
            _reticule.SendMessage("Cast");
            Destroy(_reticule);
            _state = CharacterState.MOVING;
        }

        if (_reticule != null)
        {
            if (UnityEngine.Input.GetKeyDown(_spellConfig.transition))
            {
               _reticule.SendMessage("Transition");
            }
        }
    }

    protected void Reticule()
    {
        _reticule.SendMessage("Move", CreateMoveVec(0.1f));
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