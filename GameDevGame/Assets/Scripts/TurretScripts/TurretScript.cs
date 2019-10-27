﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [Header("Basic Attributes")]
    public string turretName = "Turret";
    public float range;
    public float fireRatePerSec;
    public float damage;

    [Header("Bullet turret")]
    public bool bulletTurret = true;
    public GameObject bullet;

    [Header("Missile turret")]
    public bool missileTurret = false;
    public GameObject missile;
    public float aoeDamage;

    [Header("Pulse turret")]
    public bool pulseTurret = false;

    [Header("Freeze turret")]
    public bool freezeTurret = false;
    public float slowDuration;
    public float slowPrentageAmount;

    [Header("Death laser")]
    public bool DeathLaserTurret = false;

    [Header("upgrade names")]
    public string upgradeName1 = "Upgrade 1";
    public int upgradeCost1 = 0;
    public string upgradeName2 = "Upgrade 2";
    public int upgradeCost2 = 0;
    public string upgradeName3 = "Upgrade 3";
    public int upgradeCost3 = 0;

    [Header("Setup fields (ignore)")]
    public string enemyTag = "Enemy";
    public string planetTag = "Planet";
    public Transform rotateAxis;
    public Transform firePoint;
    public GameObject fireEffect;
    public LineRenderer laserEffect;

    private Transform target;
    private float fireCountDown = 0f;
    private bool upgraded1 = false;
    private bool upgraded2 = false;
    private bool upgraded3 = false;

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
                //check that planet doesn't obstruct the view to enemy
                //RaycastHit hit;
                //if (Physics.Raycast(firePoint.position,enemy.transform.position, out hit))
                //{
                    //if (!hit.collider.tag.Equals(planetTag))
                    //{
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    //}
                //}
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
        //cannon turret
        if (bulletTurret)
        {
            GameObject thisBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            BulletScript bulletScript = thisBullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.setTargetTo(target);
                bulletScript.setDamage(damage);
            }
        }

        //missile turret
        else if (missileTurret)
        {
            GameObject thisMissile = Instantiate(missile, firePoint.position, firePoint.rotation);
            MissileScript missileScript = thisMissile.GetComponent<MissileScript>();
            if (missileScript != null)
            {
                missileScript.setTargetTo(target);
                missileScript.setDamage(damage, aoeDamage);
            }
        }

        //pulse turret
        else if (pulseTurret)
        {
            Collider[] objectsHit = Physics.OverlapSphere(firePoint.position, range);
            foreach (Collider c in objectsHit)
            {
                if (c.tag.Equals(enemyTag))c.gameObject.GetComponent<EnemyAIScript>().takeDamage(damage);
            }
        }

        //freeze turret
        else if (freezeTurret)
        {
            GameObject enemy = target.gameObject;
            enemy.GetComponent<EnemyAIScript>().slow(slowDuration, slowPrentageAmount);
            enemy.GetComponent<EnemyAIScript>().takeDamage(damage);

            laserEffect.SetPosition(0, firePoint.position);
            laserEffect.SetPosition(1, target.position);
        }

        //death laser
        else if (DeathLaserTurret)
        {
            GameObject enemy = target.gameObject;
            enemy.GetComponent<EnemyAIScript>().takeDamage(damage);

            laserEffect.SetPosition(0, firePoint.position);
            laserEffect.SetPosition(1, target.position);
        }

        //firing effect
        if (fireEffect != null)
        {
            GameObject fireParticles = Instantiate(fireEffect, firePoint.position, Quaternion.LookRotation(target.position - transform.position));
            Destroy(fireParticles, 1f);
        }
    }
    
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
        else resetLaser();
    }

    public void resetLaser()
    {
        if (laserEffect != null) {
            laserEffect.SetPosition(0, firePoint.position);
            laserEffect.SetPosition(1, firePoint.position);
        }
    }
    
    //upgrades turret, returns true if successful
    public bool UpgradeTurret(int upgradeNum)
    {
        //cannon turret
        if (bulletTurret)
        {
            if (upgradeNum == 1)
            {
                if (Player.monies > upgradeCost1)
                {
                    fireRatePerSec += 1;
                    Player.monies -= upgradeCost1;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 2)
            {
                if (Player.monies > upgradeCost2)
                {
                    damage += 5;
                    Player.monies -= upgradeCost2;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 3)
            {
                if (Player.monies > upgradeCost3)
                {
                    Player.monies -= upgradeCost3;
                    return true;
                }
                else return false;
            }
        }

        //missile turret
        else if (missileTurret)
        {
            if (upgradeNum == 1)
            {
                if (Player.monies > upgradeCost1)
                {
                    range += 100;
                    Player.monies -= upgradeCost1;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 2)
            {
                if (Player.monies > upgradeCost2)
                {
                    Player.monies -= upgradeCost2;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 3)
            {
                if (Player.monies > upgradeCost3)
                {
                    Player.monies -= upgradeCost3;
                    return true;
                }
                else return false;
            }
        }

        //pulse turret
        else if (pulseTurret)
        {
            if (upgradeNum == 1)
            {
                if (Player.monies > upgradeCost1)
                {
                    fireRatePerSec += 1; 
                    Player.monies -= upgradeCost1;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 2)
            {
                if (Player.monies > upgradeCost2)
                {
                    Player.monies -= upgradeCost2;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 3)
            {
                if (Player.monies > upgradeCost3)
                {
                    Player.monies -= upgradeCost3;
                    return true;
                }
                else return false;
            }
        }

        //freeze turret
        else if (freezeTurret)
        {
            if (upgradeNum == 1)
            {
                if (Player.monies > upgradeCost1)
                {
                    slowPrentageAmount += 0.25f;
                    Player.monies -= upgradeCost1;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 2)
            {
                if (Player.monies > upgradeCost2)
                {
                    Player.monies -= upgradeCost2;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 3)
            {
                if (Player.monies > upgradeCost3)
                {
                    Player.monies -= upgradeCost3;
                    return true;
                }
                else return false;
            }
        }

        //death laser
        else if (DeathLaserTurret)
        {
            if (upgradeNum == 1)
            {
                if (Player.monies > upgradeCost1)
                {
                    damage += 16;
                    Player.monies -= upgradeCost1;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 2)
            {
                if (Player.monies > upgradeCost2)
                {
                    Player.monies -= upgradeCost2;
                    return true;
                }
                else return false;
            }
            if (upgradeNum == 3)
            {
                if (Player.monies > upgradeCost3)
                {
                    Player.monies -= upgradeCost3;
                    return true;
                }
                else return false;
            }
        }

        return false;
    }
}