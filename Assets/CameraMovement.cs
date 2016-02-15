using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public bool IsTwoPlayers;
    public GameObject PlayerOne;

    private Transform _transform;
    private Vector3 _currentVelocity;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTwoPlayers)
        {
            var playerPos = PlayerOne.transform.position;
            var playerVel = PlayerOne.GetComponent<CharacterController>().velocity;
            var verticalOffset = new Vector3(0, 15, 0);
            var horizontalOffset = new Vector3(0, 0, -15);

            if (playerVel != Vector3.zero)
            {
                playerPos += playerVel * 3;
            }
            
            var destination = playerPos + verticalOffset + horizontalOffset;
            var acceleration = destination - _transform.position - _currentVelocity;

            // clamp acceleration and apply to velocity
            var xAccelerationVector = new Vector3(acceleration.x, 0, 0);
            _currentVelocity += ClampComponentVector(xAccelerationVector, acceleration.x, _currentVelocity.x);

            var yAccelerationVector = new Vector3(0, acceleration.y, 0);
            _currentVelocity += ClampComponentVector(yAccelerationVector, acceleration.y, _currentVelocity.y);

            var zAccelerationVector = new Vector3(0, 0, acceleration.z);
            _currentVelocity += ClampComponentVector(zAccelerationVector, acceleration.z, _currentVelocity.z);

            // apply velocity to position
            if (_currentVelocity.magnitude > 1)
            {
                _currentVelocity.Normalize();
            }

            _transform.position += _currentVelocity;
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
