using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace ThousandLines
{
    public  class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        public bool isLock; //터치 제어용
        Camera camera;

        private void Awake()
        {
            Instance = this;
            isLock = false;

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
                RaycastHit2D hitInformation = Physics2D.Raycast(convertVector, camera.transform.forward);
                if (hitInformation.collider != null)
                {
                    //머신 버튼을 터치
                    if (hitInformation.collider.GetComponent<Machine>() != null)
                    {
                        hitInformation.collider.GetComponent<Machine>().SetInAndOut(!hitInformation.collider.GetComponent<Machine>().m_isReserved);
                    }
                }
            }
        }
    }
}