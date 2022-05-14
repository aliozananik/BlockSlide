using UnityEngine;

namespace Particles
{
    public class Particle : MonoBehaviour
    {
        [HideInInspector]public Transform targetTransform;

        public void UpdatePosition()
        {
            transform.position = targetTransform.position;
        }
    }
}
