using Controllers;

namespace Particles
{
    public class GrowParticle : Particle
    {
        void Start()
        {
            targetTransform = PlayerBoxController.instance.GrowParticleTransform;
        }
        
        void Update()
        {
            UpdatePosition();
        }
    }
}
