using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraMovementOne : MonoBehaviour
{
    public bool IsTwoPlayers;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public Camera CameraPrefab;

    private Transform _transform;
    private IList<Camera> _otherCameras;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var pois = GetPlayers().Select<GameObject, IEnumerable<Vector3>>(GetPointOfInterest);
        var target = GetTargetCameraPosition(pois.ToList());

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

    private IEnumerable<Vector3> GetPointOfInterest(GameObject player)
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

    public int GetOutlierIndex(IList<IEnumerable<Vector3>> pointsOfInterest)
    {
        var centers = pointsOfInterest.Select(pois => pois.Aggregate(Vector3.zero, (a, b) => a + b) / pois.Count());
        var center = centers.Aggregate(Vector3.zero, (a, b) => a + b) / centers.Count();

        var distances = centers.Select(c => (c - center).magnitude);

        return distances.ToList().IndexOf(distances.Max());
    }

    public void CreateBubble(IList<IEnumerable<Vector3>> pointsOfInterest, int index)
    {
        var newCamera = Instantiate<Camera>(CameraPrefab);
    }

    private Vector3 GetTargetCameraPosition(IList<IEnumerable<Vector3>> pointsOfInterest)
    {
        var xMin = float.MaxValue;
        var yMin = float.MaxValue;
        var zMin = float.MaxValue;
        var xMax = float.MinValue;
        var yMax = float.MinValue;
        var zMax = float.MinValue;

        foreach (var poi in pointsOfInterest.SelectMany(x => x))
        {
            if (poi.x > xMax) xMax = poi.x;
            if (poi.y > yMax) yMax = poi.y;
            if (poi.z > zMax) zMax = poi.z;
            if (poi.x < xMin) xMin = poi.x;
            if (poi.y < yMin) yMin = poi.y;
            if (poi.z < zMin) zMin = poi.z;
        }

        var bottomLeftCorner = new Vector3(xMin, yMin, zMin);
        var topRightCorner = new Vector3(xMax, yMax, zMax);
        var center = bottomLeftCorner + (topRightCorner - bottomLeftCorner) / 2;

        Debug.Log(topRightCorner - bottomLeftCorner);

        if ((topRightCorner - bottomLeftCorner).magnitude > 50)
        {
            var outlierIndex = GetOutlierIndex(pointsOfInterest);
            CreateBubble(pointsOfInterest, outlierIndex);

            return GetTargetCameraPosition(pointsOfInterest.Where((x, i) => i != outlierIndex).ToList());
        }
        else
        {
            var horizontalDistance = xMax - xMin;
            var verticalDistance = zMax - zMin;
            var aspectRatio = Screen.width / Screen.height;
            var heightPerVerticalDistance = .5f / Mathf.Tan(45 * (Mathf.PI / 180));

            if (verticalDistance < 2 && horizontalDistance < 2 * aspectRatio)
            {
                return center + new Vector3(0, heightPerVerticalDistance * 2, 0);
            }

            if (verticalDistance > horizontalDistance / aspectRatio)
            {
                return center + new Vector3(0, verticalDistance * heightPerVerticalDistance);
            }

            return center + new Vector3(0, (horizontalDistance / aspectRatio) * heightPerVerticalDistance);
        }
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
