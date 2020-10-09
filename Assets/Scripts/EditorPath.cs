using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPath : MonoBehaviour
{
    public Color rayColor;
    public List<Transform> nodes;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Transform[] theArray = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < theArray.Length; i++)
        {
            if(theArray[i] != transform)
            {
                nodes.Add(theArray[i]);
            }
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 position = nodes[i].position;
            Vector3 previous = nodes[(i - 1 + nodes.Count) % nodes.Count].position;
            Gizmos.DrawLine(previous, position);
            Gizmos.DrawWireSphere(position, 3f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
