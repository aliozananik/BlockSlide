using Controllers;

namespace Particles
{
    public class ShrinkParticle : Particle
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
