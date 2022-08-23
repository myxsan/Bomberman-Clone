using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb values")]
    public GameObject bombPrefab;
    public KeyCode inputBomb = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombsAmount = 1;
    
    [Header("Explosion Values")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible Values")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;


    private int bombsRemaining;

    private void OnEnable() {
        bombsRemaining = bombsAmount;
    }

    private void Update() {
        if(bombsRemaining > 0 && Input.GetKeyDown(inputBomb)){
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 pos, Vector2 dir, int length)
    {
        if(length <= 0) return;

        pos += dir;

        if(Physics2D.OverlapBox(pos, Vector2.one * 0.5f, 0f, explosionLayerMask))
        {
            ClearDestructible(pos);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(dir);
        explosion.DestroyAfter(explosionDuration);

        Explode(pos, dir, length -1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            other.isTrigger = false;
    }
}
