using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : SingletonClass<CatController>
{
    public float mass = 1f;
    public float maxMass = 10f;

    public float foodNeeded = 10f;
    public float food = 0f;

    public int massLevel = 0;

    public int life = 3;

    public List<int> massLevelToChonkSpriteLevel;
    public List<CatSrpiteLevel> catSrpiteLevels;
    public List<int> foodToLevelUp;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        foodNeeded = foodToLevelUp[massLevel];
    }

    private void Update()
    {
        Vector2 dir = PlayerMovementController.Instance.ReadInput();
        int chonkLevel = massLevelToChonkSpriteLevel[Mathf.Min(massLevel,catSrpiteLevels.Count-1)];
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && Mathf.Abs(dir.x)>0.01f)
            sr.sprite = dir.x > 0 ? catSrpiteLevels[chonkLevel].right : catSrpiteLevels[chonkLevel].left;
        else if(Mathf.Abs(dir.y) > 0.01f)
            sr.sprite = dir.y > 0 ? catSrpiteLevels[chonkLevel].up : catSrpiteLevels[chonkLevel].down;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            NPCController npc = other.GetComponent<NPCController>();
            food += npc.food;
            if(food >= foodNeeded)
            {
                massLevel++;
                food = 0;
                mass += 1;
                foodNeeded = foodToLevelUp[massLevel];
                LevelManager.Instance.ManageNewLevel();
            }
            else
                LevelManager.Instance.RemoveNPC(npc);
        }
    }
}

[System.Serializable]
public struct CatSrpiteLevel
{
    public Sprite up,right,down,left;
}
