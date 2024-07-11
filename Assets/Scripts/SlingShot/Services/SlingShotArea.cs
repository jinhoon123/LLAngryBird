using UnityEngine;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotAreaMask;
    public bool IsWithinSlingshotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Physics2D.OverlapPoint(worldPosition, slingShotAreaMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
