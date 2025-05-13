using Unity.Netcode;
using UnityEngine;
namespace FishSystem
{
    public abstract class ICommand : NetworkBehaviour
    {
        public abstract void Invoke(Fish fish);
        public virtual void Invoke(){

        }
       public virtual void Invoke(Collider col){

        }
    }
}