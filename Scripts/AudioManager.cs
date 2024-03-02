using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] 
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    [SerializeField] 
    private AudioClip cardShuffleClip; // 카드 셔플 사운드 클립
    [SerializeField] 
    private AudioClip cardTakeOutPackage1;
    [SerializeField] 
    private AudioClip quizShowCorrectClip;
    [SerializeField] 
    private AudioClip quizShowWrongClip;

    void Awake()
    {
        // 싱글턴 패턴을 사용하여 오디오 매니저 인스턴스 관리
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않음
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 카드 셔플 사운드 재생
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
