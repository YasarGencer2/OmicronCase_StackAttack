using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float forwwardSpeed;

    [SerializeField] float sidewaysSpeedMultiplier = 1f;
    Camera cam => Camera.main;
    Vector3 offset;
    bool dragging;

    void Update()
    {
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
            offset = transform.position - GetMouseWorldPos();
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), sidewaysSpeedMultiplier * Time.deltaTime);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePos);
    }
}
