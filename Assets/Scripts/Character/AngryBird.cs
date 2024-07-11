using System;
using UnityEngine;
using UnityEngine.Pool;

public class AngryBird : MonoBehaviour
{
    [SerializeField]private GameObject explosionEffect;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    
    private bool launched;
    private bool shouldFaceVelDirection;
    private bool isReleased = false;
    private bool checkHited = false;

    //오브젝트 풀 캐싱
    private IObjectPool<AngryBird> managedPool;
    
    public bool IsReleased => isReleased;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        rb.isKinematic = true;
        circleCollider.enabled = false;
    }
    private void FixedUpdate()
    {
        if (launched && shouldFaceVelDirection && !checkHited)
        {
            transform.right = rb.velocity;
        }
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        circleCollider.enabled = true;
        
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        launched = true;
        shouldFaceVelDirection = true;
        isReleased = true;
        
        Invoke("DestroyBird", 3f);
    }

    #region 풀링

    public void SetManagedPool(IObjectPool<AngryBird> pool)
    {
        managedPool = pool;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkHited = true;
    }

    public void DestroyBird()
    {
        if (isReleased) // 오브젝트가 아직 반환되지 않은 경우에만 풀에 반환
        {
            if (explosionEffect != null)
            {
                GameObject explosionInstantiate = Instantiate(explosionEffect, transform.position, transform.rotation);
                Destroy(explosionInstantiate, 1.0f);
            }
            managedPool.Release(this);
            isReleased = false; // isReleased 상태를 false로 변경
        }
    }

    #endregion

    public void Reset()
    {
        
        transform.rotation = Quaternion.identity;
        
        // 물리 리셋
        rb.velocity = Vector2.zero;

        //
        rb.isKinematic = true;
        
        // 콜라이더 끄기
        circleCollider.enabled = false; 
    }
    
    

}

