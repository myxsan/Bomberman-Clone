using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static int[] scores = new int[2];
    static int round = 1;
    public static int winnerNum;
    public int startScore = 3;
    [SerializeField] PlayerScoreStats[] players;
    [SerializeField] GameObject gameOverMenu;

    private void Start() {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);
        for(int p = 0; p < players.Length; p++)
        {
            if(round == 1)
            {
                SetScore(players[p], startScore);
            }else
            {
                SetScore(players[p], scores[p]);
            }
        }
        round++;
    }
    public void GetScoresOnDie()
    {
        for(int p = 0; p < players.Length; p++)
        {
            ScoreManager.scores[p] = players[p].score;
        }
    }

    public void SetScore(PlayerScoreStats player, int score)
    {
        player.score = score;
        player.scoreText.text = player.score.ToString();
    }

    public void CheckDeadPlayer(GameObject player)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].player == player)
            {
                SetScore(players[i], players[i].score - 1);
                GetScoresOnDie();
                if(players[i].score == 0){
                    winnerNum = i == 0 ? 1 : 2;
                    Invoke(nameof(EndGame), 2f);
                }
            }
        }
    }

    private void EndGame()
    {
        gameOverMenu.SetActive(true);                   
        Time.timeScale = 0;

        ResetStats();
    }

    private void ResetStats()
    {
        round = 1;
        scores = new int[2];
    }
}

[System.Serializable]
public class PlayerScoreStats
{
    public GameObject player;
    public Text scoreText;
    [HideInInspector] public int score;
}