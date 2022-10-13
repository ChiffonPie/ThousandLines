using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace ThousandLines
{
    public  class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        private bool isLock; //터치 제어용
        Camera camera;

        public bool IsLock
        {
            set { isLock = value; }
        }

        private void Awake()
        {
            Instance = this;
            this.isLock = true ; //게임 매니저가 로드를 완료하면 활성화 해준다.

            this.camera = this.GetComponent<Camera>();
            var mouseDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
            mouseDownStream.Subscribe(_ => TouchObject());
        }

        private void TouchObject()
        {
            if (!isLock)
            {
                Vector3 touchPos = Input.mousePosition;
                Vector3 convertVector = camera.ScreenToWorldPoint(touchPos);
                RaycastHit2D hit = Physics2D.Raycast(convertVector, camera.transform.forward);
                if (hit.collider != null)
                {
                    //머신 버튼을 터치
                    if (hit.collider.GetComponent<Machine>() != null)
                    {
                        //베이스 머신은 생상 정지를 시킴
                        if (hit.collider.GetComponent<Machine>().SettingIndex == 0)
                        {
                            hit.collider.GetComponent<Machine>().SetStopMachine(hit.collider.GetComponent<Machine>());
                        }
                        else
                        {
                            hit.collider.GetComponent<Machine>().SetInAndOut(!hit.collider.GetComponent<Machine>().m_isReserved);
                        }
                    }
                }
            }
        }
    }
}