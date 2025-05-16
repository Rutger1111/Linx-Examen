using UnityEngine;

public class SnapPosition : MonoBehaviour
{
    public bool hasObjectsInHere;

    public int snapId;
    public void setTrue(bool enable)
    {
        hasObjectsInHere = enable;
    }
}
