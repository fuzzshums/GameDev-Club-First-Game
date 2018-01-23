using UnityEngine;
using System.Collections;

/*
* A Spike that falls straight down
*/

public class DropSpike : Spike
{
    public float ySpeed;
    private Vector3 pointing;
    private Spawner spawner;

    void Start()
    {
        base.Initiate();
        spawner = GetComponentInParent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Move()
    {
        pointing = new Vector3(0, 0, -180);
        transform.eulerAngles = pointing;
        transform.Translate(-transform.up * ySpeed * Time.deltaTime);   
    }

    public override void Die()
    {
        spawner.Despawn(gameObject);
    }
}
