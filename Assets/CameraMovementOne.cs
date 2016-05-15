using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraMovementOne : MonoBehaviour
{
    public bool IsTwoPlayers;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject CameraPrefab;

    private Transform _transform;
    private GameObject[] _otherCameras;
    private bool[] _needCamera;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
        _otherCameras = new GameObject[10];
        _needCamera = new bool[10];
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _needCamera.Length; i++)
        {
            _needCamera[i] = false;
        }

        var pois = GetPlayers().Select<GameObject, IEnumerable<Vector3>>(GetPointOfInterest);
        var target = GetTargetCameraPosition(pois.ToList());
        _transform.position = target;

        for (var i = 0; i < _needCamera.Length; i++)
        {
            if (!_needCamera[i] && _otherCameras[i] != null)
            {
                Destroy(_otherCameras[i]);
            }
        }
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
        if (_otherCameras[index] == null)
        {
            var newCamera = Instantiate<GameObject>(CameraPrefab);
            _otherCameras[index] = newCamera;
        }

        var camera = _otherCameras[index];
        camera.transform.position = GetTargetCameraPosition(new[] { pointsOfInterest[index] });
        camera.GetComponent<Camera>().depth = Camera.main.depth + 1;
        camera.GetComponent<Camera>().rect = new Rect(0, 0, .2f, .2f);
        _needCamera[index] = true;
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

        if ((topRightCorner - bottomLeftCorner).magnitude > 30)
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
                return center + new Vector3(0, Mathf.Max(heightPerVerticalDistance * 2, 5), 0);
            }

            if (verticalDistance > horizontalDistance / aspectRatio)
            {
                return center + new Vector3(0, Mathf.Max(verticalDistance * heightPerVerticalDistance, 5), 0);
            }

            return center + new Vector3(0, Mathf.Max((horizontalDistance / aspectRatio) * heightPerVerticalDistance, 5), 0);
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
