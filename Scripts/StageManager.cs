using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private GameBoard gameBoard; // GameBoard에 대한 참조
    [SerializeField]
    private GameObject background;

    private int currentStage = 0; // 현재 스테이지 인덱스
    private float initialCameraSize = 5f; // 카메라의 초기 사이즈
    private int[,] stageSizes = new int[,] { { 2, 2 }, { 4, 2 }, { 6, 3 } }; // 각 스테이지별 행과 열 크기
    private float[] stageTimes = { 15f, 30f, 90f }; // 각 스테이지별 제한 시간

    void Start()
    {
        SetupStage(currentStage);
    }

    public void SetupStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageSizes.GetLength(0))
        {
            Debug.LogError("Invalid stage index");
            return;
        }

        currentStage = stageIndex;

        // 현재 스테이지에 맞게 GameBoard 초기화
        int rows = stageSizes[stageIndex, 0];
        int cols = stageSizes[stageIndex, 1];
        gameBoard.InitBoard(rows, cols);

        // 카메라 사이즈 조정
        AdjustCameraSize(currentStage);
        BackGroundSize(currentStage);

        // 스테이지 설정 후 모든 카드를 일정 시간 동안 보여주기
        StartCoroutine(gameBoard.RevealCardsTemporarily(2f));

        UIManager.instance.SetTimer(stageTimes[currentStage]);

        GameManager.instance.StartStage(stageTimes[currentStage]);
       
        UIManager.instance.UpdateStageText(currentStage);
    }

    // 카메라 사이즈를 조정하는 메소드
    private void AdjustCameraSize(int stageIndex)
    {
        // 카메라 사이즈를 초기 사이즈의 1.4배씩 stageIndex에 따라 조정
        Camera.main.orthographicSize = initialCameraSize * Mathf.Pow(1.4f, stageIndex);
    }
    private void BackGroundSize(int stageIndex)
    {
        // 배경 사이즈를 초기 사이즈의 1.4배씩 stageIndex에 따라 조정
        background.transform.localScale *= Mathf.Pow(1.4f, stageIndex);
    }
    public bool IsLastStage()
    {
        return currentStage >= stageSizes.GetLength(0) - 1;
    }
    // 다음 스테이지로 넘어가는 메소드
    public void NextStage()
    {
        if (currentStage + 1 < stageSizes.GetLength(0))
        {
            GameManager.instance.UpdateScore(20);
            SetupStage(++currentStage);
            // 스테이지 클리어 시 호출 예시
            UIManager.instance.ShowStageClearPanel();

        }
        else
        {
            GameManager.instance.UpdateScore(50);
            // 모든 스테이지 완료
            UIManager.instance.ShowGameOver(GameManager.instance.score, GameManager.instance.timer);            // 가정: 게임 오버 UI에 승리 메시지 표시
        }
    }
}