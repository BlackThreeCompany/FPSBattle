using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float sfxVolum = 1.0f;
    public float bgmVolum = 1.0f;

    public bool isSfxMute = false;

    AudioSource bgmSource;
    AudioSource sfxSource;

    public AudioClip grenadeBoom;
    public AudioClip SmokegrenadeBoom;
    public AudioClip ThrowGrenade;
    public AudioClip ThrowSmokeGrenade;
    public AudioClip ImpactGrenade;



    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(Vector3 pos,AudioClip sfx,float delayed,float volum)
    {
        if (isSfxMute) return;

        StartCoroutine(PlaySfxIE(pos, sfx, delayed, volum));
    }
    IEnumerator PlaySfxIE(Vector3 pos,AudioClip sfx,float delayed, float volum)
    {
        yield return new WaitForSeconds(delayed);

        GameObject sfxObj = new GameObject("sfx");

        AudioSource _aud = sfxObj.AddComponent<AudioSource>();

        _aud.clip = sfx;
        sfxObj.transform.position = pos;

        _aud.spatialBlend = 1;
        _aud.minDistance = 5.0f;
        _aud.maxDistance = 10.0f;

        _aud.volume = volum;
        _aud.Play();
        Destroy(sfxObj, sfx.length + delayed);
    }

    public void PlayBGM(AudioClip bgm, float delayed, bool loop)
    {
        StartCoroutine(PlayBGMIE(bgm, delayed, loop));
    }

    IEnumerator PlayBGMIE(AudioClip bgm, float delayed, bool loop)
    {
        yield return new WaitForSeconds(delayed);

        GameObject bgmObj = new GameObject("BGM");

        if (!bgmSource) bgmSource = bgmObj.AddComponent<AudioSource>();

        bgmSource.clip = bgm;
        bgmSource.volume = bgmVolum;
        bgmSource.loop = loop;
        bgmSource.Play();

    }
}
