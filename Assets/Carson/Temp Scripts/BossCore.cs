using UnityEngine;

public class BossCore : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject lineSpawner;
    [SerializeField] GameObject coneSpawner;
    [SerializeField] GameObject cardinalSpawner;

    bool lineActive = false;
    bool coneActive = false;
    bool cardinalActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            onDeath();
        }
    }
    void onDeath()
    {
        boss.SetActive(false);
    }
    public void LineBullet()
    {
        if (lineActive)
        {
            lineSpawner.SetActive(false);
            lineActive = false;
        }
        else
        {
            lineSpawner.SetActive(true);
            lineActive = true;
        }
    }
    public void ConeBullet()
    {
        if (coneActive)
        {
            coneSpawner.SetActive(false);
            coneActive = false;
        }
        else
        {
            coneSpawner.SetActive(true);
            coneActive = true;
        }
    }
    public void CardinalBullet()
    {
        if (cardinalActive)
        {
            cardinalSpawner.SetActive(false);
            cardinalActive = false;
        }
        else
        {
            cardinalSpawner.SetActive(true);
            cardinalActive = true;
        }
    }
    
}
