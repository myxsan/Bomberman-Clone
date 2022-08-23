using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Destruction")]
    public float destructionTime = 1f;
    [Header("Item Spawn")]
    public GameObject[] spawnableItems;
    [Range(0f, 1f)] public  float itemSpawnChance = 0.35f;

    private void Start() {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy() {
        if(spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
