using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject verticalPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CreateOther();
    }

    void CreateOther()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(verticalPlayer, playerTransform.position, playerTransform.rotation);
        }
    }
}
