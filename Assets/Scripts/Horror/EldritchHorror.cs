using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldritchHorror : MonoBehaviour
{
    public Sprite mainBody;
    public List<Sprite> bodyParts;
    public List<Sprite> endParts;

    public int maxDepth = 3;

    private void Start()
    {
        SummonHim();
    }

    public void SummonHim()
    {
        Stack<Tuple<GameObject, int>> stack = new Stack<Tuple<GameObject, int>>();
        stack.Push(new (gameObject, 0));

        gameObject.AddComponent<SpriteRenderer>().sprite = mainBody;

        while(stack.Count > 0)
        {
            var (body, depth) = stack.Pop();
            int bodies = UnityEngine.Random.Range(1, 4) + (depth <= 1 ? 1 : 0);
            for (int i = 0; i < bodies; i++)
            {
                GameObject newBody = new GameObject();
                newBody.transform.parent = body.transform;
                newBody.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f) * 2, 0);

                newBody.AddComponent<HorrorPartScript>();

                SpriteRenderer spriteRenderer = newBody.AddComponent<SpriteRenderer>();
                
                bool isEnd = depth == maxDepth ? true : UnityEngine.Random.Range(0, 3) == 0;
                if (isEnd)
                {
                    spriteRenderer.sprite = endParts[UnityEngine.Random.Range(0, endParts.Count)];
                    spriteRenderer.sortingOrder = depth+20;
                }
                else
                {
                    spriteRenderer.sprite = bodyParts[UnityEngine.Random.Range(0, bodyParts.Count)];
                    stack.Push(new(newBody, depth + 1));
                    spriteRenderer.sortingOrder = depth;
                }
            }
        }
    }
}
