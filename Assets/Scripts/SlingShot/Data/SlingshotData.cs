using UnityEngine;

public class SlingshotData
{
    public Vector2 SlingShotLinePosition { get;  set; }
    
    // Line
    public Transform LeftStartPosition;
    public Transform RightStartPosition;
    public Transform IdlePosition;
    public Transform CenterPosition;

    public Transform IdleStrip;
    

    public void ResetSlingShotLinePosition(Vector2 position)
    {
        SlingShotLinePosition = position;
    }
    
}