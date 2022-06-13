using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour
{
    public CharacterController player;

    public Vector3 groundPosition;
    public Vector3 lastGroundPosition;
    public string groundName;
    public string lastGroundName;

    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (player.isGrounded)
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, player.height / 4.2f, -transform.up, out hit))
            {
                GameObject groundedIn = hit.collider.gameObject;
                groundName = groundedIn.name;
                groundPosition = groundedIn.transform.position;
            }

            if(groundPosition != lastGroundPosition && groundName == lastGroundName)
            {
                this.transform.position += groundPosition - lastGroundPosition;
            }

            lastGroundName = groundName;
            lastGroundPosition = groundPosition;

        }else if (!player.isGrounded)
        {
            lastGroundName = null;
            lastGroundPosition = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        player = this.GetComponent<CharacterController>();
        Gizmos.DrawWireSphere(transform.position, player.height / 4.2f);
    }
}
