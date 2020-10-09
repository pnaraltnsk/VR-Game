using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
	private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

	void OnParticleCollision(GameObject test)
	{
		Debug.Log("Got hit");
	}
}
