using UnityEngine;
using UnityEngine.Audio;


[RequireComponent (typeof (AudioSource))]
public class AudioVisualManager : MonoBehaviour
{

    public static float[] Band { get; private set; }
    public static AudioSource AV_Source { get; private set; }

    [SerializeField] private bool useMicrophone;
    [SerializeField] private AudioMixerGroup musicMixer, microMixer; 

    [SerializeField] private float amplifier = 100f;
    [SerializeField] [Range(0.001f, 0.01f)] private float bufferCorrectionUp = 0.005f;
    [SerializeField] [Range(1f, 1.5f)] private float bufferCorrectionDown = 1.2f;


    private const int QUALITY = 512;
    private const int BANDS = 8;


    private float[] frequencyBands;
    private float[] samples; 
    private float[] buffer;


    private AudioClip music;
 


    private void Awake()
    {
        samples = new float[QUALITY];
        Band = new float  [BANDS];
        frequencyBands = new float[BANDS];
        buffer = new float[BANDS];

        AV_Source = GetComponent<AudioSource>();
        AV_Source.Stop();
      
    }

    private void Start()
    {
        //for (int i = 0; i < Microphone.devices.Length; i++)
            //Debug.Log(Microphone.devices[i]);

        music = AV_Source.clip;
        

        if(useMicrophone && Microphone.devices.Length > 0)
        {
            AV_Source.outputAudioMixerGroup = microMixer;
            AV_Source.clip = Microphone.Start(Microphone.devices[0], true, 10, AudioSettings.outputSampleRate);
            while((Microphone.GetPosition(Microphone.devices[0]) > 0) == false) { }
        }
        else
        {
            AV_Source.outputAudioMixerGroup = musicMixer;
            AV_Source.clip = music;
        }

        AV_Source.Play();

    }



    private void Update()
    {
            AV_Source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
            AudioBuffer(FrequencyBandsSetup());
    }


    private float [] FrequencyBandsSetup()
    {
        int count = 0;

        for (int i = 0; i < BANDS; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

           
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] ;
                count++;
            }

            average /= count;
            frequencyBands[i] = average * amplifier;
        }

        return frequencyBands;
    }



    private void AudioBuffer(float [] frequencies)
    {
        for (int i = 0; i < BANDS; i++)
        {
            if (frequencies[i] > Band[i])
            {
                Band[i] = frequencies[i];
                buffer[i] = bufferCorrectionUp;
            }

            if (frequencies[i] < Band[i])
            {
                Band[i] -= buffer[i];
                buffer[i] *= bufferCorrectionDown;
            }
        }
    }


}
