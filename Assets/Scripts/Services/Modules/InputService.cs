using UnityEngine;

public class InputService : IInputService
{
    public bool IsFireButtonDown()
    {
        return Input.GetButtonDown("Fire1");
    }

    public bool IsFireButtonHeld()
    {
        return Input.GetButton("Fire1");
    }

    public bool IsFireButtonUp()
    {
        return Input.GetButtonUp("Fire1");
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }
}
