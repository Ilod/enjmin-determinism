using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAI : MonoBehaviour {
    public IPlayerController controller;
    public float speed = 1;
    public Rect bounds;
    public GameObject rotationObject;
    public GameObject arrowPrefab;
    public float arrowDelay = 2;
    private float timeBeforeNextArrow = 0;
    public float respawnTime = 5;
    public float invincibilityDuration = 1;
    public float invincibilityRemaining = 0;
    public bool dead = false;
    public int score = 0;
    public Color color;
    public int index = 0;
    public CustomImage image;
    public CustomImage arrowImage;

    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    public IntEvent onScoreChanged;

	// Use this for initialization
	IEnumerator Start ()
    {
        var customizer = new SpriteCustomizer(image, color, index);
        yield return StartCoroutine(customizer.Customize());
        GetComponentInChildren<SpriteRenderer>().sprite = customizer.Sprite;
        gameObject.layer = LayerMask.NameToLayer($"Player{index}");
    }
	
	// Update is called once per frame
	void Update ()
    {
        controller.UpdateControls();
        if (timeBeforeNextArrow > 0)
            timeBeforeNextArrow -= GameplayTime.StepTime;
        if (invincibilityRemaining > 0)
            invincibilityRemaining -= GameplayTime.StepTime;
        if (dead)
            return;
        var controls = controller.GetControls();
        Vector3 direction = new Vector3();
        if (controls.Down)
            direction.y -= 1;
        if (controls.Up)
            direction.y += 1;
        if (controls.Left)
            direction.x -= 1;
        if (controls.Right)
            direction.x += 1;
        var position = gameObject.transform.position;
        position += direction.normalized * speed * GameplayTime.StepTime;
        position.x = Mathf.Clamp(position.x, bounds.xMin, bounds.xMax);
        position.y = Mathf.Clamp(position.y, bounds.yMin, bounds.yMax);
        gameObject.transform.position = position;
        if (direction != Vector3.zero)
        {
            rotationObject.transform.eulerAngles = (Vector3.forward * Vector3.SignedAngle(Vector3.up, direction, Vector3.forward));
        }
        if (controls.Shoot && timeBeforeNextArrow <= 0)
        {
            StartCoroutine(ShootArrow());
        }
    }

    public void StartInvincibility()
    {
        invincibilityRemaining = invincibilityDuration;
    }

    private IEnumerator ShootArrow()
    {
        timeBeforeNextArrow = arrowDelay;
        var spriteCustomizer = new SpriteCustomizer(arrowImage, color, index);
        yield return StartCoroutine(spriteCustomizer.Customize());
        var arrow = Instantiate(arrowPrefab, rotationObject.transform.position, rotationObject.transform.rotation);
        arrow.GetComponent<ArrowAI>().player = this;
        arrow.GetComponent<SpriteRenderer>().sprite = spriteCustomizer.Sprite;
        IncrementScore(-1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var arrow = collision.gameObject.GetComponent<ArrowAI>();
        if (arrow == null)
            return;
        if (dead)
            return;
        if (invincibilityRemaining > 0)
        {
            Destroy(collision.gameObject);
            return;
        }
        IncrementScore(-10);
        var killer = arrow.player;
        killer.IncrementScore(20);
        killer.StartInvincibility();
        Destroy(collision.gameObject);
        StartCoroutine(Reactivate());
        rotationObject.SetActive(false);
        dead = true;
    }

    private void IncrementScore(int increment)
    {
        score += increment;
        onScoreChanged.Invoke(score);
    }

    private IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(respawnTime);
        rotationObject.SetActive(true);
        dead = false;
    }
}
