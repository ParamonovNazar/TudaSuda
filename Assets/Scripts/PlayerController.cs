using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PartOfPlane touchedPart;
    
    private const string TARGET_LAYERMASK="Clickable";

    void Update()
    {
        if (TouchStarted())
        {
            var pos = GetTouchPostion();
            var hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero,10f, LayerMask.GetMask(TARGET_LAYERMASK));
            
            if (hitInfo)
            {
                var part = hitInfo.collider.gameObject.GetComponent<PartOfPlane>();
                if (part != null)
                {
                    touchedPart = part;
                }
            }
        }
        
        if (TouchFinished())
        {
            if (touchedPart != null)
            {
                touchedPart.Plane.PartRelease();
                touchedPart = null;
            }
        }

        if (touchedPart != null)
        {
            var newPos = Camera.main.ScreenToWorldPoint(GetTouchPostion());
            if (touchedPart.Plane != null)
            {
                touchedPart.Plane.PartDragged(touchedPart, newPos);
            }
        }
    }

    private bool TouchStarted()
    {
        if (GameManager.Instance.IsMobile)
        {
            if (Input.touchCount>0)
            {
                return Input.GetTouch(0).phase.Equals(TouchPhase.Began);
            }

            return false;
        }
        else
        {
            return Input.GetMouseButtonDown(0);
        }
    }
    
    private bool TouchFinished()
    {
        if (GameManager.Instance.IsMobile)
        {
            if (Input.touchCount>0)
            {
                return Input.GetTouch(0).phase.Equals(TouchPhase.Ended);
            }

            return false;
        }
        else
        {
            return Input.GetMouseButtonUp(0);
        }
    }

    private Vector2 GetTouchPostion()
    {
        if (GameManager.Instance.IsMobile)
        {
            if (Input.touchCount>0)
            {
                return Input.GetTouch(0).position;
            }

            return Vector2.zero;
        }
        else
        {
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);;
        }
    }
}
