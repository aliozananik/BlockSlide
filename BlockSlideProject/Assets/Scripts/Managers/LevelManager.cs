using System;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {

        #region Singleton

        public static LevelManager instance;

        private void Awake()
        {
            instance = this;

            SetStateAction += SetState;
        }

        private void OnDisable()
        {
            SetStateAction -= SetState;
        }

        #endregion

        #region LevelSettings

        [Header("Settings")]
        public float heightCoinMultiplier;
        public float speedPowerUpTime;

        #endregion

        #region States

        public enum LevelStates
        {
            Loaded,
            Started,
            Fail,
            Win
        }

        [HideInInspector]public LevelStates gameLevelState;

        public static Action<LevelStates> SetStateAction;

        #endregion

        #region Stats

        [Header("Stats")]
        [HideInInspector]public int lastPlayedLevel;
        [HideInInspector]public int totalCoin;
        [HideInInspector]public int toEarnCoin;

        #endregion

        void Start()
        {
            UpdateTotalCoins();
            UpdateLevelNames();
        }

        void SetState(LevelStates _levelState)
        {
            gameLevelState = _levelState;
            
            switch (_levelState)
            {
                case LevelStates.Started:

                    PlayerBoxController.instance.canMoveForward = true;
                    PlayerBoxController.instance.canMoveHorizontal = true;
                    UIManager.instance.LevelStarted();
                    PlayerBoxController.instance.MoveAnim();
                
                    break;
                case LevelStates.Fail:

                    PlayerBoxController.instance.canMoveForward = false;
                    PlayerBoxController.instance.canMoveHorizontal = false;
                    UIManager.instance.LevelFailed();
                    PlayerBoxController.instance.FailAnim();
                
                    break;
                case LevelStates.Win:

                    PlayerBoxController.instance.canMoveForward = false;
                    PlayerBoxController.instance.canMoveHorizontal = false;
                    LevelWin();

                    break;
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadSceneAsync(0);
            }
        }

        void LevelWin()
        {
            UIManager.instance.LevelWin();
            PlayerBoxController.instance.WinAnim();
            LevelWinAnimation();
            CalculateToEarnCoin();
            PlayerBoxController.instance.finalConfetti.SetActive(true);

            lastPlayedLevel++;
            PlayerPrefs.SetInt("lastPlayedLevel", lastPlayedLevel);
        }

        float CalculateToEarnCoin()
        {
            
            toEarnCoin = Mathf.RoundToInt(PlayerBoxController.instance.transform.localScale.y * heightCoinMultiplier);

            return toEarnCoin;
        }

        void LevelWinAnimation()
        {
            PlayerBoxController.SetChangesForHeightAction?.Invoke(false,
                PlayerBoxController.instance.transform.localScale.y);
        }

        public void AddCoin()
        {
            totalCoin += toEarnCoin;

            PlayerPrefs.SetInt("totalCoin", totalCoin);
            
            UIManager.instance.CoinAddAnimation();
        }

        void UpdateTotalCoins()
        {
            if (PlayerPrefs.HasKey("totalCoin"))
            {
                totalCoin = PlayerPrefs.GetInt("totalCoin");
            }
            
            UIManager.instance.UpdateTotalCoins();
        }

        void UpdateLevelNames()
        {
            if (PlayerPrefs.HasKey("lastPlayedLevel"))
            {
                lastPlayedLevel = PlayerPrefs.GetInt("lastPlayedLevel");
            }
            
            UIManager.instance.UpdateLevelNames();
        }
    }
}
