using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private Vector3 rotationDelta = Vector3.zero;
    [SerializeField] private Vector3 draggingDelta = Vector3.zero;
    [SerializeField] private float step ;
    [SerializeField] private float minAngle ;
    [SerializeField] private float maxAngle ;
    [SerializeField] private bool isPartDrag = false ;

    private void Awake()
    {
        foreach (var part in GetComponentsInChildren<PartOfPlane>())
        {
            part.Plane = this;
        }
    }

    private void Update()
    {
        if (isPartDrag)
        {
            transform.Rotate(rotationDelta * step);
        }
    }
    
    public void PartDragged(PartOfPlane part, Vector3 draggedPos)
    {
        draggedPos.z = transform.position.z;
        if (!isPartDrag)
        {
            draggingDelta = draggedPos - part.transform.position;
            isPartDrag = true;
        }

        var dir = draggedPos - transform.position - draggingDelta;
        float angle = Vector3.SignedAngle(part.transform.position-transform.position, dir, Vector3.forward);
        rotationDelta = new Vector3(0,0,angle);
    }

    public void PartRelease()
    {
        isPartDrag = false;
    }

    private void LateUpdate()
    {
        var curRot = transform.rotation.eulerAngles;
        if (curRot.z > 180)
            curRot.z = curRot.z - 360;
        transform.eulerAngles = 
            new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                Mathf.Clamp(curRot.z, minAngle, maxAngle)
                );
    }
}
