using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI enemyText;

    void Start()
    {
        foreach (Transform child in transform)
        {
            // Verificar que el nombre coincida con el enum
            Debug.Log(HUDTextName.ScoreValue.ToString());
            if (child.name.Equals(HUDTextName.ScoreValue.ToString()))
            {
                scoreText = child.GetComponent<TextMeshProUGUI>();
                if (scoreText == null)
                {
                    Debug.LogError("⚠️ No se encontró TextMeshProUGUI en " + child.name);
                }
            }
            else if (child.name.Equals(HUDTextName.EnemyValue.ToString()))
            {
                enemyText = child.GetComponent<TextMeshProUGUI>();
                if (enemyText == null)
                {
                    Debug.LogError("⚠️ No se encontró TextMeshProUGUI en " + child.name);
                }
            }
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        
    }

    public void UpdateEnemies(int amount)
    {
        Debug.Log(amount);
        if (enemyText != null)
        {
            enemyText.text = amount.ToString();
        }
        
    }
}

public enum HUDTextName
{
    ScoreValue,
    EnemyValue
}
