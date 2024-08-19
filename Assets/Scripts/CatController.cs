using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatController : SingletonClass<CatController>
{
    public float mass = 1f;
    public float maxMass = 10f;

    public float foodNeeded = 10f;
    public float food = 0f;

    public int massLevel = 0;

    public int life = 3;
    public bool iFrame = false;

    public List<int> massLevelToChonkSpriteLevel;
    public List<CatSrpiteLevel> catSrpiteLevels;
    public List<int> foodToLevelUp;
    SpriteRenderer sr;
    Animator animator;

    public GameObject activeUpdate, passiveUpdate, endGameWinScreen, endGameLoseScreen;
    public Image healthIcon;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        foodNeeded = foodToLevelUp[massLevel];
    }

    private void Update()
    {
        Vector2 dir = PlayerMovementController.Instance.ReadInput();
        int chonkLevel = massLevelToChonkSpriteLevel[massLevel];

        AudioManager.Instance.Walking = PlayerMovementController.Instance.GetVelocity().magnitude > 0.05f;
        animator.speed = dir.magnitude * 1f;

        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && Mathf.Abs(dir.x)>0.01f)
            animator.runtimeAnimatorController = dir.x > 0 ? catSrpiteLevels[chonkLevel].right : catSrpiteLevels[chonkLevel].left;
        else if(Mathf.Abs(dir.y) > 0.01f)
            animator.runtimeAnimatorController = dir.y > 0 ? catSrpiteLevels[chonkLevel].up : catSrpiteLevels[chonkLevel].down;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            
            NPCController npc = other.GetComponent<NPCController>();

            if (npc.isHunter)
            {
                npc.StartCoroutine(npc.Stun(3f));
                if (iFrame)
                    return;
                life--;
                AudioManager.Instance.PlayDamageTaken();
                //red shake
                healthIcon.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
                healthIcon.DOColor(Color.red, 0.4f).OnComplete(() => healthIcon.DOColor(Color.white, 0.4f));
                StartCoroutine(IFrames(2.5f));

                if (life <= 0)
                {
                    AudioManager.Instance.PlayGameOver();
                    endGameLoseScreen.SetActive(true);
                }
                return;
            }

            food += npc.food;
            AudioManager.Instance.PlayEat();

            if(food >= foodNeeded)
            {
                massLevel++;
                AudioManager.Instance.PlayLevelUp();
                food = 0;
                mass += 1;
                foodNeeded = foodToLevelUp[massLevel];
                if(massLevel == 3)
                {
                    GameManager.Instance.PauseGame();
                    activeUpdate.SetActive(true);
                }
                if(massLevel == 5)
                {
                    GameManager.Instance.PauseGame();
                    passiveUpdate.SetActive(true);
                }
                LevelManager.Instance.ManageNewLevel();
            }
            else
                LevelManager.Instance.RemoveNPC(npc);
        }
    }

    private IEnumerator IFrames(float duration)
    {
        iFrame = true;
        var tween = sr.DOColor(new Color(1, 1, 1, 0.5f), 0.1f).SetLoops(-1,LoopType.Yoyo);
        yield return new WaitForSeconds(duration);
        tween.Kill();
        sr.color = new Color(1, 1, 1, 1);
        iFrame = false;
    }

    public IEnumerator WinSequence()
    {
        PlayerMovementController.Instance.enabled = false;
        transform.position = new Vector3(0, 0, 0);
        FoliageScript.Respawn();
        NPCController horror = LevelManager.Instance.finalBossInstance;
        horror.speed = 0;
        horror.transform.position = new Vector3(5, 0, 0);
        yield return new WaitForSeconds(3f);
        transform.DOScale(25, 10f).OnComplete(() => endGameWinScreen.SetActive(true));
        horror.transform.DOScale(0.05f, 5f).OnComplete(() => horror.transform.DOMove(Vector3.zero,2.5f).OnComplete(() => Destroy(horror.gameObject)));
    }
}

[System.Serializable]
public struct CatSrpiteLevel
{
    public RuntimeAnimatorController up,right,down,left;
}
