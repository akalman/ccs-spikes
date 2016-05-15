using UnityEngine;
using System.Linq;

public class ReticuleFactory : MonoBehaviour {

    public GameObject reticuleFire;
    public GameObject reticuleWater;
    public GameObject reticuleEarth;
    public GameObject reticuleMetal;
    public GameObject reticuleAir;

    public GameObject reticuleAdam;
    public GameObject reticuleMatt;

    private SpellRegistry _registry;

    // Use this for initialization
    void Start () {
        _registry = FindObjectsOfType(typeof(SpellRegistry)).Cast<SpellRegistry>().First();
    }
	
	// Update is called once per frame
	void Update () {}

    public SpellReticule reticuleFor(ISpell spell, Vector3 pos, Quaternion rot, Parent parent)
    {
        GameObject obj;
        switch (spell.effect().element)
        {
            case Element.Adam:
                obj = reticuleAdam;
                break;

            case Element.Earth:
                obj = reticuleEarth;
                break;

            case Element.Fire:
                obj = reticuleFire;
                break;

            case Element.Lightning:
                obj = reticuleAir;
                break;

            case Element.Matt:
                obj = reticuleMatt;
                break;

            case Element.Metal:
                obj = reticuleMetal;
                break;

            case Element.Water:
            default:
                obj = reticuleWater;
                break;
        }
        return SpellReticule.Create(obj, pos, rot, spell, _registry, parent);
    }
}
