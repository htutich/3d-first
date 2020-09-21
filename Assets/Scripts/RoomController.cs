using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public bool doorTop = true, doorBot = true, doorLeft = true, doorRight = true, PlayerSpawn = false;
    public GameObject dTop, dBot, dLeft, dRight, Player;
    public Transform PlayerSpawnPosition;

    void Start()
    {
        if (!doorTop) Destroy(dTop);
        if (!doorBot) Destroy(dBot);
        if (!doorLeft) Destroy(dLeft);
        if (!doorRight) Destroy(dRight);

        //if(PlayerSpawn) Instantiate(Player, PlayerSpawnPosition, false);
    }

}
