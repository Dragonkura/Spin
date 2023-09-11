using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotateSpeed;
    private Transform _transform;
    private void Awake()
    {
        _transform = this.GetComponent<Transform>();
    }
    private void Update()
    {
        var tmp = _transform.eulerAngles;
        tmp.y += Time.deltaTime * rotateSpeed;
        _transform.eulerAngles = tmp;
    }
}
