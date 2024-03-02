using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite backSprite; // 카드의 뒷면 스프라이트

    [SerializeField]
    private Sprite cardSprite; // 카드의 앞면 스프라이트, 동적으로 설정됩니다.

    public bool IsFlipped { get; private set; } = false; // 카드가 뒤집혀있는지 여부
    private bool isFlipping = false; // 카드가 뒤집히고 있는지 여부를 추적하는 변수

    private readonly float flipAnimationTime = 1.0f; // 뒤집기 애니메이션 시간

    // 카드의 앞면 스프라이트를 설정하는 메소드
    public void SetCardSprite(Sprite newSprite)
    {
        cardSprite = newSprite;
    }

    public void FlipCard()
    {
        if (isFlipping) return; // 이미 뒤집히고 있다면 반환

        isFlipping = true; // 뒤집기 시작

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, flipAnimationTime / 2).OnComplete(() =>
        {
            IsFlipped = !IsFlipped;
            cardRenderer.sprite = IsFlipped ? cardSprite : backSprite;

            transform.DOScale(originalScale, flipAnimationTime / 2).OnComplete(() =>
            {
                isFlipping = false; // 뒤집기 완료
            });
        });
    }

    void OnMouseDown()
    {
        if (!isFlipping && !IsFlipped) // 이미 뒤집힌 카드는 무시
        {
            AudioManager.instance.PlayCardTakeOutSound(); // 카드 소리 재생
            GameManager.instance.CardClicked(this);
        }
    }
}