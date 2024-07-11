using System;
using System.Collections;
using SlingShot.Controller;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SlingShotLineRender))]
public class SlingShotHandler : MonoBehaviour
{
    

    [Header("Transform")] 
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePosition; // 라인랜더러스크립트에 추가함

    [Header("SlingShot Stat")] 
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private float shotForce = 10f;//fd
    [SerializeField] private float timeForBirdRespawn = 2.0f;//fd

    [Header("Scripts")] 
    [SerializeField] private SlingShotArea slingShotArea;
    [SerializeField] private Trajectory trajectory;//fd

    [Header("Bird")] 
    [SerializeField] private AngryBird angryBirdPrefab;
    [SerializeField] private float angryBirdPositionOffset = 0.25f;//fd
    

    // Data
    private SlingshotData slingshotData;
    
    // Service
    private InputService inputService;

    // View
    private SlingShotLineRender shotLineRender;
    
    // controller
    private BirdFIreController birdFIreController;
    
    
    
    //Position Stat
    private Vector2 direction; //fd
    private Vector2 directionNormalized;

    //For Bird Fire
    private bool clickedWithinArea; //fd
    private bool birdOnSlingShot; //fd
    private bool isDestroying; //fd

    //For Object Pool
    private AngryBird angryBirdInstance;//fd
    private IObjectPool<AngryBird> pool; //fd


    // flow
    private void Awake()
    {
        Initialize();
        
    }

    private void Update()
    {
        if (inputService.IsFireButtonDown() && slingShotArea.IsWithinSlingshotArea())
        {
            // 터치가 가능한 영역에서 잡아 당겼을 때
            clickedWithinArea = true;
        }

        // 누르는 중, 터치가 가능한 영역일 때, 앵그리가 받침대 위에 있을 때 (날라가기 전), 그리고 리소스가 릴리즈 되지 않았을 때
        if (inputService.IsFireButtonHeld() && clickedWithinArea && birdOnSlingShot && !angryBirdInstance.IsReleased)
        {
            DrawSlingShot();
            
            AngryBirdPosition();
            SetStrip(false);

            //궤적 시각화
            Vector2 initialVelocity = direction.normalized * shotForce;
            trajectory.DrawTrajectory((Vector2)centerPosition.position, initialVelocity);
        }

        // 마우스에서 손 땔 때
        if (inputService.IsFireButtonUp() && !angryBirdInstance.IsReleased)
        {
            birdFIreController.BirdFire();
            slingshotData.ResetSlingShotLinePosition(idlePosition.position);
        }
    }


