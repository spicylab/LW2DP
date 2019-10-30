using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LW2DCharacter : MonoBehaviour
{
    public LayerMask wallLayer;

    public Vector2 speed;
    public Vector2 force;
    public float damping = 0.1f;

    [Header("Setting")]
    public bool forceGroundedOnStart = false;

    public bool isLeft => speed.x < 0;
    public bool isGrounded;

    private float safeUntilX;

    private float halfWidth, halfHeight;
    private float gravity;
    private bool shouldUpdateGrounded;

    private void Start()
    {
        if (forceGroundedOnStart)
            StickToGround();
    }
    private void OnEnable()
    {
        var collider = GetComponent<Collider2D>();
        halfWidth = collider.bounds.size.x / 2;
        halfHeight = collider.bounds.size.y / 2;

        UpdateGrounded();
        UpdateSafeUntil();
    }

    private void Update()
    {
        if (isGrounded == false)
        {
            gravity += 4.8f * Time.deltaTime;
            shouldUpdateGrounded = true;
        }
        if (force.magnitude > 0.1f)
            shouldUpdateGrounded = true;

        var movement = speed * Time.deltaTime;
        movement += new Vector2(0, -gravity) * Time.deltaTime;
        movement += force * Time.deltaTime;
        transform.Translate(movement);

        if (shouldUpdateGrounded)
        {
            UpdateGrounded();
            shouldUpdateGrounded = false;
        }

        if (isGrounded)
        {
            if ((isLeft && transform.position.x < safeUntilX) ||
                (!isLeft && transform.position.x > safeUntilX))
            {
                speed *= new Vector2(-1, 1);
                UpdateSafeUntil();
            }
            if ((isLeft && safeUntilX > transform.position.x) ||
                (!isLeft && safeUntilX < transform.position.x))
                UpdateSafeUntil();
        }

        if (force.magnitude > 0.1f)
            force = force * (1 - damping);
        else
            force = Vector2.zero;
    }

    private void UpdateSafeUntil()
    {
        if (isGrounded == false)
        {
            return;
        }

        var hit = Physics2D.Raycast(transform.position, speed, 10, wallLayer.value);
        if (hit.collider != null)
        {
            var dist = hit.point.x - transform.position.x + (isLeft ? halfWidth : -halfWidth);
            safeUntilX = transform.position.x + dist;

            return;
        }
        else
            safeUntilX = transform.position.x + (isLeft ? -3 : 3);

        hit = Physics2D.Raycast(
            transform.position + new Vector3(isLeft ? -3 : 3, 0),
            transform.up * -1,
            3,
            wallLayer.value);
        if (hit.collider == null)
        {
            hit = Physics2D.Raycast(
                transform.position + new Vector3(isLeft ? -3 : 3, -(halfHeight + .1f)),
                new Vector3(isLeft ? 1: -1, 0),
                3,
                wallLayer.value);

            var dist = hit.point.x - transform.position.x + (isLeft ? halfWidth : -halfWidth);
            safeUntilX = transform.position.x + dist;
        }
    }
    private void UpdateGrounded()
    {
        if (StickToGround(0.01f) == false)
            isGrounded = false;
    }

    public bool StickToGround(float maxDistance = 10f)
    {
        var hit = Physics2D.Raycast(
            transform.position - new Vector3(0, halfHeight + 0.01f),
            transform.up * -1,
            maxDistance,
            wallLayer);

        if (hit.collider != null)
        {
            var pos = transform.position;
            pos.y = hit.collider.transform.position.y + hit.collider.bounds.size.y / 2 +
                halfHeight;
            transform.position = pos;

            gravity = 0;
            isGrounded = true;

            return true;
        }
        return false;
    }

    public void AddForce(Vector2 xy)
    {
        force += xy;
    }
}
