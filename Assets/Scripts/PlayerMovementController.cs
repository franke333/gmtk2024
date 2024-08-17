using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerMovementController : SingletonClass<PlayerMovementController>
{
    //movement
    public float maxSpeed = 5f;
    public float acceleration = 1f;
    public AnimationCurve dragCurve;
    private Vector2 velocity;

    private void Start()
    {
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public void AddVelocity(Vector2 velocity)
    {
        this.velocity += velocity;
        // allow bigger max speed
        float oldMaxSpeed = maxSpeed;
        maxSpeed = this.velocity.magnitude;
        DOTween.To(() => maxSpeed, x => maxSpeed = x, oldMaxSpeed, 0.5f).SetEase(Ease.OutCubic);
    }



    public Vector2 ReadInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        var dir = new Vector2(x, y);
        return dir.sqrMagnitude > 1 ? dir.normalized : dir;
    }


    private void UpdateVelocity()
    {
        //Drag
        var dir = ReadInput();
        if(dir == Vector2.zero)
        {
            var massRatio = Mathf.Clamp01(CatController.Instance.mass / CatController.Instance.maxMass);
            velocity *= Mathf.Pow(0.8f * dragCurve.Evaluate(massRatio), 3 * Time.deltaTime);
        }

        velocity += dir * Time.deltaTime * acceleration / CatController.Instance.mass;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void Update()
    {
        UpdateVelocity();

    }
}
