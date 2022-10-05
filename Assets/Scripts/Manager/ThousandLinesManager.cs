using UnityEngine;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //전체적인 게임 시스템 및 시퀀스를 관리한다.
        public static ThousandLinesManager Instance { get; private set; }

        //천개의 줄
        //천개의 공정(라인) 의 뜻
        //자동화 시스템에서 생산되는 원자재를
        //추가 공정 및 가공함으로써 더욱 가치있는 자재로 업그레이드 후 판매하는 게임

        //머신 하나당 1개의 메테리얼만 가질 수 있다. (다음 소비가 안되면 멈춤)

        // 오브젝트 풀링
        // 아이템 구조 설계
        // 아이템 인터렉션

        private void Awake()
        {
            Instance = this;
        }
    }
}