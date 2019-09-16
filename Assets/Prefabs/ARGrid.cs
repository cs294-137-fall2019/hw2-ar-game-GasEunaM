using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARGrid : MonoBehaviour,OnTouch3D
{
    public GameObject sphere;
    public GameObject cube;
    public float debounceTime = 0.3f;
    private float remainingDebounceTime;
    // Update is called once per frame
    public PlayerAndResult flag= PlayerAndResult.NONE;
    public Text TextPlayer;
    void Start()
    {
        remainingDebounceTime = 0.3f;
        flag = PlayerAndResult.NONE;
    }
    void Update()
    {
        if(Game.gameState==State.PAUSED)
            remainingDebounceTime = 0.3f;
        if (remainingDebounceTime > 0)
            remainingDebounceTime -= Time.deltaTime;
    }
    public void OnTouch()
    {
        if (flag == PlayerAndResult.NONE && remainingDebounceTime <= 0 && (Game.player == PlayerAndResult.P1 || Game.player == PlayerAndResult.P2))
        {
            if (Game.player == PlayerAndResult.P1)
                sphere.SetActive(true);
            if (Game.player == PlayerAndResult.P2)
                cube.SetActive(true);
            flag = Game.player;
            remainingDebounceTime = debounceTime;
            if (Game.mode == Mode.PVC)
                Game.player = PlayerAndResult.AI;
            else if (Game.player == PlayerAndResult.P1)
            {
                Game.player = PlayerAndResult.P2;
                TextPlayer.text = "P2";
                TextPlayer.color = Color.blue;
            }
            else
            {
                Game.player = PlayerAndResult.P1;
                TextPlayer.text = "P1";
                TextPlayer.color = Color.red;
            }
        }


    }
    public void AI()
    {
        cube.SetActive(true);
        flag = Game.player;
        Game.player = PlayerAndResult.P1;
    }
    public void Clear()
    {
        sphere.SetActive(false);
        cube.SetActive(false);
        flag = PlayerAndResult.NONE;
        remainingDebounceTime = 0.3f;
    }
}
