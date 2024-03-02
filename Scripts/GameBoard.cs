using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private CardManager cardManager;
    private List<Card> allCards = new List<Card>();
    public void InitBoard(int rowCount, int colCount)
    {
        // ī�� ���� ����
        float spaceX = 1.75f;
        float spaceY = 2.75f;

        // �׸����� ���� ��ġ ���
        float totalWidth = (colCount - 1) * spaceX;
        float totalHeight = (rowCount - 1) * spaceY;
        float startX = -totalWidth / 2;
        float startY = totalHeight / 2;

        // ���� ī�� ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int totalCardsNeeded = rowCount * colCount;
        int uniqueCardsNeeded = totalCardsNeeded / 2;

        // ��ġ�� ��ġ�� �����ϴ� ����Ʈ ����
        List<Vector3> positions = new List<Vector3>();

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                Vector3 pos = new Vector3(startX + col * spaceX, startY - row * spaceY, 0f);
                positions.Add(pos);
            }
        }

        // ��ġ ����Ʈ�� ����
        ShuffleList(positions);
        allCards.Clear();
        // ���� ��ġ�� ���� ī�� ��ġ
        for (int i = 0; i < uniqueCardsNeeded && i < cardManager.cards.Length; i++)
        {
            int cardIndex = i; // ���⼭�� �ܼ��� ó������ ������� ī�带 �����մϴ�.

            for (int j = 0; j < 2; j++) // �� ���õ� ī�带 2�� ��ġ
            {
                int posIndex = (i * 2 + j) % positions.Count; // ������ �ε���
                if (posIndex < positions.Count)
                {
                    GameObject cardObject = Instantiate(cardManager.cards[cardIndex], positions[posIndex], Quaternion.identity, this.transform);
                    Card cardComponent = cardObject.GetComponent<Card>(); // GameObject���� Card ������Ʈ�� ������
                    if (cardComponent != null) // Card ������Ʈ�� �����ϸ�
                    {
                        allCards.Add(cardComponent); // ������ ī�带 ����Ʈ�� �߰�
                    }
                }
            }
        }

    }
    // ��� ī�带 �Ͻ������� ������ �޼ҵ�
    public IEnumerator RevealCardsTemporarily(float duration)
    {
        GameManager.instance.PauseTimer(true); // Ÿ�̸� �Ͻ� ����

        foreach (var card in allCards)
        {
            card.FlipCard(); // ��� ī�� ������
        }

        yield return new WaitForSeconds(duration); // ������ �ð� ���� ���

        foreach (var card in allCards)
        {
            card.FlipCard(); // �ٽ� ��� ī�带 ������ ���� ���·�
        }

        GameManager.instance.PauseTimer(false); // Ÿ�̸� �簳
    }
    // ����Ʈ ���� �޼ҵ�
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void CheckAllCardsDisabled()
    {
        foreach (var card in allCards)
        {
            if (card.gameObject.activeSelf)
            {
                return; // �ϳ��� Ȱ��ȭ�� ī�尡 �ִٸ� ����
            }
        }

        // ��� ī�尡 ��Ȱ��ȭ�Ǿ��ٸ� ���� ���������� �Ѿ
        FindObjectOfType<StageManager>().NextStage();
    }
}