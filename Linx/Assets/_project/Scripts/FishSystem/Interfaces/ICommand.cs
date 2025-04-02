using UnityEngine;
namespace FishSystem
{
    public abstract class ICommand : MonoBehaviour
    {
        public abstract void Invoke(Fish fish);
        public virtual void Invoke(){

        }
    }
}