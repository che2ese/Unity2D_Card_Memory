using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite backSprite; // ī���� �޸� ��������Ʈ

    [SerializeField]
    private Sprite cardSprite; // ī���� �ո� ��������Ʈ, �������� �����˴ϴ�.

    public bool IsFlipped { get; private set; } = false; // ī�尡 �������ִ��� ����
    private bool isFlipping = false; // ī�尡 �������� �ִ��� ���θ� �����ϴ� ����

    private readonly float flipAnimationTime = 1.0f; // ������ �ִϸ��̼� �ð�

    // ī���� �ո� ��������Ʈ�� �����ϴ� �޼ҵ�
    public void SetCardSprite(Sprite newSprite)
    {
        cardSprite = newSprite;
    }

    public void FlipCard()
    {
        if (isFlipping) return; // �̹� �������� �ִٸ� ��ȯ

        isFlipping = true; // ������ ����

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, flipAnimationTime / 2).OnComplete(() =>
        {
            IsFlipped = !IsFlipped;
            cardRenderer.sprite = IsFlipped ? cardSprite : backSprite;

            transform.DOScale(originalScale, flipAnimationTime / 2).OnComplete(() =>
            {
                isFlipping = false; // ������ �Ϸ�
            });
        });
    }

    void OnMouseDown()
    {
        if (!isFlipping && !IsFlipped) // �̹� ������ ī��� ����
        {
            AudioManager.instance.PlayCardTakeOutSound(); // ī�� �Ҹ� ���
            GameManager.instance.CardClicked(this);
        }
    }
}