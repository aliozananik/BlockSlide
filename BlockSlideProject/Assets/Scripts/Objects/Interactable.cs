using Controllers;
using UnityEngine;

namespace Objects
{
    public abstract class Interactable : MonoBehaviour
    {
        public float heightEffect;
        public bool isEffectGrow;
        public new string tag;

        public void Interaction()
        {
            PlayerBoxController.SetChangesForHeightAction?.Invoke(isEffectGrow, heightEffect);
            
            Particle();
            
            gameObject.SetActive(false);
        }

        public abstract void Particle();
    }
}
