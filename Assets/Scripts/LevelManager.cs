
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

    public Material worldMaterial;

    public NPCController finalBoss;
    public NPCController finalBossInstance;

    public float minRange = 10f;
    public float maxRange = 15f;

    public List<FoliageSprites> foliageSprites;

    // Start is called before the first frame update



    public Sprite GetFoliage()
    {
        int level = CatController.Instance.massLevel;
        return foliageSprites[level].sprites[Random.Range(0, foliageSprites[level].sprites.Count)];
    }

    public void ManageNewLevel()
    {
        int level = CatController.Instance.massLevel;

        //TODO update foliage
        float target = 6_000 * Mathf.Pow(2f, level);
        worldMaterial.SetFloat("_Scale", target);
        worldMaterial.SetInt("_Stars", 0);
        FoliageScript.Respawn();
        if (level == 7)
        {
            worldMaterial.SetFloat("_Scale", 100000);
            worldMaterial.SetInt("_Stars", 1);
            worldMaterial.SetColor("_Color1", Color.black);
            worldMaterial.SetColor("_Color2", new Color(0.05f,0.05f,0.05f));
            worldMaterial.SetColor("_Ground", Color.black);
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

        if(level >= 6)
        {   if(level == 6)
                AudioManager.Instance.StartBossMusic();
            if(finalBossInstance != null)
                Destroy(finalBossInstance.gameObject);
            finalBossInstance = Instantiate(finalBoss);
            finalBossInstance.transform.position = CatController.Instance.transform.position + Vector3.right * 8;
            if (level == 7)
            {
                finalBossInstance.isHunter = false;
                finalBossInstance.transform.localScale = Vector3.one * 0.05f;

                StartCoroutine(CatController.Instance.WinSequence());
            }
        }
    }

    void Start()
    {
        npcsFood = new List<NPCController>();
        npcsHunt = new List<NPCController>();

        ManageNewLevel();
        worldMaterial.SetFloat("_Scale", 6_000);

        worldMaterial.SetColor("_Color1", new Color(0.27058f, 0.3333f, 0.129411f));
        worldMaterial.SetColor("_Color2", new Color(0.4352f, 0.415f, 0.17f));
        worldMaterial.SetColor("_Ground", new Color(0.4117f, 0.309f, 0.1607f));

        FoliageScript.instances = new List<FoliageScript>();
        for (int i = 0; i < 20; i++)
        {
            new GameObject().AddComponent<SpriteRenderer>().gameObject.AddComponent<FoliageScript>();
        }
        FoliageScript.Respawn();
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

[System.Serializable]
public class FoliageSprites
{
    public List<Sprite> sprites;
}
