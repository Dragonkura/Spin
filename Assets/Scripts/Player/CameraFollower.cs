using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Vector3 offset;
    // Start is called before the first frame update
    [SerializeField] Transform playerTranform;
    void Start()
    {
        offset = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = playerTranform.position + offset;
    }
}
