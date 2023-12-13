using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpell : BasicSpell
{
    public Vector3 movingAxis;
    public float speed;
    private Vector3 movingWorldAxis;

    public float collisionRadius = 0.2f;
    public LayerMask collisionLayer;

    public ParticleSystem bolt;
    public ParticleSystem trail;
    public ParticleSystem explosion;


    private bool hasExploded = false;
    public override void Initialize(Transform wantTip)
    {
        base.Initialize(wantTip);
        movingWorldAxis = wantTip.TransformDirection(movingAxis);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * movingWorldAxis * speed;
    }

    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, collisionRadius, collisionLayer);

            if (results.Length > 0)
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        hasExploded = true;

        explosion.Play();
        trail.Stop();
        bolt.Stop();

        Destroy(gameObject, 1);
    }
}
