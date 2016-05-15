using UnityEngine;
using System.Collections.Generic;

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

    public IDictionary<long, SpellReticule> WithinRadius(long id, float radius)
    {
        var inner = new Dictionary<long, SpellReticule>();
        var center = registry[id].transform.position;

        foreach (var pair in registry)
        {
            if (Vector3.Distance(pair.Value.transform.position, center) < radius && pair.Key != id)
            {
                inner.Add(pair.Key, pair.Value);
            }
        }
        return inner;
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
