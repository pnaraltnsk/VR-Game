using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    public EditorPath PathToFollow;
    public int CurrentWayPointID = 0;
    private float reachDistance = 300f;
    public float rotationSpeed = 0.03f;
    public float speed = 30f;

    Vector3 last_position;

    // Start is called before the first frame update
    void Start()
    {
        last_position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(PathToFollow.nodes[CurrentWayPointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, PathToFollow.nodes[CurrentWayPointID].position, Time.deltaTime * speed);

		var rotation = Quaternion.LookRotation(PathToFollow.nodes[CurrentWayPointID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		

		if (distance <= reachDistance)
        {
            CurrentWayPointID++;
        }

        if(CurrentWayPointID >= PathToFollow.nodes.Count)
        {
            CurrentWayPointID = 0;
        }

		



	}

	
}
