using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform cameraTarget;
	public float smoothSpeed = 0.125f;
	public Vector3 offset;


	// Attach script to main camera

	void FixedUpdate()
	{
		Vector3 desiredPosition = cameraTarget.position + offset;                                   // desiredPosition vector is made using the camerTarget's position and the offset vector
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // smoothPosition is created by using lerp with the camera's current position and the desired postion
		transform.position = smoothedPosition;                                                      // cameras transform is changed to the new smoothed position, resulting in smooth camera movement 
	}


	// Axonometric projection calculations:
	private void OnPreCull()
	{
		var matrix = Camera.main.transform.worldToLocalMatrix;
		matrix.SetRow(2, -matrix.GetRow(2));
		matrix.SetColumn(2, 1e-3f * matrix.GetColumn(2) - new Vector4(0, -1, 0, 0));
		Camera.main.worldToCameraMatrix = matrix;
	}

}
