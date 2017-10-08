using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITree : MonoBehaviour
{
    private TicTacToeController ticTacToeController;
    
    public void Start()
    {
        //Get Controller
        ticTacToeController = GetComponentInParent<TicTacToeController>();
    }

    public void yourTurn()
    {
        List<int> winLocations = calculateLocations(slotWin);
        List<int> loseLocations = calculateLocations(slotRisk);

        List<int> locations = new List<int>();

        //Check to see if there are any possible instant win locations with high risk
        foreach (int location in winLocations)
        {
            if (loseLocations.Contains(location))
            {
                locations.Add(location);
            }
        }

        //Executes position if there is a high risk instant location available
        if (locations.Count > 0)
        {
            ticTacToeController.passClick(locations[0]);
        }
        else
        {
            //Executes position if instant win location available
            if (winLocations.Count > 0)
            {
                ticTacToeController.passClick(winLocations[0]);
            }
            else
            {
                //Executes position if high risk location is available
                if (loseLocations.Count > 0)
                {
                    ticTacToeController.passClick(loseLocations[0]);
                }
                else
                {
                    //Calctues seconadry setup win moves
                    winLocations = calculateLocations(slotWinSecondary);

                    //Executes position if secondary location is available
                    if (winLocations.Count > 0)
                    {
                        ticTacToeController.passClick(winLocations[0]);
                    }
                    else
                    {
                        //Chooses a random square on the board
                        ButtonManager[] buttons = ticTacToeController.buttons;
                        int count = 0;
                        List<int> possible = new List<int>();
                        foreach (ButtonManager button in buttons)
                        {
                            if (button.ButtonState == ButtonManager.State.NOTHING)
                            {
                                possible.Add(count);
                            }
                            count++;
                        }

                        ticTacToeController.passClick(possible[new System.Random().Next(possible.Count)]);

                    }
                }
            }
        }
    }

    //Runs through every possible tile setup in grid. Takes a delegate for what filter to use
    private List<int> calculateLocations(Func<int, int, int, int> slotter)
    {
        List<int> locations = new List<int>();

        int result;
        for (int row = 0; row < 3; ++row)
        {
            result = -1;
            if ((result = slotter(3 * row, 3 * row + 1, 3 * row + 2)) != -1)
                locations.Add(result);
        }

        for (int column = 0; column < 3; ++column)
        {
            result = -1;
            if ((result = slotter(column, column + 3, column + 6)) != -1)
                locations.Add(result);
        }

        result = -1;
        if ((result = slotter(0, 4, 8)) != -1)
            locations.Add(result);

        result = -1;
        if ((result = slotter(2, 4, 6)) != -1)
            locations.Add(result);

        return locations;
    }

    //Calcutes if the 3 grid locations can be a instant win
    private int slotWin(int first, int second, int third)
    {
        ButtonManager[] buttons = ticTacToeController.buttons;

        int me = 0;
        int empty = 0;
        int emptyLocation = -1;
        if (buttons[first].ButtonState == ButtonManager.State.PLAYER_2)
            me++;
        if (buttons[second].ButtonState == ButtonManager.State.PLAYER_2)
            me++;
        if (buttons[third].ButtonState == ButtonManager.State.PLAYER_2)
            me++;

        if (buttons[first].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = first;
        }
        if (buttons[second].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = second;
        }
        if (buttons[third].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = third;
        }

        if (me >= 2 && empty == 1)
        {
            return emptyLocation;
        }

        return -1;
    }

    //Calcutes to see if there is a move that can be setup for the 3 grid locations
    private int slotWinSecondary(int first, int second, int third)
    {
        ButtonManager[] buttons = ticTacToeController.buttons;

        int me = 0;
        int empty = 0;

        if (buttons[first].ButtonState == ButtonManager.State.PLAYER_1)
            return -1;
        if (buttons[second].ButtonState == ButtonManager.State.PLAYER_1)
            return -1;
        if (buttons[third].ButtonState == ButtonManager.State.PLAYER_1)
            return -1;

        if (buttons[first].ButtonState == ButtonManager.State.PLAYER_2)
            me++;
        if (buttons[second].ButtonState == ButtonManager.State.PLAYER_2)
            me++;
        if (buttons[third].ButtonState == ButtonManager.State.PLAYER_2)
            me++;

        if (me > 0)
        {
            if (buttons[first].ButtonState == ButtonManager.State.NOTHING)
                return first;
            if (buttons[second].ButtonState == ButtonManager.State.NOTHING)
                return second;
            if (buttons[third].ButtonState == ButtonManager.State.NOTHING)
                return third;
        }

        return -1;
    }

    //Calcutes if there is instant lose going to occur.
    private int slotRisk(int first, int second, int third)
    {
        ButtonManager[] buttons = ticTacToeController.buttons;

        int oppenent = 0;
        int empty = 0;
        int emptyLocation = -1;
        if (buttons[first].ButtonState == ButtonManager.State.PLAYER_1)
            oppenent++;
        if (buttons[second].ButtonState == ButtonManager.State.PLAYER_1)
            oppenent++;
        if (buttons[third].ButtonState == ButtonManager.State.PLAYER_1)
            oppenent++;

        if (buttons[first].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = first;
        }
        if (buttons[second].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = second;
        }
        if (buttons[third].ButtonState == ButtonManager.State.NOTHING)
        {
            empty++;
            emptyLocation = third;
        }

        if (oppenent >= 2 && empty == 1)
        {
            return emptyLocation;
        }

        return -1;
    }
}
