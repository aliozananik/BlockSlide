using UnityEngine;

namespace Objects
{
    public class DoubleShrinkObject : Interactable
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
