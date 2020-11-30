using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    GameDirector gameDirector;
    public GameObject enemy;
    public GameObject heavyEnemy;
    //public GameObject enemyGuns;
    public GameObject soilTilePrefab;
    public GameObject soilTileSurfacePrefab;
    public GameObject holeColliderPrefab;
    public GameObject explosiveCan;
    public GameObject woodStand;
    public GameObject woodBox;
    public GameObject helicopter;
    GameObject soilTile;
    GameObject player;
    float playerPosX;
    int tilePosY;
    float playerPosDistance;
    float playerPastPos;
    int tileDistance;
    int tilePositionX;
    float holePosDistance;
    int holeInterval;
    int i=0;
    float holeDistance;
    float enemySpawnInterval;
    float enemySpawnDistance;
    float enemySpawnPastPos;
    float obstacleSpawnInterval;
    float obstacleSpawnDistance;
    float obstacleSpawnPastPos;
    float woodStandInterval;
    float woodStandDistance;
    float woodStandPastPos;
    float helicopterInterval;
    float helicopterDistance;
    float helicopterPastPos;
    public float enemyIncreaseRate;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
        playerPosX = player.transform.position.x;
        tilePosY = (int)player.transform.position.y;
        playerPastPos = player.transform.position.x;
        enemySpawnInterval = 0;
        obstacleSpawnInterval = 0;
        woodStandInterval = 0;
        helicopterInterval = 0;
        holeDistance = Random.Range(20, 30);
        tileDistance = Random.Range(4, 7);
        tilePositionX = (int)player.transform.position.x;
        holeInterval = 1;
        enemySpawnDistance = Random.Range(3, 25);
        obstacleSpawnDistance = Random.Range(10, 20);
        woodStandDistance = Random.Range(10, 30);
        helicopterDistance = helicopterDistance = Random.Range(100, 200);
        enemySpawnPastPos = playerPosX;
        obstacleSpawnPastPos = playerPosX;
        woodStandPastPos = playerPosX;
        helicopterPastPos = playerPosX;
        enemyIncreaseRate = 1f;

        int x;
        int y;
        for (x = -20; x < 28; x++)
        {
            for (y = -20; y < -2; y++)
            {
                soilTile = Instantiate(soilTilePrefab) as GameObject;
                soilTile.transform.position = new Vector3(
                    (int)playerPosX + x,
                    tilePosY + y,
                    0);
            }
            soilTile = Instantiate(soilTileSurfacePrefab) as GameObject;
            soilTile.transform.position = new Vector3(
                    (int)playerPosX + x,
                    tilePosY + y,
                    0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<StageGenerator>().enabled = false;
        }

        playerPosX = player.transform.position.x;
        playerPosDistance = playerPosX - playerPastPos;
        enemySpawnInterval = playerPosX - enemySpawnPastPos;
        obstacleSpawnInterval = playerPosX - obstacleSpawnPastPos;
        woodStandInterval = playerPosX - woodStandPastPos;
        helicopterInterval = playerPosX - helicopterPastPos;

        if (playerPosDistance > tileDistance / 1.3f)
        {
            if (holePosDistance > holeDistance)
            {
                if(holeInterval <= i)
                {
                    CreateHole();
                    tileDistance = Random.Range(4, 7);
                    holeDistance = Random.Range(20, 30);
                    holeInterval = 1;
                    holePosDistance = 0;
                    i = 0;
                }
                i++;
            }
            else
            { 
                CreateTiles();
                tileDistance = Random.Range(4, 7);
            }
            playerPastPos = playerPosX;
            holePosDistance += playerPosDistance;
            playerPosDistance = 0;
        }

        if (enemySpawnInterval > enemySpawnDistance)
        {
            EnemySpawn();
            if (enemyIncreaseRate < 0.11)
            {
                enemyIncreaseRate = 0.11f;
            }
            enemySpawnDistance = Random.Range(2, 25 * enemyIncreaseRate);
            enemySpawnPastPos = playerPosX;
            enemySpawnInterval = 0;
        }

        if (obstacleSpawnInterval > obstacleSpawnDistance)
        {
            CreateObstacles();
            obstacleSpawnDistance = Random.Range(10, 20);
            obstacleSpawnPastPos = playerPosX;
            obstacleSpawnInterval = 0;
        }

        if (woodStandInterval > woodStandDistance)
        {
            CreateWoodStand();
            woodStandDistance = Random.Range(10, 30);
            woodStandPastPos = playerPosX;
            woodStandInterval = 0;
        }

        if (helicopterInterval > helicopterDistance)
        {
            HelicopterSpawn();
            if (enemyIncreaseRate < 0.11)
            {
                enemyIncreaseRate = 0.11f;
            }
            helicopterDistance = Random.Range(100, 200 * enemyIncreaseRate);
            helicopterPastPos = playerPosX;
            helicopterInterval = 0;
        }
    }

    private void CreateTiles()
    {
        int x;
        int y;

        tilePosY += Random.Range(-1, 2);

        for (x = 0; x < tileDistance; x++)
        {
            for (y = -20; y < -2; y++)
            {
                soilTile = Instantiate(soilTilePrefab) as GameObject;
                soilTile.transform.position = new Vector3(
                    tilePositionX + x + 28,
                    tilePosY + y,
                    0);
            }
            soilTile = Instantiate(soilTileSurfacePrefab) as GameObject;
            soilTile.transform.position = new Vector3(
                    (int)tilePositionX + x + 28,
                    tilePosY + y,
                    0);
        }
        tilePositionX += tileDistance;
    }

    private void CreateHole()
    {
        int x;
        int y;

        GameObject holeCollider;

        for (x = 0; x < tileDistance; x++)
        {
            for (y = -10; y < -1; y++)
            {
                holeCollider = Instantiate(holeColliderPrefab) as GameObject;
                holeCollider.transform.position = new Vector3(
                    tilePositionX + x + 28,
                    tilePosY + y - 3,
                    0);
            }
        }
        tilePositionX += tileDistance;
    }

    private void EnemySpawn()
    {
        int dice = Random.Range(0, 11);
        if(0 <= dice && dice < 9)
        {
            GameObject enemySpawn = Instantiate(enemy) as GameObject;
            enemySpawn.transform.position = new Vector3(playerPosX + 28,
                tilePosY + 4,
                0);
        }
        else
        {
            GameObject enemySpawn = Instantiate(heavyEnemy) as GameObject;
            enemySpawn.transform.position = new Vector3(playerPosX + 28,
                tilePosY + 4,
                0);
        }
        
    }

    private void CreateObstacles()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
            case 2:
                GameObject obstacles = Instantiate(woodBox) as GameObject;
                obstacles.transform.position = new Vector3(playerPosX + 28,
                    tilePosY + 2,
                    0);
                break;
            case 3:
                obstacles = Instantiate(explosiveCan) as GameObject;
                obstacles.transform.position = new Vector3(playerPosX + 28,
                    tilePosY + 2,
                    0);
                break;

        }
    }

    private void CreateWoodStand()
    {
        int random = Random.Range(-3, -2);
        GameObject stand = Instantiate(woodStand) as GameObject;
        stand.transform.position = new Vector3(playerPosX + 28,
            tilePosY + random,
            0);
        GameObject enemySpawn = Instantiate(enemy) as GameObject;
        enemySpawn.transform.position = new Vector3(playerPosX + 28,
            tilePosY + random + 10,
            0);

    }

    private void HelicopterSpawn()
    {
        GameObject helicopterSpawn = Instantiate(helicopter) as GameObject;
        helicopterSpawn.transform.position = new Vector3(playerPosX + 5,
            tilePosY + 15,
            0);
    }

}
