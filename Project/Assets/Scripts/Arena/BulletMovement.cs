using UnityEngine;
using System.Collections;

public class BulletMovement: MonoBehaviour
{
    const float speed = 2f;
    public bool move = false;
    const int destructionTime = 5;
    bool startDestruction = false;
    float reduceAlphaPerSec;
    TrailRenderer trailRend;

    void Start()
    {
        trailRend = transform.GetComponent<TrailRenderer>();
        reduceAlphaPerSec = trailRend.material.color.a / destructionTime;
    }

    void FixedUpdate()
    {
        if (!move)
            return;

        Vector3 direction = transform.forward * speed;
        rigidbody.velocity = direction;
    }

    void Update()
    {
        if (startDestruction)
        {
            Color color = trailRend.material.color;

            color.a -= Time.deltaTime * reduceAlphaPerSec;
            if (color.a <= 0)
                Destroy(gameObject);
            else
                trailRend.material.color = color;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        move = false;
        rigidbody.isKinematic = true;
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
        startDestruction = true;
        if (collision.gameObject.tag.Equals("Monster"))
        {
            Monster monsterCl = collision.gameObject.GetComponent<Monster>();
            int hp = 15 + Random.Range(0, 6);
            HUD.instance.addMessage(monsterCl.monsterName + " lost " + hp +
                " hit points.");
            if (monsterCl.HP - hp > 0)
            {
                monsterCl.HP -= hp;
                OSD.instance.updateTooltip(monsterCl);
            }
            else
            {
                AI.instance.removeMonster(collision.gameObject);
                HUD.instance.addMessage(monsterCl.monsterName + " died.");
            }
        }
        else
            HUD.instance.addMessage("Robot missed.");
    }
}
