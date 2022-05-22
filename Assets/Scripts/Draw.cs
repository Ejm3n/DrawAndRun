using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    [SerializeField] private float VertexDist;
    [SerializeField] private float Scale;
    [SerializeField] private Transform DrawZone;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private GameObject linePrefab;
    private GameObject currentLine;
    private bool Started;

    private bool isMobile()
    {
        return Application.isMobilePlatform;
    }

    private LineRenderer lineRenderer;
    public List<Vector3> fingerPositions;
    public List<Vector3> fingerDelta;
    public List<Vector3> drawDelta;

    public List<Vector3> fixedPoints()
    {
        List<Vector3> Points = new List<Vector3>();
        float xCos = Mathf.Cos(Mathf.PI / 3);
        for (int i = 0; i < fingerPositions.Count; i++)
        {
            Vector3 tempPoint = (fingerPositions[i] - drawDelta[i]);
            Vector3 CurrPoint = new Vector3(tempPoint.x * xCos * Scale, 0, tempPoint.z * Scale);
            Points.Add(CurrPoint + Offset);
        }

        return Points;
    }

    public delegate void OnDrawEnded(List<Vector3> Points);
    public static OnDrawEnded OnDrawEnd;

    private Vector3 ToWorldPoint()
    {
        Vector3 InputPos;

        if (isMobile())
        {
            InputPos = Input.GetTouch(0).position;
        }
        else
        {
            InputPos = Input.mousePosition;
        }
        Ray ray = Camera.main.ScreenPointToRay(InputPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1 << 6))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void CreateLine()
    {
        Vector3 Pos = ToWorldPoint();
        if (Pos == Vector3.zero)
            return;

        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, DrawZone);
        lineRenderer = currentLine.GetComponent<LineRenderer>();

        fingerPositions.Clear();
        fingerDelta.Clear();
        drawDelta.Clear();

        fingerPositions.Add(Pos);
        fingerPositions.Add(Pos);
        fingerDelta.Add(transform.position);
        fingerDelta.Add(transform.position);
        drawDelta.Add(DrawZone.position);
        drawDelta.Add(DrawZone.position);

        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(0, fingerPositions[1]);

        Started = true;
    }

    private void UpdateLine()
    {
        Vector3 Pos = ToWorldPoint();
        if (Pos == Vector3.zero)
            return;
        if (Vector2.Distance(Pos, fingerPositions[fingerPositions.Count - 1]) > VertexDist)
        {
            fingerPositions.Add(Pos);
            fingerDelta.Add(transform.position);
            drawDelta.Add(DrawZone.position);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, Pos);
        }
    }

    private void FollowLine()
    {
        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 Offset = fingerDelta[i] - transform.position;
            lineRenderer.SetPosition(i, fingerPositions[i] - Offset);
        }
    }

    private void ClearLine()
    {
        Started = false;
        Destroy(lineRenderer.gameObject);
        fingerPositions.Clear();
        fingerDelta.Clear();
        drawDelta.Clear();
    }

    private void MobileUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CreateLine();
        }
        if (Input.touchCount > 0 && Started)
        {
            UpdateLine();
        }
        if (Input.touchCount == 0 && Started)
        {
            OnDrawEnd?.Invoke(fixedPoints());
            ClearLine();
        }
    }
    private void PCUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        if (Input.GetMouseButton(0) && Started)
        {
            UpdateLine();
        }
        if (Input.GetMouseButtonUp(0) && Started)
        {
            OnDrawEnd?.Invoke(fixedPoints());
            ClearLine();
        }
    }

    private void Update()
    {
        if(isMobile())
        {
            MobileUpdate();
        }
        else
        {
            PCUpdate();
        }


    }

    private void FixedUpdate()
    {
        if (Started)
        {
            FollowLine();
        }
    }
}
