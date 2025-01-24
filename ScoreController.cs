using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TTConstants;

public class ScoreController : MonoBehaviour
{
    TMPro.TextMeshProUGUI scoreText_A;
    TMPro.TextMeshProUGUI scoreText_B;
    TMPro.TextMeshProUGUI gameTextA;
    TMPro.TextMeshProUGUI gameTextB;
    TMPro.TextMeshProUGUI gameCountText;

    TMPro.TextMeshProUGUI episodeText;
    TMPro.TextMeshProUGUI stepText;
    
    TTAgent agentA;
    TTAgent agentB;
    GameController gameController;

    int currGame = 1;
    int totalGames = 5;

    int gamesWon_A = 0;
    int gamesWon_B = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText_A = GameObject.FindGameObjectWithTag(TTConstants.tag_scoreA)
                        .GetComponent<TMPro.TextMeshProUGUI>();
        scoreText_B = GameObject.FindGameObjectWithTag(TTConstants.tag_scoreB)
                        .GetComponent<TMPro.TextMeshProUGUI>();
        gameTextA = GameObject.FindGameObjectWithTag(TTConstants.tag_game_textA)
                        .GetComponent<TMPro.TextMeshProUGUI>();
        gameTextB = GameObject.FindGameObjectWithTag(TTConstants.tag_game_textB)
                        .GetComponent<TMPro.TextMeshProUGUI>();

        gameCountText = GameObject.FindGameObjectWithTag(TTConstants.tag_game_count_text)
                        .GetComponent<TMPro.TextMeshProUGUI>();

        episodeText = GameObject.FindGameObjectWithTag(TTConstants.tag_episode)
                        .GetComponent<TMPro.TextMeshProUGUI>();
        stepText = GameObject.FindGameObjectWithTag(TTConstants.tag_step_count)
                        .GetComponent<TMPro.TextMeshProUGUI>();
       
        agentA = GameObject.FindGameObjectWithTag(TTConstants.tag_agentA)
                    .GetComponent<TTAgent>();
        agentB = GameObject.FindGameObjectWithTag(TTConstants.tag_agentB)
                    .GetComponent<TTAgent>();

        gameController = GameObject.FindGameObjectWithTag(TTConstants.tag_game_controller)
                           .GetComponent<GameController>();

        scoreText_A.text = "0";
        scoreText_B.text = "0";
        episodeText.text = "0";
        stepText.text = "0";
        gameCountText.text = "0";
        gameTextA.text = "(0)";
        gameTextB.text = "(0)";

        resetParameters();
    }

    void resetParameters() {
        currGame = 1;
        gamesWon_A = 0;
        gamesWon_B = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("Score Controller update");

        scoreText_A.text = agentA.getScore().ToString();
        scoreText_B.text = agentB.getScore().ToString();
        gameTextA.text = "(" + gamesWon_A.ToString() + ")";
        gameTextB.text = "(" + gamesWon_B.ToString() + ")";
        gameCountText.text = currGame.ToString();

        stepText.text = agentA.StepCount.ToString();
        episodeText.text = agentA.CompletedEpisodes.ToString();

        if (agentA.getScore() >= 11 || agentB.getScore() >= 11) {
            currGame++;

            if (agentA.getScore() == 11)
                gamesWon_A += 1;
            else
                gamesWon_B += 1;

            Debug.Log("Game ends: " + currGame);

            if (gamesWon_A + gamesWon_B == totalGames)
            {
                Debug.Log("match over");

                if (gamesWon_A > gamesWon_B)
                    Debug.Log("Agent A won: " + gamesWon_A);
                else
                    Debug.Log("Agent B won: " + gamesWon_B);

                resetParameters();
            }

            gameController.matchReset();
        }
    }
}
