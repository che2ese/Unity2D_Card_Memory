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
        // 카드 간격 설정
        float spaceX = 1.75f;
        float spaceY = 2.75f;

        // 그리드의 시작 위치 계산
        float totalWidth = (colCount - 1) * spaceX;
        float totalHeight = (rowCount - 1) * spaceY;
        float startX = -totalWidth / 2;
        float startY = totalHeight / 2;

        // 기존 카드 정리
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int totalCardsNeeded = rowCount * colCount;
        int uniqueCardsNeeded = totalCardsNeeded / 2;

        // 배치할 위치를 저장하는 리스트 생성
        List<Vector3> positions = new List<Vector3>();

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                Vector3 pos = new Vector3(startX + col * spaceX, startY - row * spaceY, 0f);
                positions.Add(pos);
            }
        }

        // 위치 리스트를 섞음
        ShuffleList(positions);
        allCards.Clear();
        // 섞인 위치에 따라 카드 배치
        for (int i = 0; i < uniqueCardsNeeded && i < cardManager.cards.Length; i++)
        {
            int cardIndex = i; // 여기서는 단순히 처음부터 순서대로 카드를 선택합니다.

            for (int j = 0; j < 2; j++) // 각 선택된 카드를 2번 배치
            {
                int posIndex = (i * 2 + j) % positions.Count; // 포지션 인덱스
                if (posIndex < positions.Count)
                {
                    GameObject cardObject = Instantiate(cardManager.cards[cardIndex], positions[posIndex], Quaternion.identity, this.transform);
                    Card cardComponent = cardObject.GetComponent<Card>(); // GameObject에서 Card 컴포넌트를 가져옴
                    if (cardComponent != null) // Card 컴포넌트가 존재하면
                    {
                        allCards.Add(cardComponent); // 생성된 카드를 리스트에 추가
                    }
                }
            }
        }

    }
    // 모든 카드를 일시적으로 뒤집는 메소드
    public IEnumerator RevealCardsTemporarily(float duration)
    {
        GameManager.instance.PauseTimer(true); // 타이머 일시 정지

        foreach (var card in allCards)
        {
            card.FlipCard(); // 모든 카드 뒤집기
        }

        yield return new WaitForSeconds(duration); // 지정된 시간 동안 대기

        foreach (var card in allCards)
        {
            card.FlipCard(); // 다시 모든 카드를 뒤집어 원래 상태로
        }

        GameManager.instance.PauseTimer(false); // 타이머 재개
    }
    // 리스트 섞기 메소드
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
                return; // 하나라도 활성화된 카드가 있다면 리턴
            }
        }

        // 모든 카드가 비활성화되었다면 다음 스테이지로 넘어감
        FindObjectOfType<StageManager>().NextStage();
    }
}