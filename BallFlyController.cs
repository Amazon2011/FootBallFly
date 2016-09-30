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

	private float leftAcceleration = 10f;
	private float verticalAcceleration = 6f;

	// Use this for initialization
	void Start () {
		linearSpeed = initialLinearSpeed;
		linearDirection = initialLinearDirection;
	}

	// Update is called once per frame
	void Update () {
		float deltaSpeed = acceleration * Time.deltaTime;
	    Vector3 newSpeed = linearDirection * linearSpeed
			+ accelerationDirection * (acceleration * Time.deltaTime)
			+ Vector3.left * (leftAcceleration * Time.deltaTime)
			//+ Vector3.up * (Mathf.Sign(linearDirection.y) * verticalAcceleration * Time.deltaTime)
			;
	    if (Mathf.Sign(linearDirection.y) < 0 )
		{
			newSpeed += Vector3.down * (verticalAcceleration * Time.deltaTime);
		}
		linearSpeed = newSpeed.magnitude;
		linearDirection = newSpeed.normalized;

		transform.position += linearDirection * (linearSpeed * Time.deltaTime);

		if (transform.position.y < 0f)
		{
			transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
			linearSpeed = 0;
			acceleration = 0;
			angularSpeed = 0;
			leftAcceleration = 0;
			verticalAcceleration = 0;
		}
	}
}
