using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public KeyCode inputBomb = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombsAmount = 1;

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

        Destroy(bomb);
        bombsRemaining++;
    }


    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            other.isTrigger = false;
    }
}
