using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpObject : MonoBehaviour
{

    public GameObject ObjectToPickup;
    public GameObject PickekedObject;
    public Transform interactiveZone;

    private void Update()
    {
        if (
            ObjectToPickup != null &&
            ObjectToPickup.GetComponent<pickableObject>().isPickable == true &&
            PickekedObject == null
            )
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickekedObject = ObjectToPickup;
                PickekedObject.GetComponent<pickableObject>().isPickable = false;
                PickekedObject.transform.SetParent(interactiveZone);
                PickekedObject.transform.position = new Vector3(PickekedObject.transform.position.x, PickekedObject.transform.position.y + 1f, PickekedObject.transform.position.z);
                PickekedObject.GetComponent<Rigidbody>().useGravity = false;
                PickekedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }else if(PickekedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickekedObject.GetComponent<pickableObject>().isPickable = true;
                PickekedObject.transform.SetParent(null);
                PickekedObject.GetComponent<Rigidbody>().useGravity = true;
                PickekedObject.GetComponent<Rigidbody>().isKinematic = false;
                PickekedObject = null;
            }
        }
    }
}
