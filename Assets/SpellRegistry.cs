using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpellRegistry : MonoBehaviour {

    private IDictionary<long, SpellReticule> registry;

    private long lastLong;

	// Use this for initialization
	void Start () {
        registry = new Dictionary<long, SpellReticule>();
        lastLong = 0L;
	}
	
	// Update is called once per frame
	void Update () {}

    public IDictionary<long, SpellReticule> GetRegistry()
    {
        return registry;
    }

    public IEnumerable<KeyValuePair<long, SpellReticule>> WithinRadius(float radius, Vector3 center)
    {
        return registry.Select(pair => new
            {
                dist = Vector3.Distance(pair.Value.transform.position, center),
                pair = pair
            }
        )
        .OrderBy(anon => anon.dist)
        .TakeWhile(anon => anon.dist < radius)
        .Select(anon => anon.pair);
    }

    public long Register(SpellReticule reticule)
    {
        lastLong += 1;
        registry.Add(lastLong, reticule);
        return lastLong;
    }

    public void Remove(long id)
    {
        registry.Remove(id);
    }
}
