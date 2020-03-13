using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float camOffset;

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x + camOffset, transform.position.y, transform.position.z);
    }
}
