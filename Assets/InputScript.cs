using UnityEngine;
using System.Linq;

public class InputScript : Input
{
  
    // Use this for initialization
    void Start()
    {
        _dirConfig = new DirectionKeyConfig();
        _dirConfig.up = KeyCode.W;
        _dirConfig.right = KeyCode.D;
        _dirConfig.down = KeyCode.S;
        _dirConfig.left = KeyCode.A;

        _spellConfig = new SpellKeyConfig();
        _spellConfig.cast = KeyCode.F;
        _spellConfig.transition = KeyCode.Y;

        _controller = GetComponent<CharacterController>();
        _state = CharacterState.MOVING;

        _spell = new PerfectSpell();
        _spellRegistry = FindObjectsOfType(typeof(SpellRegistry)).Cast<SpellRegistry>().First();
        _spellRegistry.GetRegistry();
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
