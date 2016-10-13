using UnityEngine;
using System.Collections;

public class BallFlyController : MonoBehaviour {
    public float initialLinearSpeed = 10;
    public Vector3 initialLinearDirection = Vector3.forward;
    public float initialAngularSpeed = 0;
    public Vector3 initialAngularDirection = Vector3.up;

    private float linearSpeed = 10;
    private Vector3 linearDirection = Vector3.forward;
    private float angularSpeed = 0;
    private Vector3 angularDirection = Vector3.up;

    private float acceleration = 9.8f;
    private Vector3 accelerationDirection = Vector3.down;

    private float leftAcceleration = 5f;
    private float verticalAcceleration = 6f;

    private bool isRolling = false;

    private float rollingAcceleration = 1f;

    // Use this for initialization
    void Start () {
        linearSpeed = initialLinearSpeed;
        linearDirection = initialLinearDirection;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (isRolling) {
			transform.position += linearDirection * (linearSpeed * Time.deltaTime);
			Debug.LogWarning("rollingPosition = " + transform.position);
			Vector3 rotateAxis = RotateXZ(linearDirection, -Mathf.PI / 2);
			transform.Rotate(rotateAxis * (linearDirection * (linearSpeed * Time.deltaTime)).magnitude / 0.22f * 180f);

			float deltaSpeed = rollingAcceleration * Time.deltaTime;
			Vector3 newSpeed = linearDirection * linearSpeed + (-linearDirection) * deltaSpeed;

			Vector3 newLinearDirection = newSpeed.normalized;

			if (newLinearDirection.x * linearDirection.x < 0 || newLinearDirection.y * linearDirection.y < 0)
			{
				linearSpeed = 0f;
				rollingAcceleration = 0f;
			}
			else
			{
				linearSpeed = newSpeed.magnitude;
				linearDirection = newSpeed.normalized;
			}
		}
		else
		{
			transform.position += linearDirection * (linearSpeed * Time.deltaTime);

			float deltaSpeed = acceleration * Time.deltaTime;
			Vector3 newSpeed = linearDirection * linearSpeed
				+ accelerationDirection * (acceleration * Time.deltaTime)
				+ Vector3.left * (leftAcceleration * Time.deltaTime)
				//+ Vector3.up * (Mathf.Sign(linearDirection.y) * verticalAcceleration * Time.deltaTime)
				;
			/*if (Mathf.Sign(linearDirection.y) < 0 )
			{
				newSpeed += Vector3.down * (verticalAcceleration * Time.deltaTime);
			}*/
			linearSpeed = newSpeed.magnitude;
			linearDirection = newSpeed.normalized;

			if (transform.position.y < 0f)
			{
				HitGround();
			}
		}
    }

	private void HitGround()
	{
		Vector3 newSpeed = linearDirection * linearSpeed;
		newSpeed = new Vector3(newSpeed.x * 0.6f, newSpeed.y * -0.5f, newSpeed.z * 0.6f);

		Debug.LogWarning("newspeed.y = " + newSpeed.y);

		if (newSpeed.y < 0.4f)
		{
			newSpeed.y = 0f;
			acceleration = 0;
			leftAcceleration = 0;
			verticalAcceleration = 0;
			isRolling = true;
			Debug.LogWarning("start rolling rolling speed = " + newSpeed.magnitude);
		}

		linearSpeed = newSpeed.magnitude;
		linearDirection = newSpeed.normalized;
		transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
	}

    //Contrarotate vector v in X-Z plane
    private Vector3 RotateXZ(Vector3 v, float radAngle)
    {
        float ca = Mathf.Cos(radAngle);
        float sa = Mathf.Sin(radAngle);

        return new Vector3(v.x * ca - v.z * sa, v.y, v.x * sa + v.z * ca);
    }
}