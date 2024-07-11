using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float explosionForceThreshold = 3.0f; // 돼지가 터지게 만드는 최소 충돌 세기
    public GameObject explosionEffect; // 터질 때 나타나는 이펙트

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌의 상대 속도
        float collisionForce = collision.relativeVelocity.magnitude;

        if (collisionForce > explosionForceThreshold)
        {
            Explode();
        }
    }

    private void Explode()
    {
        
        // 돼지 오브젝트 제거
        GameManager.gmInstance.RemoveEnemy(this);
        Destroy(gameObject);
        
        // 터지는 이펙트 생성
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosionInstance, 1.0f);
        }
    }
}