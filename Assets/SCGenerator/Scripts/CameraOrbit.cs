using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour {
    public Transform target;
    private float speed = 20;
    public float distance = 15;
    private float currentAngleY = 0;
    private float currentAngleX = 0;
    public Slider Zoom;


    public void Update() {
        #region UpdateCamerPositio
        target = GameObject.Find("SCG_camTarget").transform;
        currentAngleY += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        currentAngleX += Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        #endregion
        #region UpdateRotationCam
        Quaternion q = Quaternion.Euler(currentAngleX, currentAngleY, 0);
        Vector3 direction = q * Vector3.up;
        transform.position = target.position - direction * distance;
        transform.LookAt(target.position);
        #endregion
        #region ZoomIn
        distance = distance + Input.GetAxis("Mouse ScrollWheel") * 30;
        #endregion
    }
}