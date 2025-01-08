using RDG;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    [Serializable]
    public class Vibrate
    {
        public string key;

        [Serializable]
        public class Iteration
        {
            public int delay = 50;
            public int duration = 100;
            public int amplitude = 50;
        }
        public Iteration[] vibration;
    }

    [SerializeField] private Vibrate[] vibrates;

#if UNITY_EDITOR
    [Space]
    [SerializeField] private AudioSource vibrSound;
#endif

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (vibrSound != null)
            vibrSound.Play();

        vibrSound.volume = 0f;
#endif
    }

    public void Play(string key)
    {
        foreach (var v in vibrates)
            if (key == v.key)
            {
                if (v.vibration.Length == 0) break;

                StartCoroutine(Vibrating(v.vibration));

                break;
            }

        IEnumerator Vibrating(Vibrate.Iteration[] vibration)
        {
            foreach (var v in vibration)
            {
                yield return new WaitForSeconds(v.delay * 0.001f);

                Vibration.Vibrate(v.duration, v.amplitude);
                //Debug.Log("Vibration("+v.duration+", "+v.amplitude+")");//

#if UNITY_EDITOR
                if (vibrSound != null)
                {
                    vibrSound.volume = Mathf.Clamp01(1f * v.amplitude / 300f);

                    StartCoroutine(Mute(v.duration * 0.001f));
                    
                    IEnumerator Mute(float delay)
                    {
                        yield return new WaitForSeconds(delay);

                        vibrSound.volume = 0f;
                    }
                }
#endif
            }
        }
    }
}
