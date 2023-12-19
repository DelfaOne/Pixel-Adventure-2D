using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Serialize field then drag and drop the player on it to access to the player Transform object
    [SerializeField] private Transform player;

    private void Update()
    {
        //Keep the camera is own z value
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
