using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int id;
    private GameManager game;
    private bool hasCollided = false;
    public Dropper dropper;
    public bool isTouchingTopBar = false;

    // gets game manager
    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        dropper = GameObject.Find("Dropper").GetComponent<Dropper>();
    }

    // adds a delay before destroying the fruits
    IEnumerator DestroyFruits(GameObject fruit1, GameObject fruit2)
    {
        yield return new WaitForSeconds(0.01f); // wait for 0.1 seconds
        Destroy(fruit1);
        Destroy(fruit2);
    }

    IEnumerator WaitForEndGame() {
        yield return new WaitForSeconds(5f);
        if (isTouchingTopBar == true) {
            game.endGame();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {

        dropper.currentFruit = null;
        dropper.SpawnNewFruit();
    }

    void OnTriggerStay2D(Collider2D collision) {
        isTouchingTopBar = true;
        StartCoroutine(WaitForEndGame());
    }

    void OnTriggerExit2D(Collider2D collision) {
        isTouchingTopBar = false;
    }

    // checks if fruit is touching another fruit
    void OnCollisionEnter2D(Collision2D collision)
    {
        // gets fruit component on the other fruit
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        // checks if the other fruit exists and if the id is the same, and checks if the instance id is different (to prevent double combining)
        if (otherFruit != null && otherFruit.id == this.id && this.gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            game.InstantiateFruit(this.transform.position, this.id + 1);
            game.addScore(this.id);
            StartCoroutine(DestroyFruits(this.gameObject, collision.gameObject));
        } 
    }
}
