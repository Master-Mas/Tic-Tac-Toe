using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class ButtonManager : MonoBehaviour {

    public enum State
    {
        PLAYER_1,
        PLAYER_2,
        NOTHING
    }

    private State buttonState = State.NOTHING;
    private State state = State.NOTHING;

    [HideInInspector]
    public Image image;
    [HideInInspector]
    public Button button;

	void Start () {
        image = GetComponent<Image>();
	    image.sprite = GetComponentInParent<TicTacToeController>().nothing;

	    button = GetComponent<Button>();
	}

    public void Update()
    {
        state = buttonState;
    }

    public State ButtonState
    {
        get { return buttonState; }
        set
        {
            buttonState = value;
            if (buttonState == State.PLAYER_1)
            {
                image.sprite = GetComponentInParent<TicTacToeController>().player1Icon;
            }
            else if (buttonState == State.PLAYER_2)
            {
                image.sprite = GetComponentInParent<TicTacToeController>().player2Icon;
            }
            else
            {
                image.sprite = GetComponentInParent<TicTacToeController>().nothing;
            }
        }
    }
}
