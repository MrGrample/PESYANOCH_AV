using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource _audioSource;
    [HideInInspector] public float[] _samplesLeft, _samplesRight;

    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isMicrophone;
    [SerializeField] private string _selectedMicrophone;
    [SerializeField] private AudioMixerGroup _mixerGroupMicrophone, _mixerGroupMaster;

    [HideInInspector] public float[] _freqBands, _bandBuffer;
    private float[] _bufferDescrease = new float[8];

    private float[] _freqBandHighest = new float[8];
    [HideInInspector] public float[] _audioBand, _audioBandBuffer;

    [HideInInspector] public float _Amplitude, _AmplitudeBuffer;
    private float _AmplitudeHighest;

    [SerializeField] private float _audioProfile;

    public enum _channel { Stereo, Left, Right};
    public _channel channel = new _channel();

    void Start()
    {
        _samplesLeft = new float[512];
        _samplesRight = new float[512];
        _audioBand = new float[8];
        _audioBandBuffer = new float[8];
        _freqBands = new float[8];
        _bandBuffer = new float[8];

        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);

        if (_isMicrophone && Microphone.devices.Length > 0)
        {
                _selectedMicrophone = Microphone.devices[0].ToString();
                Debug.Log(_selectedMicrophone);

                _audioSource.outputAudioMixerGroup = _mixerGroupMicrophone;
                _audioSource.clip = Microphone.Start(_selectedMicrophone, true, 10, AudioSettings.outputSampleRate);

                while ((Microphone.GetPosition(Microphone.devices[0]) > 0) == false) { }
        }
        else
        {
            _audioSource.outputAudioMixerGroup = _mixerGroupMaster;
            _audioSource.clip = _audioClip;
        }

        _audioSource.Play();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    public int GetMaximumAmplitudeBandIndex()
    {

        int _maxIndex = 0;
        float _maximum = -1;

        for (int i = 0; i < 8; i++)
        {
            if (_audioBand[i] >= _maximum)
            {
                _maximum = _audioBand[i];
                _maxIndex = i;
            }
        }

        return _maxIndex;
    }

    public int GetMaximumAmplitudeBandIndexBuffer()
    {

        int _maxIndex = 0;
        float _maximum = -1;

        for (int i = 0; i < 8; i++)
        {
            if (_audioBandBuffer[i] >= _maximum)
            {
                _maximum = _audioBandBuffer[i];
                _maxIndex = i;
            }
        }

        return _maxIndex;
    }

    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i<8; i++)
        {
            if (_freqBands[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBands[i];
            }
            if (_freqBandHighest[i] != 0)
            {
                _audioBand[i] = (_freqBands[i] / _freqBandHighest[i]);
                _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
            }
        }
    }

    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_CurrentAmplitude > _AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }
        if (_AmplitudeHighest != 0)
        {
            _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
            _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (_freqBands[i] >= _bandBuffer[i])
            {
                _bandBuffer[i] = _freqBands[i];
                _bufferDescrease[i] = 0.001f;
            }
            else
            {
                //                _bandBuffer[i] -= _bufferDescrease[i];
                //                _bufferDescrease[i] *= 1.2f;
                _bufferDescrease[i] = (_bandBuffer[i] - _freqBands[i]) / 8;
                _bandBuffer[i] -= _bufferDescrease[i];
            }

            //bandBuffer[g] = bandBuffer[g] - bandBuffer[g]/8 + freqBand[g]/8
        }
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {

            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if (channel == _channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }
                count++;
            }

            average /= count;

            _freqBands[i] = average * 10;

        }
    }
}
