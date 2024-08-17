
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonClass<LevelManager>
{
    public Sprite square;
    public float foodNPCs = 5;
    public float hunterNPCs = 2;

    public NPCController npcPrefab;
    public List<NPCController> npcsFood,npcsHunt;

    [Header("Enemies Stats")]
    public List<EnemyStats> enemiesStats;
    [Header("Levels Stats")]
    public List<LevelStats> levelsStats;

    public GameObject[,] background;

    public float minRange = 10f;
    public float maxRange = 15f;

    // Start is called before the first frame update

    public void SpawnBackground(float scale)
    {
        if (background == null)
        {
            GameObject MAP = GameObject.Find("MAP");
            background = new GameObject[200, 200];
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    var go = new GameObject();
                    go.transform.position = new Vector3(i * scale - 50, j * scale - 50, 0);
                    go.transform.parent = MAP.transform;
                    var sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = square;
                    sr.color = (i + j) % 2 == 0 ? new Color(0.5f,0.7f,0.3f) : new Color(0.4f, 0.2f, 0.8f);
                    sr.sortingOrder = -10;
                    go.transform.localScale = new Vector3(scale, scale, 1);
                    background[i, j] = go;
                }
            }
            return;
        }
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                background[i, j].transform.position = new Vector3(i * scale - 50, j * scale - 50, 0);
                background[i, j].transform.localScale = new Vector3(scale, scale, 1);

            }
        }
    }

    public void ManageNewLevel()
    {
        int level = CatController.Instance.massLevel;
        switch (level)
        {
            case 0:
                SpawnBackground(4);
                break;
            case 1:
                SpawnBackground(2);
                break;
            case 2:
                SpawnBackground(1);
                break;
            case 3:
                SpawnBackground(0.5f);
                break;
        }

        foodNPCs = levelsStats[level].foodNPCs;
        hunterNPCs = levelsStats[level].hunterNPCs;

        //update enemies
        // jesus christ what have i cooked????!!
        while (npcsFood.Count > 0)
        {
            Debug.Log(npcsFood.Count);
            var npc = npcsFood[0];
            npcsFood.RemoveAt(0);
            Destroy(npc.gameObject);
        }
        while(npcsHunt.Count > 0 && npcsFood.Count < foodNPCs)
        {
            var npc = npcsHunt[0];
            npc.isHunter = false;
            npc.GetComponent<SpriteRenderer>().sprite = enemiesStats[level].smallSpr;
            npcsHunt.RemoveAt(0);
        }
        while(npcsHunt.Count > 0)
        {
            var npc = npcsHunt[0];
            npcsHunt.RemoveAt(0);
            Destroy(npc.gameObject);
        }
        while(npcsFood.Count < foodNPCs)
        {
            var npc = Instantiate(npcPrefab);
            npc.isHunter = false;
            npcsFood.Add(npc);
            SpawnNpc(npc,false);
        }
        while(npcsHunt.Count < hunterNPCs)
        {
            var npc = Instantiate(npcPrefab);
            npc.isHunter = true;
            npcsHunt.Add(npc);
            SpawnNpc(npc,true);
        }

    }

    void Start()
    {
        npcsFood = new List<NPCController>();
        npcsHunt = new List<NPCController>();

        ManageNewLevel();

    }
    
    private void SpawnNpc(NPCController npc, bool hunter)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        npc.transform.position = new Vector3(Mathf.Cos(angle) * minRange, Mathf.Sin(angle) * minRange, 0) + CatController.Instance.transform.position;

        int level = CatController.Instance.massLevel + (hunter ? 1 : 0);


        npc.speed = enemiesStats[level].speed;
        npc.GetComponent<SpriteRenderer>().sprite = hunter ? enemiesStats[level].bigSpr : enemiesStats[level].smallSpr;
        npc.GetComponent<CircleCollider2D>().radius = hunter ? enemiesStats[level].sizeBig : enemiesStats[level].sizeSmall;
        npc.food = enemiesStats[level].food;
        npc.isHunter = hunter;
        npc.flipHorizontal = enemiesStats[level].flipHorizontal;



    }

    public void RemoveNPC(NPCController go)
    {
        bool hunter = go.isHunter;
        DOTween.TweensByTarget(go.transform)?.ForEach(t => t.Kill());
        SpawnNpc(go, hunter);
    }

    private void Update()
    {
        foreach(var npc in npcsFood)
            if(Vector3.Distance(npc.transform.position, CatController.Instance.transform.position) > maxRange)
                RemoveNPC(npc);
        foreach(var npc in npcsHunt)
            if(Vector3.Distance(npc.transform.position, CatController.Instance.transform.position) > maxRange)
                RemoveNPC(npc);
    }

    private void OnDrawGizmos()
    {
        if(CatController.Instance == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CatController.Instance.transform.position, minRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CatController.Instance.transform.position, maxRange);
    }
}


[System.Serializable]
public class EnemyStats
{
    public string name;
    public int food;
    public Sprite bigSpr, smallSpr;
    public float speed;
    public float sizeBig;
    public float sizeSmall;

    public bool flipHorizontal;
}

[System.Serializable]
public class LevelStats
{
    public int foodNPCs;
    public int hunterNPCs;
    //TODO background :)
}
