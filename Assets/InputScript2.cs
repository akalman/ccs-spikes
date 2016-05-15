using UnityEngine;
using System.Linq;

public class InputScript2 : Parent
{

    // Use this for initialization
    void Start()
    {
        _dirConfig = new DirectionKeyConfig();
        _dirConfig.up = KeyCode.I;
        _dirConfig.right = KeyCode.L;
        _dirConfig.down = KeyCode.K;
        _dirConfig.left = KeyCode.J;

        _spellConfig = new SpellKeyConfig();
        _spellConfig.cast = KeyCode.N;
        _spellConfig.transition = KeyCode.M;

        _controller = GetComponent<CharacterController>();
        _state = CharacterState.MOVING;

        _spell = new TerribleSpell();
        _reticuleFactory = FindObjectsOfType(typeof(ReticuleFactory)).Cast<ReticuleFactory>().First();
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
}