    // functions
    private void Initialize()
    {
        inputService = new InputService();
        slingshotData = new SlingshotData();
        
        shotLineRender = GetComponent<SlingShotLineRender>();
        if (shotLineRender == null)
        {
            Debug.LogError("SlingShotLineRender 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        shotLineRender.InitializeLineRender();
        
        birdFIreController = GetComponent<BirdFIreController>();
        if (birdFIreController == null)
        {
            Debug.LogError("BirdFIreController 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        
        InitializeData();
        InitializePool();
        
        birdFIreController.SpawnAngryBird();
        SetStrip(false);
    }

    // init
    private void InitializePool()
    {
        pool = new ObjectPool<AngryBird>(CreatBird, OnGetBird, OnReleaseBird, OnDestroyBird, maxSize: 4);
    }

    private void InitializeData()
    {
        slingshotData.SlingShotLinePosition = slingshotData.IdleStrip.position;
        slingshotData.LeftStartPosition = transform.Find("StripTransform").Find("StripStartL");
        slingshotData.RightStartPosition = transform.Find("StripTransform").Find("StripStartR");
    }


    // Setter
    private void SetStrip(bool isActive)
    {
        shotLineRender.idleStrip.SetActive(isActive);
    }

    
    
    // ---- 수정 해야 할 친구들

    // private void BirdFire()
    // {
    //     if (GameManager.gmInstance.CheckBirdNumber())
    //     {
    //         clickedWithinArea = false; // controller
    //
    //         angryBirdInstance.LaunchBird(direction, shotForce); // service
    //
    //         GameManager.gmInstance.UseShot(); // service
    //
    //         birdOnSlingShot = false; // controller
    //          
    //         shotLineRender.SetLeftLineRender(false);//  service
    //         shotLineRender.SetRightLineRender(false);// service
    //         
    //         SetStrip(true); // service
    //
    //         //궤적 제거
    //         trajectory.ClearPoints(); // service
    //
    //         if (GameManager.gmInstance.CheckBirdNumber())
    //         {
    //             StartCoroutine(SpawnAngryBirdAfterTime(angryBirdInstance, timeForBirdRespawn)); // controller
    //         }
    //     }
    // }

    #region SlingShot Methods

    /// <summary>
    /// clamp magnitude : 길이제한
    /// </summary>
    private void DrawSlingShot()
    {
        if (Camera.main != null)
        {
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(inputService.GetMousePosition());
            
            var position = centerPosition.position;
            slingshotData.SlingShotLinePosition = position + Vector3.ClampMagnitude(touchPoint - position, maxDistance); // 라인 포지션 계산하는 기능도 Services 뺴는게 좋다.
        }

        shotLineRender.SetLine(slingshotData.SlingShotLinePosition, slingshotData.LeftStartPosition, slingshotData.RightStartPosition);
        
        direction = (Vector2)centerPosition.position - slingshotData.SlingShotLinePosition;
        directionNormalized = direction.normalized;
    }
    

    #endregion

    #region AngryBird Methods

    private void AngryBirdPosition()
    {
        angryBirdInstance.transform.position = slingshotData.SlingShotLinePosition + directionNormalized * angryBirdPositionOffset;
        angryBirdInstance.transform.right = directionNormalized;
    }

    // private IEnumerator SpawnAngryBirdAfterTime(AngryBird bird, float time)
    // {
    //     isDestroying = true;
    //     yield return new WaitForSeconds(time);
    //     isDestroying = false;
    //     SpawnAngryBird();
    // }

    // private void SpawnAngryBird()
    // {
    //     if (!isDestroying)
    //     {
    //         var position = idlePosition.position;
    //         Vector2 dir = (centerPosition.position - position).normalized;
    //
    //         angryBirdInstance = pool.Get(); // 새를 풀에서 가져옴
    //         angryBirdInstance.Reset();
    //         angryBirdInstance.transform.position = (Vector2)position + dir * angryBirdPositionOffset;
    //         birdOnSlingShot = true;
    //     }
    // }

    #endregion

    
    // https://starlightbox.tistory.com/84 (유니티 오브젝트 풀링 사용했는데 풀링 매니저로 관리한  글임)
    // https://velog.io/@gusrb0296/Unity
    
    #region 오브젝트풀링

    private AngryBird CreatBird()
    {
        var position = idlePosition.position;

        shotLineRender.SetLine(slingshotData.SlingShotLinePosition, slingshotData.LeftStartPosition, slingshotData.RightStartPosition);
        
        Vector2 dir = (centerPosition.position - position).normalized;
        Vector2 spawnPosition = (Vector2)position + dir * angryBirdPositionOffset;

        angryBirdInstance = Instantiate(angryBirdPrefab, spawnPosition, Quaternion.identity);
        angryBirdInstance.transform.right = dir;

        birdOnSlingShot = true;

        AngryBird angryBird = angryBirdInstance.GetComponent<AngryBird>();
        angryBird.SetManagedPool(pool);
        return angryBird;
    }

    private void OnGetBird(AngryBird angryBird)
    {
        angryBird.gameObject.SetActive(true);
    }

    private void OnReleaseBird(AngryBird angryBird)
    {
        angryBird.gameObject.SetActive(false);
    }

    private void OnDestroyBird(AngryBird angryBird)
    {
        Destroy(angryBird.gameObject);
    }

    #endregion

    // 주석처리 한게 추가된거고 버튼땔때 생성만 해줘도 리지드바디는 초기화 되는거 같긴함 . 인스턴스 생성 시킨걸 삭제해야 하는데 효율적인 
    // 코스트 사용을 위해서 메모리 풀링을 구현해보자 . }
}