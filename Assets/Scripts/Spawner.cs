using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    public Text spawnMessageText;

    private int lastSpawnType = -1;

    int level;
    float timer;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        spawnMessageText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);

        if (spawnData[level].spriteType != lastSpawnType)
        {
            lastSpawnType = spawnData[level].spriteType; 
            StartCoroutine(ShowSpawnMessage()); 
        }
    }
    IEnumerator ShowSpawnMessage()
    {
        spawnMessageText.gameObject.SetActive(true); 
        spawnMessageText.text = "새로운 적이 등장합니다.";
        yield return new WaitForSeconds(3); 
        spawnMessageText.gameObject.SetActive(false); 
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
