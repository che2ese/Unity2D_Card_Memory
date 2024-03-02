using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] 
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ
    [SerializeField] 
    private AudioClip cardShuffleClip; // ī�� ���� ���� Ŭ��
    [SerializeField] 
    private AudioClip cardTakeOutPackage1;
    [SerializeField] 
    private AudioClip quizShowCorrectClip;
    [SerializeField] 
    private AudioClip quizShowWrongClip;

    void Awake()
    {
        // �̱��� ������ ����Ͽ� ����� �Ŵ��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� ����
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // ī�� ���� ���� ���
    public void PlayCardShuffleSound()
    {
        if (cardShuffleClip != null && audioSource != null)
        {
            audioSource.clip = cardShuffleClip;
            audioSource.Play();
        }
    }
    public void PlayCardTakeOutSound()
    {
        if (cardTakeOutPackage1 != null && audioSource != null)
        {
            audioSource.PlayOneShot(cardTakeOutPackage1, 0.25f);
        }
    }
    public void PlayQuizShowCorrectSound()
    {
        if (quizShowCorrectClip != null)
        {
            audioSource.PlayOneShot(quizShowCorrectClip);
        }
    }
    public void PlayQuizShowWrongSound()
    {
        if (quizShowWrongClip != null)
        {
            audioSource.PlayOneShot(quizShowWrongClip);
        }
    }
}
