using Controllers;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        public static UIManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        #region Screens

        [Header("Screens")] 
        public GameObject entryScreen;
        public GameObject inGameScreen;
        public GameObject pauseScreen;
        public GameObject failScreen;
        public GameObject winScreen;
    
        #endregion

        #region UIElements

        [Header("UI Elements")]
        public GameObject speedPowerUpButton;

        public GameObject nextLevelButton;

        public TextMeshProUGUI[] levelNameTexts;
        public TextMeshProUGUI[] totalCoinTexts;

        public TextMeshProUGUI toEarnCoinText;

        #endregion
    
        void Start()
        {
            UpdateLevelNames();
            UpdateTotalCoinTexts();
            
            entryScreen.SetActive(true);
        }

        #region UIElementsFunctions

        public void StartLevel()
        {
            LevelManager.SetStateAction?.Invoke(LevelManager.LevelStates.Started);
        }

        public void RestartLevel()
        {
            LevelManager.instance.RestartLevel();
        }

        public void NextLevel()
        {
            LevelManager.instance.NextLevel();
        }

        public void SpeedPowerUp()
        {
            StartCoroutine(PlayerBoxController.instance.SpeedPowerUp());
        }

        #endregion

        #region ScreenFunctions

        public void LevelStarted()
        {
            entryScreen.SetActive(false);

            inGameScreen.SetActive(true);
        }

        public void LevelFailed()
        {
            inGameScreen.SetActive(false);

            failScreen.SetActive(true);
        }

        public void LevelWin()
        {
            inGameScreen.SetActive(false);

            winScreen.SetActive(true);
        }
        
        #endregion

        #region UpdateFunctions

        public void UpdateLevelNames()
        {
            for (int i = 0; i < levelNameTexts.Length; i++)
            {
                levelNameTexts[i].text = "Level " + (LevelManager.instance.lastPlayedLevel + 1);
            }
        }

        void UpdateTotalCoinTexts()
        {
            for (int i = 0; i < totalCoinTexts.Length; i++)
            {
                totalCoinTexts[i].text = LevelManager.instance.totalCoin.ToString();
            }
        }
        public void UpdateTotalCoins()
        {
            for (int i = 0; i < totalCoinTexts.Length; i++)
            {
                totalCoinTexts[i].text = LevelManager.instance.totalCoin.ToString();
            }
        }

        #endregion

        #region Animations
        

        public void CoinIncreaseAnimation()
        {
            toEarnCoinText.text =
                Mathf.RoundToInt(LevelManager.instance.toEarnCoin -
                                 PlayerBoxController.instance.transform.localScale.y *
                                 LevelManager.instance.heightCoinMultiplier).ToString();
        }

        public void CoinAddAnimation()
        {
            UpdateTotalCoinTexts();

            nextLevelButton.SetActive(true);
        }

        #endregion
    
    }
}
