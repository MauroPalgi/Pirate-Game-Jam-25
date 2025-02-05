using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameManager gameManager;
    public MenuManager menuManager;

    public EnemySpawner enemySpawner;
    public ObstacleSpawner obstacleSpawner;
    private HUDController hudController;
    public int currentLevel = 0;
    public int enemiesKilled = 0;
    private int enemyAmount;
    public List<Texture2D> textures = new List<Texture2D>();

    public GameObject backgroundGame;
    public Material bgMaterial;
    private Vector3 backgroundGamePos;

    void Start()
    {
        backgroundGamePos = backgroundGame.transform.position;
    }

    void ChangeLevel()
    {
        bgMaterial.mainTexture = textures[Random.Range(0, textures.Count)];
        backgroundGame
            .transform.DOMove(
                new Vector3(backgroundGamePos.x, backgroundGamePos.y + 22, backgroundGamePos.z),
                0.3f
            )
            .OnComplete(
                () =>
                    backgroundGame.transform.DOMove(
                        new Vector3(
                            backgroundGamePos.x,
                            backgroundGamePos.y - 22,
                            backgroundGamePos.z
                        ),
                        0.3f
                    )
            );
    }

    //event shit
    protected void Awake()
    {
        Enemy.OnEnemyDead += HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart += HandleEnemyAmount;
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDead -= HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart -= HandleEnemyAmount;
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleEnemyDying(int i)
    {
        menuManager.UpdateScore(99);
    }

    private void HandleEnemyAmount(int i) { }

    private void HandleOnGameStateChanged(GameState state)
    {
        if (state == GameState.SpawningLevel)
        {
            menuManager.UpdateScore(99);
            Debug.Log("enemies " + enemySpawner.GetEnemyAmount());
            menuManager.UpdateEnemies(enemySpawner.GetEnemyAmount());
        }
    }
}
