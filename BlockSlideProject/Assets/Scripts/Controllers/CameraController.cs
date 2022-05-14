using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [HideInInspector]public Vector3 target;

        [Header("Follow Variables")]
        public float followXDistance;
        public float followYDistance;
        public float followZDistance;

        void FixedUpdate()
        {
            FollowTarget();
        }

        void FollowTarget()
        {
            var playerTransform = PlayerBoxController.instance.transform;
            target = playerTransform.position + (playerTransform.localScale.y + 1) * Vector3.up;

            transform.position = 
                Vector3.Lerp(
                    transform.position,
                    new Vector3(0, target.y, target.z) + new Vector3(followXDistance,followYDistance,followZDistance),
                    .8f);
        }
    }
}

