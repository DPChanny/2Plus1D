using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private float speed;

    private bool isEnd = false;

    void Update()
    {
        lineRenderer.SetPosition(0, startPos.position);
        lineRenderer.SetPosition(1, endPos.position);

        if (isEnd)
        {
            transform.position += (startPos.position - transform.position).normalized * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, startPos.position) < 0.1f)
            {
                isEnd = false;
            }
        }
        else
        {
            transform.position += (endPos.position - transform.position).normalized * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, endPos.position) < 0.1f)
            {
                isEnd = true;
            }
        }
        transform.Rotate(0f, 0f, 720 * Time.deltaTime);
    }
}
