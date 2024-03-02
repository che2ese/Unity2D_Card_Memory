using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameBoard gameBoard; // GameBoard 참조 추가
    private Card firstCard;
    private Card secondCard;

    public float timer;
    public int score = 10; // 초기 점수
    private bool gameActive = false; // 게임이 진행 중인지 여부를 나타냄
    private bool isTimerPaused = false; // 타이머를 일시적으로 멈추기 위한 플래그

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
                    // 타이머가 0이 되면 게임 오버 처리
                    GameOver();
                }
            }
        }
    }
    public void UpdateScore(int points)
    {
        score += points;
        UIManager.instance.UpdateScoreUI(score); // 점수 UI 업데이트
    }
    public void PauseTimer(bool pause)
    {
        isTimerPaused = pause;
    }


    public void InitializeGame()
    {
        score = 0; // 게임 전체 시작 시에만 점수 초기화
        UIManager.instance.UpdateScoreUI(score); // 초기 점수로 UI 업데이트
        // 여기에서 게임 시작 로직을 추가할 수 있습니다.
    }
    public void StartStage(float stageTime)
    {
        timer = stageTime;
        gameActive = true; // 게임 시작
        UIManager.instance.UpdateTimerUI(timer); // 타이머 UI 업데이트
        UIManager.instance.HideGameOver(); // 게임 오버 화면 숨기기
    }

    // 게임 오버 처리
    public void GameOver()
    {
        gameActive = false;
        UIManager.instance.ShowGameOver(score, timer); // 수정: 최고 점수를 인자로 전달하지 않음
        gameBoard.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    // 최고 점수 불러오기
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
        yield return new WaitForSeconds(1.0f); // 잠시 기다린 후 두 카드 비교

        if (firstCard.name == secondCard.name)
        {
            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);
            score += 10;
            AudioManager.instance.PlayQuizShowCorrectSound();
            gameBoard.CheckAllCardsDisabled(); // 모든 카드가 비활성화되었는지 확인
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