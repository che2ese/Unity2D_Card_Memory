using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private GameBoard gameBoard; // GameBoard�� ���� ����
    [SerializeField]
    private GameObject background;

    private int currentStage = 0; // ���� �������� �ε���
    private float initialCameraSize = 5f; // ī�޶��� �ʱ� ������
    private int[,] stageSizes = new int[,] { { 2, 2 }, { 4, 2 }, { 6, 3 } }; // �� ���������� ��� �� ũ��
    private float[] stageTimes = { 15f, 30f, 90f }; // �� ���������� ���� �ð�

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

        // ���� ���������� �°� GameBoard �ʱ�ȭ
        int rows = stageSizes[stageIndex, 0];
        int cols = stageSizes[stageIndex, 1];
        gameBoard.InitBoard(rows, cols);

        // ī�޶� ������ ����
        AdjustCameraSize(currentStage);
        BackGroundSize(currentStage);

        // �������� ���� �� ��� ī�带 ���� �ð� ���� �����ֱ�
        StartCoroutine(gameBoard.RevealCardsTemporarily(2f));

        UIManager.instance.SetTimer(stageTimes[currentStage]);

        GameManager.instance.StartStage(stageTimes[currentStage]);
       
        UIManager.instance.UpdateStageText(currentStage);
    }

    // ī�޶� ����� �����ϴ� �޼ҵ�
    private void AdjustCameraSize(int stageIndex)
    {
        // ī�޶� ����� �ʱ� �������� 1.4�辿 stageIndex�� ���� ����
        Camera.main.orthographicSize = initialCameraSize * Mathf.Pow(1.4f, stageIndex);
    }
    private void BackGroundSize(int stageIndex)
    {
        // ��� ����� �ʱ� �������� 1.4�辿 stageIndex�� ���� ����
        background.transform.localScale *= Mathf.Pow(1.4f, stageIndex);
    }
    public bool IsLastStage()
    {
        return currentStage >= stageSizes.GetLength(0) - 1;
    }
    // ���� ���������� �Ѿ�� �޼ҵ�
    public void NextStage()
    {
        if (currentStage + 1 < stageSizes.GetLength(0))
        {
            GameManager.instance.UpdateScore(20);
            SetupStage(++currentStage);
            // �������� Ŭ���� �� ȣ�� ����
            UIManager.instance.ShowStageClearPanel();

        }
        else
        {
            GameManager.instance.UpdateScore(50);
            // ��� �������� �Ϸ�
            UIManager.instance.ShowGameOver(GameManager.instance.score, GameManager.instance.timer);            // ����: ���� ���� UI�� �¸� �޽��� ǥ��
        }
    }
}