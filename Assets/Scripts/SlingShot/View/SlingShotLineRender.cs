using UnityEngine;

public class SlingShotLineRender : MonoBehaviour
{
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;
    public GameObject idleStrip; // 프라이빗 시리얼라이즈필드에서 퍼블릭으로 변경
    
    public void InitializeLineRender()
    {
        leftLineRenderer = transform.Find("StripL").GetComponent<LineRenderer>();
        rightLineRenderer = transform.Find("StripR").GetComponent<LineRenderer>();
        
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
    }
    
    public void SetLine(Vector2 position, Transform leftStartPosition, Transform rightStartPosition)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);
    }

    public void SetLeftLineRender(bool isActive)
    {
        leftLineRenderer.enabled = isActive;
    }
    
    public void SetRightLineRender(bool isActive)
    {
        rightLineRenderer.enabled = isActive;
    }
    
    public void SetStrip(bool isActive)
    {
        idleStrip.SetActive(isActive);
    }
}