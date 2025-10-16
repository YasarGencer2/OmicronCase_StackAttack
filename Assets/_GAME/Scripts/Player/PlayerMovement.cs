using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float forwwardSpeed;
    [SerializeField] float sidewaysSpeedMultiplier = 1f;
    [SerializeField] TrailRenderer trail;
    Camera cam => Camera.main;
    Vector3 offset;
    bool dragging;
    bool didFirstInput = false;


    void OnEnable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted += OnLevelLoadStarted;
    }
    void OnDisable()
    {
        GameEventSystem.Instance.OnLevelLoadStarted -= OnLevelLoadStarted;
    } 
    private void OnLevelLoadStarted()
    {
        didFirstInput = false;
        dragging = false;
        offset = Vector3.zero;
        trail.emitting = false;
        trail.Clear();
        transform.position = new Vector3(0, 0, 0);
        trail.emitting = true;
        trail.Clear();
    }
    void Update()
    {
        if (didFirstInput == false && Input.GetMouseButtonDown(0))
            StartDrag();
        if (GameHelper.Instance.IsGamePlaying() == false)
            return;
        ForwardMovement();
        SidewaysMovement();
    }
    void ForwardMovement()
    {
        transform.Translate(Vector3.up * forwwardSpeed * Time.deltaTime);
    }
    void SidewaysMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }
        if (dragging)
        {
            Drag();
        }
    }
    void StartDrag()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
        if (didFirstInput == false)
        {
            didFirstInput = true;
            GameEventSystem.Instance.Trigger_FirstInput();
        }
    }
    void StopDrag()
    {
        dragging = false;
    }
    void Drag()
    {
        Vector3 targetPos = GetMouseWorldPos() + offset;
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), sidewaysSpeedMultiplier * Time.deltaTime);
    }

    Vector3 GetMouseWorldPos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePos);
    }
    void StartTrailMovement()
    {

    }
}
