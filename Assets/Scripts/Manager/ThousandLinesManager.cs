using UnityEngine;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.
        public static ThousandLinesManager Instance { get; private set; }

        //õ���� ��
        //õ���� ����(����) �� ��
        //�ڵ�ȭ �ý��ۿ��� ����Ǵ� �����縦
        //�߰� ���� �� ���������ν� ���� ��ġ�ִ� ����� ���׷��̵� �� �Ǹ��ϴ� ����

        //�ӽ� �ϳ��� 1���� ���׸��� ���� �� �ִ�. (���� �Һ� �ȵǸ� ����)
        //���� ��Ÿ���� �����Ѵ�.

        // ������Ʈ Ǯ��
        // ������ ���� ����
        // ������ ���ͷ���

        private void Awake()
        {
            Instance = this;
        }
        public void Initiaize()
        {

        }
    }
}