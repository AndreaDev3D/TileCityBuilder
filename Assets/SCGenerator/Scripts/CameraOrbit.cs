using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    private float speed = 5;
    private float currentAngleY = 0;
    private float currentAngleX = 0;
    private float moveSpeedX = 50;
    private float moveSpeedY = 30;
    private float moveSpeedZ = 200;

    public void LateUpdate()
    {
        target = GameObject.Find("SCG_camTarget").transform;
        var xmove = Input.GetAxis("Mouse X");
        var ymove = Input.GetAxis("Mouse Y");
        var zmove = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButton(1))
        {
            currentAngleY += xmove * speed;
            currentAngleX += (ymove*-1) * speed;
            transform.eulerAngles = new Vector3(currentAngleX, currentAngleY, 0.0f);
        }
        if (Input.GetMouseButton(0))
        {
            if (xmove > 0.25)
                transform.Translate(Vector3.left * (Time.deltaTime * moveSpeedX));
            if (xmove < -0.25)
                transform.Translate(Vector3.right * (Time.deltaTime * moveSpeedX));
            if (ymove > 0.25)
                transform.Translate(Vector3.down * (Time.deltaTime * moveSpeedY));
            if (ymove < -0.25)
                transform.Translate(Vector3.up * (Time.deltaTime * moveSpeedY));

        }
        if(zmove > 0)
            transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeedZ));
        if (zmove < 0)
            transform.Translate(Vector3.back * (Time.deltaTime * moveSpeedZ));
        if (Input.GetKey(KeyCode.F))
        {
            transform.LookAt(target.position);
        }
    }
}