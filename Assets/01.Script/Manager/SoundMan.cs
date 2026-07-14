using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource loopSource;
    public AudioSource shootSource;

    [Header("BGM")]
    public AudioClip[] stageBGM;
    public AudioClip bossBGM;

    [Header("SFX")]
    public AudioClip shoot;
    public AudioClip ready;
    public AudioClip error;
    public AudioClip respawn;
    public AudioClip playerHit;
    public AudioClip playerDead;
    public AudioClip[] enemyBoom;
    public AudioClip[] enemyBullet;

    [Header("Loop SFX")]
    public AudioClip onUI;
    
    public AudioSource boostSource;
    
    public float fadeTime = 1.0f; // 부드럽게 바뀌는 시간 (1초)
    public float maxVolume = 0.5f; // BGM 기본 볼륨
    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    public void Boost()
    {
        if (!boostSource.isPlaying)
        {
            boostSource.Play();
        }
    }

    public void StopBoost()
    {
        if (boostSource.isPlaying)
        {
            boostSource.Stop();
        }
    }
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;

        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        if (sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }
    public void PlayShoot(AudioClip clip)
    {
        if (clip == null) return;
        if (shootSource == null) return;

        shootSource.PlayOneShot(clip);
    }
    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;
        if (loopSource == null) return;

        if (loopSource.clip == clip && loopSource.isPlaying)
            return;

        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        if (loopSource == null) return;

        loopSource.Stop();
        loopSource.clip = null;
        loopSource.loop = false;
    }

    public void PlayStageBGM(int stageIndex)
    {
        // 배열 범위를 벗어나는 예외 처리 (방어 코드)
        if (stageIndex < 0 || stageIndex >= stageBGM.Length)
        {
            Debug.LogWarning($"[BGM] {stageIndex}번 스테이지 음악이 배열에 없습니다!");
            return;
        }

        // 재생할 타겟 음악 가져오기
        AudioClip nextBGM = stageBGM[stageIndex];

        // 현재 똑같은 음악이 재생 중이라면 페이드를 넘깁니다.
        if (bgmSource.clip == nextBGM && bgmSource.isPlaying) return;

        StopAllCoroutines(); 
        StartCoroutine(BGMSequence(nextBGM));
    }
    private IEnumerator BGMSequence(AudioClip nextBGM)
    {
        float t = 0f;

        // 1. 현재 재생 중인 이전 BGM 볼륨 줄이기
        float startVolume = bgmSource.volume;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        // 2. 새로운 스테이지 음악으로 교체 후 재생 시작
        bgmSource.clip = nextBGM;
        bgmSource.volume = 0f;
        bgmSource.Play();

        // 3. 새 BGM 볼륨 부드럽게 키우기
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, maxVolume, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = maxVolume;
    }
    public void EnemyBoom()
    {
        PlaySFX(enemyBoom[Random.Range(0,enemyBoom.Length)]);
    }
    public void EnemyBullet()
    {
        
        PlaySFX(enemyBullet[Random.Range(0,enemyBullet.Length)]);
    }
    public void PlayShoot()
    {
        
        PlayShoot(shoot);
    }


 
}