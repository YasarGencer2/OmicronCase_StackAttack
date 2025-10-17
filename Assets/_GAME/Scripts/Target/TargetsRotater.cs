using UnityEngine;

public class TargetsRotater : MonoBehaviour
{
    bool rotating = false;

    [SerializeField] float rotateSpeed = 50f;
    [SerializeField] float activateDistance = 15f;

    void Start()
    {
        rotating = false;
    }
    void Update()
    {
        if(GameHelper.Instance.IsGamePlaying() == false)
            return;
        if (rotating == false)
            CheckForStartRotating();
        else
            Rotate();
    }
    void CheckForStartRotating()
    {
        if (Vector3.Distance(GameHelper.Instance.PTransform.position, transform.position) > activateDistance)
            return;
        rotating = true;
    }
    void CheckForStopRotating()
    {
        if (Vector3.Distance(GameHelper.Instance.PTransform.position, transform.position) > activateDistance)
            return;
        rotating = false;
    }
    void Rotate()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == false)
                continue;
            child.rotation = Quaternion.identity;
        }
        CheckForStopRotating();
    }
}
