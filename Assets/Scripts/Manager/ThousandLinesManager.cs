using UnityEngine;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        public static ThousandLinesManager Instance { get; private set; }

        //õ���� ��
        //õ���� ����(����) �� ��
        //�ڵ�ȭ �ý��ۿ��� ����Ǵ� �����縦
        //�߰� ���� �� ���������ν� ���� ��ġ�ִ� ����� ���׷��̵� �� �Ǹ��ϴ� ����

        // ������Ʈ Ǯ��
        // ������ ���� ����
        // ������ ���ͷ���

        private void Awake()
        {
            Instance = this;
        }
    }
}