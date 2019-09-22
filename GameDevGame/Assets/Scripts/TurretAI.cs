﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [Header ("Attributes")]
    public float range=100f;
    public float fireRatePerSec=2;
    private float fireCountDown = 0f;

    [Header("Setup fields")]
    public string enemyTag = "Enemy";
    public Transform rotateAxis;
    public GameObject bullet;
    public Transform firePoint;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        } else
        {
            target = null;
        }
    }

    void Shoot(Transform target)
    {
        GameObject thisBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation);
        BulletScript bulletScript = thisBullet.GetComponent<BulletScript>();
        if(bulletScript != null)
        {
            bulletScript.setTargetTo(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 directionOfEnemy = target.position - transform.position;
            Quaternion lookDirection = Quaternion.LookRotation(directionOfEnemy);
            rotateAxis.rotation = lookDirection;


            if (fireCountDown <= 0)
            {
                Shoot(target);
                fireCountDown = 1f / fireRatePerSec;
            }

            fireCountDown -= Time.deltaTime;

        }
    }
}
