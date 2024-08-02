using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class camFollow : MonoBehaviour


{
    public Transform player;

    public float minXclamp = -0.95f;
    public float maxXclamp = 236.9f;

    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(player.transform.position.x, minXclamp, maxXclamp);
        transform.position = cameraPos;

    }


}
