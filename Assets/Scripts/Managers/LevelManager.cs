using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public int currentLevel = 0;
    public int enemiesKilled = 0;
    private int enemyAmount;
    public List<Texture2D> textures = new List<Texture2D>();
    
    public GameObject backgroundGame;
    public Material bgMaterial;
    private Vector3 backgroundGamePos;

    void Start(){

        backgroundGamePos = backgroundGame.transform.position;
    }


    void ChangeLevel(){
        GameManager.Instance.ChangeState(GameState.RestartSpawners);
        bgMaterial.mainTexture = textures[Random.Range(0,textures.Count)];
        backgroundGame.transform.DOMove(new Vector3(backgroundGamePos.x,backgroundGamePos.y + 22,backgroundGamePos.z), 0.3f).OnComplete(() => 
        backgroundGame.transform.DOMove(new Vector3(backgroundGamePos.x,backgroundGamePos.y - 22,backgroundGamePos.z), 0.3f));

    }




    //event shit
    protected void Awake()
    {
        Enemy.OnEnemyDead += HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart += HandleEnemyAmount;
    }
    private void OnDestroy()
    {
        Enemy.OnEnemyDead -= HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart -= HandleEnemyAmount;
    }
    private void HandleEnemyDying(int i){
        enemiesKilled = enemiesKilled + i;
        if(enemiesKilled >= enemyAmount){
            currentLevel++;
            ChangeLevel();
        }
    }
    private void HandleEnemyAmount(int i){
        enemyAmount = i;
    }
}
