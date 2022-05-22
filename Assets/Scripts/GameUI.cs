using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private TMP_Text bonusText;
    private Game game;

    private void Start()
    {
        Time.timeScale = 1;
        game = Game.instance;
        game.onBonusTake += AddBonus;
        game.onLoseGame += LoseGame;
        game.onWinGame += WinGame;
        game.onStartGame += StartGame;
        gameScreen.SetActive(false);
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void OnDisable()
    {
        game.onBonusTake -= AddBonus;
        game.onLoseGame -= LoseGame;
        game.onWinGame -= WinGame;
        game.onStartGame -= StartGame;
    }

    private void WinGame()
    {
        startScreen.SetActive(false);
        winScreen.SetActive(true);
        loseScreen.SetActive(false);
    }

    private void StartGame()
    {
        gameScreen.SetActive(true);
        startScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void LoseGame()
    {
        Time.timeScale = 0;
        startScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(true);
    }

    private void AddBonus()
    {
        bonusText.text = game.BonusCount.ToString();
    }
}
