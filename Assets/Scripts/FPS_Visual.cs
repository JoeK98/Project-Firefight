using TMPro;
using UnityEngine;

public class FPS_Visual : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate = 72;

    [SerializeField]
    private float updateInterval = 5.0f;

    [SerializeField]
    private TextMeshProUGUI fpsText = null;

    private readonly Color orange = new Color(1.0f, 0.55f, 0.0f);

    private int frames = 0;

    private float timeSinceLastTextUpdate = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        frames++;

        timeSinceLastTextUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastTextUpdate >= updateInterval)
        {
            int fps = Mathf.RoundToInt(frames / timeSinceLastTextUpdate);

            if (fps < targetFrameRate)
            {
                fpsText.color = fps < fps - fps * 0.05 ? Color.red : orange;
            }
            else
            {
                fpsText.color = Color.green;
            }
            
            fpsText.text = fps.ToString();

            timeSinceLastTextUpdate %= updateInterval;
            frames = 0;


        }
    }
}
