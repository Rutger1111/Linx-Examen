using UnityEngine;

public abstract class ICommand : MonoBehaviour
{
    public abstract void Invoke(Fish fish);
}
