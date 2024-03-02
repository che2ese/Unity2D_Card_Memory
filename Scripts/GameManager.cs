using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameBoard gameBoard; // GameBoard ���� �߰�
    private Card firstCard;
    private Card secondCard;

    public float timer;
    public int score = 10; // �ʱ� ����
    private bool gameActive = false; // ������ ���� ������ ���θ� ��Ÿ��
    private bool isTimerPaused = false; // Ÿ�̸Ӹ� �Ͻ������� ���߱� ���� �÷���

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;
        InitializeGame();
        UIManager.instance.UpdateScoreUI(score);
    }
    void Update()
    {
        if (gameActive)
        {
            if (gameActive && !isTimerPaused && timer > 0 )
            {
                timer -= Time.deltaTime;
                UIManager.instance.UpdateTimerUI(timer);
                if (timer <= 0 || score < 0)
                {
                    // Ÿ�̸Ӱ� 0�� �Ǹ� ���� ���� ó��
                    GameOver();
                }
            }
        }
    }
    public void UpdateScore(int points)
    {
        score += points;
        UIManager.instance.UpdateScoreUI(score); // ���� UI ������Ʈ
    }
    public void PauseTimer(bool pause)
    {
        isTimerPaused = pause;
    }


    public void InitializeGame()
    {
        score = 0; // ���� ��ü ���� �ÿ��� ���� �ʱ�ȭ
        UIManager.instance.UpdateScoreUI(score); // �ʱ� ������ UI ������Ʈ
        // ���⿡�� ���� ���� ������ �߰��� �� �ֽ��ϴ�.
    }
    public void StartStage(float stageTime)
    {
        timer = stageTime;
        gameActive = true; // ���� ����
        UIManager.instance.UpdateTimerUI(timer); // Ÿ�̸� UI ������Ʈ
        UIManager.instance.HideGameOver(); // ���� ���� ȭ�� �����
    }

    // ���� ���� ó��
    public void GameOver()
    {
        gameActive = false;
        UIManager.instance.ShowGameOver(score, timer); // ����: �ְ� ������ ���ڷ� �������� ����
        gameBoard.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    // �ְ� ���� �ҷ�����
    public void CardClicked(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            card.FlipCard();
        }
        else if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            card.FlipCard();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1.0f); // ��� ��ٸ� �� �� ī�� ��

        if (firstCard.name == secondCard.name)
        {
            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);
            score += 10;
            AudioManager.instance.PlayQuizShowCorrectSound();
            gameBoard.CheckAllCardsDisabled(); // ��� ī�尡 ��Ȱ��ȭ�Ǿ����� Ȯ��
        }
        else
        {
            firstCard.FlipCard();
            secondCard.FlipCard();
            score -= 5;
            AudioManager.instance.PlayQuizShowWrongSound();
        }

        UIManager.instance.UpdateScoreUI(score);

        firstCard = null;
        secondCard = null;
    }

}