using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] 
    private Text timerText; // Ÿ�̸� �ؽ�Ʈ UI
    [SerializeField] 
    private Text scoreText; // ���� �ؽ�Ʈ UI
    [SerializeField] 
    private GameObject gameOverPanel; // ���� ���� �г�
    [SerializeField] 
    private Text finalScoreText;
    [SerializeField]
    private GameObject StageClearPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField] 
    private Text stageText; // �������� ��ȣ�� ǥ���� �ؽ�Ʈ
    [SerializeField]
    private Text highScoreText; // �ְ� ���� �ؽ�Ʈ UI �߰�

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
        gameOverPanel.SetActive(false);// ���� ���� �� ���� ���� �г��� ����ϴ�.
        StageClearPanel.SetActive(false);
    }

    // Ÿ�̸� UI ������Ʈ
    public void UpdateTimerUI(float time)
    {
        timerText.text = $"Timer " + 
            $"{Mathf.RoundToInt(time)}";
    }
    public void SetTimer(float time)
    {
        UpdateTimerUI(time); // ���� Ÿ�̸� UI ������Ʈ �޼��带 ȣ��
    }

    // ���� UI ������Ʈ
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Score : {score}";
    }

    // ���� ���� UI ǥ��
    public void ShowGameOver(int totalScore, float remainingTime)
    {
        int finalScore = totalScore + Mathf.RoundToInt(remainingTime) * 10;
        finalScoreText.text = $"Final Score {finalScore}";
        gameOverPanel.SetActive(true);
    }

    // ���� ���� UI �����
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
        Time.timeScale = 0; // ���� ���� �Ͻ� ����
        StageClearPanel.SetActive(true); // �������� Ŭ���� �г� Ȱ��ȭ
        AudioManager.instance.PlayCardShuffleSound(); // ī�� ���� ���� ���

        // ���� �ð� �������� 2�� ���
        yield return new WaitForSecondsRealtime(2.5f);

        StageClearPanel.SetActive(false); // �������� Ŭ���� �г� ��Ȱ��ȭ
        Time.timeScale = 1; // ���� ���� �簳
    }

    public void OnStartGameClicked()
    {
        // MainPanel ��Ȱ��ȭ
        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
        }

        // GameManager�� ���� ���� ���� ���� ����
        if (GameManager.instance != null)
        {
            GameManager.instance.InitializeGame(); // ���� �ʱ�ȭ �� ����
            Time.timeScale = 1;
            AudioManager.instance.PlayCardShuffleSound(); // ī�� ���� ���� ���
        }
    }
    public void UpdateStageText(int currentStage)
    {
        stageText.text = $"Stage {currentStage + 1}"; // �������� �ε����� 0���� �����ϹǷ� 1�� ���� ǥ��
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}

