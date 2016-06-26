using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets;

public class CameraMovementOne : MonoBehaviour
{
    public bool IsTwoPlayers;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject CameraPrefab;

    private int _count = 0;

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

        var pois = GetPlayers().Select<GameObject, PlayerInterests>(GetPointOfInterest);
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

    private PlayerInterests GetPointOfInterest(GameObject player)
    {
        var pois = new PlayerInterests(player.transform.position);
        var playerVel = player.GetComponent<CharacterController>().velocity;

        if (playerVel != Vector3.zero)
        {
            pois.Add(pois.GetPlayerPosition() + playerVel * 3);
        }

        return pois;
    }

    public int GetOutlierIndex(IList<PlayerInterests> pointsOfInterest)
    {
        return pointsOfInterest
            .IndexOf(pointsOfInterest.Last(poi => !poi.Equals(pointsOfInterest.First())));
    }

    private List<Rect> IndexToCornerRect = new List<Rect>
    {
        new Rect(0, .8f, .2f, .2f),
        new Rect(.8f, .8f, .2f, .2f),
        new Rect(0, 0, .2f, .2f),
        new Rect(.8f, 0, .2f, .2f)
    };

    public void CreateBubble(IList<PlayerInterests> pointsOfInterest, int index)
    {
        if (_otherCameras[index] == null)
        {
            var newCamera = Instantiate<GameObject>(CameraPrefab);
            _otherCameras[index] = newCamera;
        }

        var camera = _otherCameras[index];
        camera.transform.position = GetTargetCameraPosition(new[] { pointsOfInterest[index] });
        camera.GetComponent<Camera>().depth = Camera.main.depth + 1;
        camera.GetComponent<Camera>().rect = IndexToCornerRect[index];
        _needCamera[index] = true;
    }

    private Vector3 GetTargetCameraPosition(IList<PlayerInterests> pointsOfInterest)
    {
        var xMin = float.MaxValue;
        var zMin = float.MaxValue;
        var xMax = float.MinValue;
        var zMax = float.MinValue;

        foreach (var poi in pointsOfInterest.SelectMany(x => x))
        {
            if (poi.x > xMax) xMax = poi.x;
            if (poi.z > zMax) zMax = poi.z;
            if (poi.x < xMin) xMin = poi.x;
            if (poi.z < zMin) zMin = poi.z;
        }

        var aspect = Screen.width / Screen.height;
        var allowedZ = Mathf.Sqrt((30 * 30) / (1 + aspect * aspect));
        var allowedX = allowedZ * aspect;

        if (xMax - xMin > allowedX || zMax - zMin > allowedZ)
        {
            var outlierIndex = GetOutlierIndex(pointsOfInterest);
            CreateBubble(pointsOfInterest, outlierIndex);
            var placeholder = GetPlaceholderPointOfInterest(pointsOfInterest, outlierIndex, xMin, xMax, zMin, zMax);
            pointsOfInterest[outlierIndex] = placeholder;

            return GetTargetCameraPosition(pointsOfInterest);
        }
        else
        {
            var center = (new Vector3(xMin, 0, zMin) + new Vector3(xMax, 0, zMax)) / 2;
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
                return center + new Vector3(0, Mathf.Max(verticalDistance * heightPerVerticalDistance + 1, 5), 0);
            }

            return center + new Vector3(0, Mathf.Max((horizontalDistance / aspectRatio) * heightPerVerticalDistance + 1, 5), 0);
        }
    }

    private PlayerInterests GetPlaceholderPointOfInterest(
        IList<PlayerInterests> pois, int outlierIndex,
        float xMin, float xMax, float zMin, float zMax)
    {
        return pois.First();
    }
}
