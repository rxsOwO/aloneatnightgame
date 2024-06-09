using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEnemy : MonoBehaviour
{
    public float health = 100f;
    public Slider slider;
    public void Update(){
        slider.value = health;
    }

    public void TakeDamage(float amount){
        health -= amount;

        if(health <= 0f) {
            Die();
        }

    }
    public void Die(){
        SceneManager.LoadScene("Xavier's Level!");
    }
}
