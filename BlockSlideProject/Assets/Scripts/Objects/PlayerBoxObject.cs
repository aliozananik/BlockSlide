using Managers;
using UnityEngine;

namespace Objects
{
    public class PlayerBoxObject : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Grow"))
            {
                other.gameObject.GetComponent<Interactable>().Interaction();
            }

            if (other.gameObject.CompareTag("Shrink"))
            {
                other.gameObject.GetComponent<Interactable>().Interaction();
            }

            if (other.gameObject.CompareTag("Finish"))
            {
                LevelManager.SetStateAction?.Invoke(LevelManager.LevelStates.Win);
            }
        }
    }
}
