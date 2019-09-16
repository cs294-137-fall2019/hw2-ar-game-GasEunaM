using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State {PAUSED,ONGOING,END};
public enum PlayerAndResult { NONE,P1, P2,AI,TIE}
public enum Mode { PVC,PVP}

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    private PlaceGameBoard placeGameBoard;
    public GameObject gameBoard;
    public ARGrid[,] Grids;
    private PlayerAndResult result;
    public static State gameState=State.PAUSED;
    public static PlayerAndResult player = PlayerAndResult.P1;
    public static Mode mode = Mode.PVC;
    public Text resultText;
    public Text TextPlayer;
    public Text TextMode;

    void Start()
    {
        placeGameBoard = GetComponent<PlaceGameBoard>();
        Grids = new ARGrid[3, 3]; 
        gameState = State.PAUSED;
        resultText.gameObject.SetActive(false);
        result = PlayerAndResult.NONE;
        mode = Mode.PVC;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                string name = "grid" + i.ToString() + j.ToString();
                Grids[i, j] = gameBoard.transform.Find(name).gameObject.GetComponent<ARGrid>();

            }
        }
    }
    bool Full()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {

                if (Grids[i, j].flag == PlayerAndResult.NONE)
                    return false;
            }
        }
        return true;
    }
    bool Checkresult()
    {
        for (int i = 0; i < 3; i++)
        {
            if(Grids[i,0].flag!= PlayerAndResult.NONE && Grids[i,1].flag==Grids[i,0].flag&& Grids[i, 2].flag == Grids[i, 0].flag)
            {
                result = Grids[i, 0].flag;
                return true;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (Grids[0, i].flag != PlayerAndResult.NONE && Grids[1, i].flag == Grids[0, i].flag && Grids[2, i].flag == Grids[0, i].flag)
            {
                result = Grids[0, i].flag;
                return true;
            }
        }
        if (Grids[0, 0].flag != PlayerAndResult.NONE && Grids[1, 1].flag == Grids[0, 0].flag && Grids[2, 2].flag == Grids[0, 0].flag)
        {
            result = Grids[0, 0].flag;
            return true;
        }
        if (Grids[0, 2].flag != PlayerAndResult.NONE && Grids[1, 1].flag == Grids[0, 2].flag && Grids[2, 0].flag == Grids[0, 2].flag)
        {
            result = Grids[0, 2].flag;
            return true;
        }
        if(Full())
        {
            result = PlayerAndResult.TIE;
            return true;
        }
        return false;
    }

    public void ChangeMode()
    {
        if (mode == Mode.PVC)
        {
            mode = Mode.PVP;
            TextPlayer.gameObject.SetActive(true);
            TextPlayer.text = "P1";
            TextPlayer.color = Color.red;
            TextMode.text = "PVP";
        }
        else
        {
            mode = Mode.PVC;
            TextPlayer.gameObject.SetActive(false);
            TextMode.text = "PVC";
        }
        Restart();
    }

    public void Restart()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {

                Grids[i, j].Clear();
                gameState = State.ONGOING;
                result = PlayerAndResult.NONE;
                player = PlayerAndResult.P1;
                TextPlayer.text = "P1";
                TextPlayer.color = Color.red;
            }
        }
    }




    void AIDecision()
    {
        int numAI = 0;
        int numP1 = 0;
        int emptyj = -1;
        int emptyi = -1;
        for (int i=0;i<3;i++)
        {
            numAI = 0;
            emptyj = -1;
            for(int j=0;j<3;j++)
            {
                if (Grids[i, j].flag == PlayerAndResult.AI)
                    numAI++;
                else if (Grids[i, j].flag == PlayerAndResult.NONE)
                    emptyj = j;
            }
            if(numAI==2 && emptyj!= -1)
            {
                Grids[i, emptyj].AI();
                return;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            numAI = 0;
            emptyi = -1;
            for (int i = 0; i < 3; i++)
            {
                if (Grids[i, j].flag == PlayerAndResult.AI)
                    numAI++;
                else if (Grids[i, j].flag == PlayerAndResult.NONE)
                    emptyi = i;
            }
            if (numAI == 2 && emptyi != -1)
            {
                Grids[emptyi, j].AI();
                return;
            }
        }

        numAI = 0;
        int empty = -1;
        for (int i = 0; i < 3; i++)
        {
            if (Grids[i, i].flag == PlayerAndResult.AI)
                numAI++;
            else if (Grids[i, i].flag == PlayerAndResult.NONE)
                empty = i;
        }
        if (numAI == 2 && empty != -1)
        {
            Grids[empty, empty].AI();
            return;
        }

        numAI = 0;
        empty = -1;
        for (int i = 0; i < 3; i++)
        {
            if (Grids[i, 2-i].flag == PlayerAndResult.AI)
                numAI++;
            else if (Grids[i, 2-i].flag == PlayerAndResult.NONE)
                empty = i;
        }
        if (numAI == 2 && empty != -1)
        {
            Grids[empty, 2-empty].AI();
            return;
        }



        for (int i = 0; i < 3; i++)
        {
            numP1 = 0;
            emptyj = -1;
            for (int j = 0; j < 3; j++)
            {
                if (Grids[i, j].flag == PlayerAndResult.P1)
                    numP1++;
                else if (Grids[i, j].flag == PlayerAndResult.NONE)
                    emptyj = j;
            }
            if (numP1 == 2 && emptyj != -1)
            {
                Grids[i, emptyj].AI();
                return;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            numP1 = 0;
            emptyi = -1;
            for (int i = 0; i < 3; i++)
            {
                if (Grids[i, j].flag == PlayerAndResult.P1)
                    numP1++;
                else if (Grids[i, j].flag == PlayerAndResult.NONE)
                    emptyi = i;
            }
            if (numP1 == 2 && emptyi != -1)
            {
                Grids[emptyi, j].AI();
                return;
            }
        }

        numP1 = 0;
        empty = -1;
        for (int i = 0; i < 3; i++)
        {
            if (Grids[i, i].flag == PlayerAndResult.P1)
                numP1++;
            else if (Grids[i, i].flag == PlayerAndResult.NONE)
                empty = i;
        }
        if (numP1 == 2 && empty != -1)
        {
            Grids[empty, empty].AI();
            return;
        }

        numP1 = 0;
        empty = -1;
        for (int i = 0; i < 3; i++)
        {
            if (Grids[i, 2 - i].flag == PlayerAndResult.P1)
                numP1++;
            else if (Grids[i, 2 - i].flag == PlayerAndResult.NONE)
                empty = i;
        }
        if (numP1 == 2 && empty != -1)
        {
            Grids[empty, 2 - empty].AI();
            return;
        }


        int x = Random.Range(0, 2);
        int y = Random.Range(0, 2);
        for(int i=0;i<5;i++)
        {
            if (Grids[x, y].flag == PlayerAndResult.NONE)
            {
                Grids[x, y].AI();
                return;
            }
            x = Random.Range(0, 2);
            y = Random.Range(0, 2);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Grids[i, j].flag == PlayerAndResult.NONE)
                {
                    Grids[i, j].AI();
                    return;
                }

            }
        }
       
    }

    void Update()
    {
        if (!placeGameBoard.Placed())
        {
            gameState = State.PAUSED;
        }
        else
        {
            gameState = State.ONGOING;
            if (Checkresult())
            {
                gameState = State.END;
            }
            if (gameState == State.END)
            {
                resultText.gameObject.SetActive(true);
                if (result == PlayerAndResult.P1 && mode == Mode.PVC)
                {
                    resultText.text = "Win!";
                    resultText.color = Color.red;
                }
                else if (result == PlayerAndResult.P1 && mode == Mode.PVP)
                {
                    resultText.text = "P1 Win!";
                    resultText.color = Color.red;
                }
                if (result == PlayerAndResult.P2)
                {
                    resultText.text = "P2 Win!";
                    resultText.color = Color.blue;
                }
                else if (result == PlayerAndResult.AI)
                {
                    resultText.text = "Lose!";
                    resultText.color = Color.blue;
                }
                else if (result == PlayerAndResult.TIE)
                {
                    resultText.text = "Tie!";
                    resultText.color = Color.yellow;
                }
            }
            if (gameState == State.ONGOING)
            {
                resultText.gameObject.SetActive(false);
                if (player == PlayerAndResult.AI)
                {
                    AIDecision();
                }
            }
        }

    }

}
