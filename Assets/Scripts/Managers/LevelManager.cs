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
    private Material bgMaterial;
    private Renderer objRenderer;
    private Vector3 backgroundGamePos;

    void Start(){
        objRenderer = backgroundGame.GetComponent<Renderer>();
        bgMaterial = objRenderer.GetComponent<Material>();
        backgroundGamePos = backgroundGame.transform.position;
    }


    void ChangeLevel(){
        GameManager.Instance.ChangeState(GameState.RestartSpawners);
        bgMaterial.mainTexture = textures[Random.Range(0,textures.Count)];
        backgroundGame.transform.DOMove(new Vector3(backgroundGamePos.x,112,backgroundGamePos.z), 0.1f).OnComplete(() => 
        backgroundGame.transform.DOMove(new Vector3(backgroundGamePos.x,90,backgroundGamePos.z), 0.1f));

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
