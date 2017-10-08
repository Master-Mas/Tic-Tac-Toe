using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeController : MonoBehaviour
{
    private enum WinCondition
    {
        PLAYER_1,
        PLAYER_2,
        NO_ONE
    }

    public Sprite player1Icon;
    public Sprite player2Icon;
    public Sprite nothing;

    public Text displayText;
    public Text score1;
    public Text score2;

    private int[] score = {0,0};

    public bool player1Turn = true;
    private int totalTurns;

    public ButtonManager[] buttons = new ButtonManager[9];

    public bool win = false;

    public void Start()
    {
        reset();
    }

    public void passClick(int index)
    {
        if (!win && buttons[index].ButtonState == ButtonManager.State.NOTHING)
        {
            buttons[index].ButtonState = player1Turn ? ButtonManager.State.PLAYER_1 : ButtonManager.State.PLAYER_2;
            //        buttons[index].button.interactable = false;
            buttons[index].enabled = false;
            player1Turn = !player1Turn;
            totalTurns++;

            WinCondition result;
            if ((result = winCondition()) != WinCondition.NO_ONE)
            {
                win = true;
                if (result == WinCondition.PLAYER_1)
                {
                    displayText.text = "Player 1 Wins";
                    score[0]++;
                }
                else
                {
                    displayText.text = "Player 2 Wins";
                    score[1]++;
                }

                updateScore();
                return;              
            }

            if (totalTurns > 8)
            {
                gameOver();
                return;
            }

            if (player1Turn)
            {
                foreach (ButtonManager button in buttons)
                {
                    button.button.enabled = true;
                }
                displayText.text = "Player 1's Turn";
            }
            else
            {
                foreach (ButtonManager button in buttons)
                {
                    button.button.enabled = false;
                }
                displayText.text = "Player 2's Turn";
                playAI();
            }
        }
    }

    private void updateScore()
    {
        score1.text = ""+score[0];
        score2.text = "" + score[1];
    }

    private IEnumerator waitForComputer()
    {
        yield return new WaitForSeconds(1);
        GetComponent<AITree>().yourTurn();
    }

    private void playAI()
    {
        StartCoroutine(waitForComputer());
    }

    private WinCondition winCondition()
    {
        for (int row = 0; row < 3; ++row)
        {
            int first = 3 * row;
            int second = 3 * row + 1;
            int third = 3 * row + 2;

            if (buttons[first].ButtonState == ButtonManager.State.PLAYER_1 &&
                buttons[second].ButtonState == ButtonManager.State.PLAYER_1 &&
                buttons[third].ButtonState == ButtonManager.State.PLAYER_1)
            {
                return WinCondition.PLAYER_1;
            }

            if (buttons[first].ButtonState == ButtonManager.State.PLAYER_2 &&
                buttons[second].ButtonState == ButtonManager.State.PLAYER_2 &&
                buttons[third].ButtonState == ButtonManager.State.PLAYER_2)
            {
                return WinCondition.PLAYER_2;
            }
        }

        for (int column = 0; column < 3; ++column)
        {
            int first = column;
            int second = column + 3;
            int third = column + 6;

            if (buttons[first].ButtonState == ButtonManager.State.PLAYER_1 &&
                buttons[second].ButtonState == ButtonManager.State.PLAYER_1 &&
                buttons[third].ButtonState == ButtonManager.State.PLAYER_1)
            {
                return WinCondition.PLAYER_1;
            }

            if (buttons[first].ButtonState == ButtonManager.State.PLAYER_2 &&
                buttons[second].ButtonState == ButtonManager.State.PLAYER_2 &&
                buttons[third].ButtonState == ButtonManager.State.PLAYER_2)
            {
                return WinCondition.PLAYER_2;
            }
        }

        if (buttons[0].ButtonState == ButtonManager.State.PLAYER_1 &&
            buttons[4].ButtonState == ButtonManager.State.PLAYER_1 &&
            buttons[8].ButtonState == ButtonManager.State.PLAYER_1)
        {
            return WinCondition.PLAYER_1;
        }

        if (buttons[0].ButtonState == ButtonManager.State.PLAYER_2 &&
            buttons[4].ButtonState == ButtonManager.State.PLAYER_2 &&
            buttons[8].ButtonState == ButtonManager.State.PLAYER_2)
        {
            return WinCondition.PLAYER_2;
        }

        if (buttons[2].ButtonState == ButtonManager.State.PLAYER_1 &&
            buttons[4].ButtonState == ButtonManager.State.PLAYER_1 &&
            buttons[6].ButtonState == ButtonManager.State.PLAYER_1)
        {
            return WinCondition.PLAYER_1;
        }

        if (buttons[2].ButtonState == ButtonManager.State.PLAYER_2 &&
            buttons[4].ButtonState == ButtonManager.State.PLAYER_2 &&
            buttons[6].ButtonState == ButtonManager.State.PLAYER_2)
        {
            return WinCondition.PLAYER_2;
        }

        return WinCondition.NO_ONE;
    }

    private void gameOver()
    {
        win = true;
        displayText.text = "Draw!";
    }

    public void reset()
    {
        displayText.text = "Player 1's Turn";
        totalTurns = 0;
        player1Turn = true;
        win = false;
        foreach (ButtonManager buttonManager in buttons)
        {
            buttonManager.ButtonState = ButtonManager.State.NOTHING;
        }

        foreach (ButtonManager button in buttons)
        {
            button.button.enabled = true;
        }

        updateScore();
    }
}
