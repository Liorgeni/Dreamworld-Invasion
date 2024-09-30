using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;  // Name of the enemy
    public int health;        // Enemy's health
    public int damage;        // Damage dealt by the enemy
    public GameObject enemyPrefab; // Reference to the enemy prefab (box, sphere)
}