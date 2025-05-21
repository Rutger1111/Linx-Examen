using UnityEngine;

namespace _New_Game.Scripts
{
    public class SnapPosition : MonoBehaviour
    {
        public bool hasObjectsInHere;

        public int snapId;
        public void SetTrue(bool enable)
        {
            hasObjectsInHere = enable;
        }
    }
}
