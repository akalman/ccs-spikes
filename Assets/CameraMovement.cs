using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraMovement : MonoBehaviour
{
    public bool IsTwoPlayers;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    private Transform _transform;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var pois = GetPlayers().SelectMany<GameObject, Vector3>(GetPointOfInterest);
        var target = GetTargetCameraPosition(pois);

        _transform.position = target;
    }

    private IList<GameObject> GetPlayers()
    {
        if (IsTwoPlayers)
        {
            return new[] { PlayerOne, PlayerTwo };
        }
        return new[] { PlayerOne };
    }

    private IList<Vector3> GetPointOfInterest(GameObject player)
    {
        var pois = new Vector3[0].ToList();
        var playerPos = player.transform.position;
        var playerVel = player.GetComponent<CharacterController>().velocity;

        pois.Add(playerPos);
        if (playerVel != Vector3.zero)
        {
            pois.Add(playerPos + playerVel * 3);
        }

        return pois;
    }

    private Vector3 GetTargetCameraPosition(IEnumerable<Vector3> pointsOfInterest)
    {
        var xMin = float.MaxValue;
        var yMin = float.MaxValue;
        var zMin = float.MaxValue;
        var xMax = float.MinValue;
        var yMax = float.MinValue;
        var zMax = float.MinValue;

        foreach (var poi in pointsOfInterest)
        {
            if (poi.x > xMax) xMax = poi.x;
            if (poi.y > yMax) yMax = poi.y;
            if (poi.z > zMax) zMax = poi.z;
            if (poi.x < xMin) xMin = poi.x;
            if (poi.y < yMin) yMin = poi.y;
            if (poi.z < zMin) zMin = poi.z;
        }

        var dist = new Vector3(xMax, yMax, zMax) - new Vector3(xMin, yMin, zMin);
        var height = dist.magnitude / 2;
        var center = new Vector3(xMin, yMin, zMin) + (dist / 2);
        var target = center + (new Vector3(0, 1, -1) * height);
        Debug.Log(target);

        return target + new Vector3(0, 1, -1);
    }

    private Vector3 ClampComponentVector(
        Vector3 acceleration,
        float componentMagnitude,
        float velocityMagnitude)
    {
        if (componentMagnitude * velocityMagnitude < 0)
        {
            if (acceleration.magnitude > .2)
            {
                acceleration.Normalize();
                acceleration.Scale(new Vector3(.2f, .2f, .2f));
            }
        }
        else
        {
            if (acceleration.magnitude > .01)
            {
                acceleration.Normalize();
                acceleration.Scale(new Vector3(.01f, .01f, .01f));
            }
        }
        return acceleration;
    }
}
