using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickableObject : MonoBehaviour
{
    public bool isPickable = true;

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerInteractiveZone")
        {
            other.GetComponentInParent<pickUpObject>().ObjectToPickup = this.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerInteractiveZone")
        {
            other.GetComponentInParent<pickUpObject>().ObjectToPickup = null;

        }
    }
}
