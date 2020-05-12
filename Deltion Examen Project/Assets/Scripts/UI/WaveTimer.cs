using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveTimer : MonoBehaviour
{
    public TextMeshProUGUI wave;
    public TextMeshProUGUI waveTimer;

    private float curentWaveTime;

    private void Update()
    {
        if(curentWaveTime >= 0)
        {
            int minutes = (int)curentWaveTime / 60;
            int seconds = (int)curentWaveTime - (minutes * 60);

            //waveTimer.text = minutes.ToString() + ":" + seconds.ToString();

            waveTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            curentWaveTime -= Time.deltaTime;
        }
    }

    public void SetTimerValues(float time, int waveNumber)
    {
        wave.text = waveNumber.ToString();

        curentWaveTime = time;
    }
}
