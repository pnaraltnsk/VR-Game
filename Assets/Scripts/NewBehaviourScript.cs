using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Color gazedAt;
    public Color inactive;

    public GameObject player;

    private MeshRenderer myRenderer;

    public void OnPointerEnter()
    {
        GazedAt(true);
    }

    public void OnPointerExit()
    {
        GazedAt(false);
    }

    public void OnPointerClick()
    {
        Teleport();
    }

    public void GazedAt(bool gazing)
    {
        if (gazing)
        {
            myRenderer.material.color = gazedAt;
        }
        else
        {
            myRenderer.material.color = inactive;
        }
    }

    public void Teleport()
    {
        Vector3 pos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.position = pos;
    }

    public void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = inactive;
    }

    void Update()
    {

    }
}
