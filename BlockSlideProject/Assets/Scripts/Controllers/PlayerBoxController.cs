using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerBoxController : MonoBehaviour
    {
        #region Singleton

        public static PlayerBoxController instance;

        private void Awake()
        {
            instance = this;

            SetChangesForHeightAction += SetChangesForHeight;
        }

        private void OnDisable()
        {
            SetChangesForHeightAction -= SetChangesForHeight;
        }

        #endregion

        #region Movement

        [Header("Movement")]
        public float forwardSpeed;
        public float horizontalSpeed;
        
        [HideInInspector]public bool canMoveForward;
        [HideInInspector]public bool canMoveHorizontal;

        public float horizontalRightBorder;
        public float horizontalLeftBorder;

        #endregion

        #region Height
        
        [Header("Height")]
        public float heightChangeSpeed;
        private bool isNeedGrow;
        private bool heightNeedChange;
        private float targetHeight;
    
        public static Action<bool, float> SetChangesForHeightAction;

        #endregion

        #region PowerUps

        [Header("PowerUp")]
        public float speedPowerUpTime;
        public float speedPowerUpMultiplier;

        #endregion

        #region Character

        public Transform characterTransform;
        public GameObject character;
        public Animator characterAnimator;

        #endregion

        #region Particles

        public GameObject trailParticle;
        public Transform trailTransform;
        public Transform GrowParticleTransform;
        public GameObject finalConfetti;
    
        #endregion

    
        void Update()
        {
            if (heightNeedChange)
            {
                ChangeHeight();
            }

            if (canMoveHorizontal)
            {
                if (Input.touchCount > 0)
                {
                    MoveHorizontal();
                }
            }
            
            character.transform.position = characterTransform.position;
            trailParticle.transform.position = trailTransform.position;
        }

        private void FixedUpdate()
        {
            if (canMoveForward)
            {
                MoveForward();
            }
        }

        #region MovementFunctions
    
        void MoveForward()
        {
            transform.position += Vector3.forward * (Time.deltaTime * forwardSpeed);
            
        }

        void MoveHorizontal()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var touch = Input.GetTouch(0);

                var xMoveDifference = touch.deltaPosition.x;

                var selfPos = transform.position += transform.right * xMoveDifference / Screen.width * horizontalSpeed;

                if (selfPos.x > horizontalRightBorder)
                {
                    transform.position = new Vector3(
                        horizontalRightBorder,
                        selfPos.y,
                        selfPos.z);
                }

                if (selfPos.x < horizontalLeftBorder)
                {
                    transform.position = new Vector3(
                        horizontalLeftBorder,
                        selfPos.y,
                        selfPos.z);
                }
            }
        }
        
        #endregion

        #region HeightFunctions
    
        void SetChangesForHeight(bool _isIncrease, float _amount)
        {
            if (_isIncrease)
            {
                isNeedGrow = true;
                targetHeight = transform.localScale.y + _amount;
            }
            else
            {
                isNeedGrow = false;
                targetHeight = transform.localScale.y - _amount;
            }
        
            heightNeedChange = true;
        }

        void ChangeHeight()
        {
            if (isNeedGrow)
            {
                var scale = transform.localScale += Vector3.up * (Time.deltaTime * heightChangeSpeed);
                
                if (scale.y >= targetHeight)
                {
                    heightNeedChange = false;
                    transform.localScale = new Vector3(scale.x, targetHeight, scale.z);
                }
            }
            else
            {
                var scale = transform.localScale -= Vector3.up * (Time.deltaTime * heightChangeSpeed);
                
                if (scale.y <= targetHeight)
                {
                    heightNeedChange = false;
                    transform.localScale = new Vector3(scale.x, targetHeight, scale.z);

                    if (LevelManager.instance.gameLevelState == LevelManager.LevelStates.Win)
                    {
                        LevelManager.instance.AddCoin();
                    }
                }

                if (transform.localScale.y <= 0 && 
                    LevelManager.instance.gameLevelState == LevelManager.LevelStates.Started)
                {
                    LevelManager.SetStateAction?.Invoke(LevelManager.LevelStates.Fail);
                }
            }

            if (LevelManager.instance.gameLevelState == LevelManager.LevelStates.Win)
            {
                UIManager.instance.CoinIncreaseAnimation();
            }
        }

        #endregion

        #region PowerUpFunctions

        public IEnumerator SpeedPowerUp()
        {
            forwardSpeed *= speedPowerUpMultiplier;
            UIManager.instance.speedPowerUpButton.SetActive(false);

            yield return new WaitForSeconds(speedPowerUpTime);

            forwardSpeed /= speedPowerUpMultiplier;
            UIManager.instance.speedPowerUpButton.SetActive(true);
        }

        #endregion

        #region CharacterAnimation

        public void MoveAnim()
        {
            characterAnimator.Play("Move");
        }

        public void WinAnim()
        {
            characterAnimator.Play("Win");
        }

        public void FailAnim()
        {
            characterAnimator.Play("Fail");
        }

        public void IdleAnim()
        {
            characterAnimator.Play("Idle");
        }

        #endregion
    }
}
