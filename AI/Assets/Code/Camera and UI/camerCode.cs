using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerCode : MonoBehaviour
{

    public float xSensetivity;
    public float ySensetivity;

    public float minimunY;
    public float maximumY;

    

    public float moveSpeed;

    private float rotX;
    public float rotY;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

       

    }





    private void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * xSensetivity;
        rotY += Input.GetAxis("Mouse Y") * ySensetivity;

        rotY = Mathf.Clamp(rotY, minimunY, maximumY);



        transform.rotation = Quaternion.Euler(-rotY, rotX, 0);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0;



        if (Input.GetKey(KeyCode.Q))
        {
            y = 10;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = -10;
        }

        Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }



}
