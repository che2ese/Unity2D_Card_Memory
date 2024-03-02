using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] 
    private Text timerText; // 타이머 텍스트 UI
    [SerializeField] 
    private Text scoreText; // 점수 텍스트 UI
    [SerializeField] 
    private GameObject gameOverPanel; // 게임 오버 패널
    [SerializeField] 
    private Text finalScoreText;
    [SerializeField]
    private GameObject StageClearPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField] 
    private Text stageText; // 스테이지 번호를 표시할 텍스트
    [SerializeField]
    private Text highScoreText; // 최고 점수 텍스트 UI 추가

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameOverPanel.SetActive(false);// 게임 시작 시 게임 오버 패널을 숨깁니다.
        StageClearPanel.SetActive(false);
    }

    // 타이머 UI 업데이트
    public void UpdateTimerUI(float time)
    {
        timerText.text = $"Timer " + 
            $"{Mathf.RoundToInt(time)}";
    }
    public void SetTimer(float time)
    {
        UpdateTimerUI(time); // 기존 타이머 UI 업데이트 메서드를 호출
    }

    // 점수 UI 업데이트
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Score : {score}";
    }

    // 게임 오버 UI 표시
    public void ShowGameOver(int totalScore, float remainingTime)
    {
        int finalScore = totalScore + Mathf.RoundToInt(remainingTime) * 10;
        finalScoreText.text = $"Final Score {finalScore}";
        gameOverPanel.SetActive(true);
    }

    // 게임 오버 UI 숨기기
    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
    public void ShowStageClearPanel()
    {
        StartCoroutine(ShowStageClearPanelCoroutine());
    }

    private IEnumerator ShowStageClearPanelCoroutine()
    {
        Time.timeScale = 0; // 게임 동작 일시 중지
        StageClearPanel.SetActive(true); // 스테이지 클리어 패널 활성화
        AudioManager.instance.PlayCardShuffleSound(); // 카드 셔플 사운드 재생

        // 실제 시간 기준으로 2초 대기
        yield return new WaitForSecondsRealtime(2.5f);

        StageClearPanel.SetActive(false); // 스테이지 클리어 패널 비활성화
        Time.timeScale = 1; // 게임 동작 재개
    }

    public void OnStartGameClicked()
    {
        // MainPanel 비활성화
        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
        }

        // GameManager를 통해 게임 시작 로직 실행
        if (GameManager.instance != null)
        {
            GameManager.instance.InitializeGame(); // 게임 초기화 및 시작
            Time.timeScale = 1;
            AudioManager.instance.PlayCardShuffleSound(); // 카드 셔플 사운드 재생
        }
    }
    public void UpdateStageText(int currentStage)
    {
        stageText.text = $"Stage {currentStage + 1}"; // 스테이지 인덱스는 0부터 시작하므로 1을 더해 표시
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}

