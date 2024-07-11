using System.Collections;
using UnityEngine;

namespace SlingShot.Controller
{
    public class BirdFIreController : MonoBehaviour
    {
        private FireData fireData;
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            fireData = new FireData();
        }
    
    
        private void SetStrip(bool isActive)
        {
            fireData.ShotLineRender.idleStrip.SetActive(isActive);
        }
    
        public void BirdFire()
        {
            if (GameManager.gmInstance.CheckBirdNumber())
            {
                fireData.ClickedWithinArea = false; // controller
                fireData.BirdOnSlingShot = false; // controller

                fireData.AngryBirdInstance.LaunchBird(fireData.Direction, fireData.ShotForce); // service

                GameManager.gmInstance.UseShot(); // service
            
                fireData.ShotLineRender.SetLeftLineRender(false);//  service
                fireData.ShotLineRender.SetRightLineRender(false);// service
            
                SetStrip(true); // service

                //궤적 제거
                fireData.Trajectory.ClearPoints(); // service

                if (GameManager.gmInstance.CheckBirdNumber())
                {
                    StartCoroutine(SpawnAngryBirdAfterTime(fireData.AngryBirdInstance, fireData.TimeForBirdRespawn)); // controller
                }
            }
        }
    
        private IEnumerator SpawnAngryBirdAfterTime(AngryBird bird, float time)
        {
            fireData.IsDestroying = true;
            yield return new WaitForSeconds(time);
            fireData.IsDestroying = false;
            SpawnAngryBird();
            //CreatBird();
        }

        public void SpawnAngryBird()
        {
            if (!fireData.IsDestroying)
            {
                var position = fireData.SlingShotData.IdlePosition.position;
                Vector2 dir = (fireData.SlingShotData.CenterPosition.position - position).normalized;

                fireData.AngryBirdInstance = fireData.Pool.Get(); // 새를 풀에서 가져옴
                fireData.AngryBirdInstance.Reset();
                fireData.AngryBirdInstance.transform.position = (Vector2)position + dir * fireData.AngryBirdPositionOffset;
                fireData.BirdOnSlingShot = true;
            }
        }
    }
}