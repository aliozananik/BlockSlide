using UnityEngine;

namespace Objects
{
    public class SimpleShrinkObject : Interactable
    {
        public override void Particle()
        {
            ObjectPooler.instance.SpawnFromPool(
                tag,
                transform.position,
                Quaternion.identity);
        }
    }
}
