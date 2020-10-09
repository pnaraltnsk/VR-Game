using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject player;

    public void Move()
    {
        if(player.transform.position.z < 7700)
        {
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
            player.transform.position = pos;
        }
    }

    public void Start()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        player.transform.position = pos;
    }

    void Update()
    {
        Move();
    }
}
