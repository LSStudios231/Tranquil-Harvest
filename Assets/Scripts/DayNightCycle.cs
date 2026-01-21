using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class DayNightCycle : MonoBehaviour
{
    public TMP_Text dayText; // Reference to the text displaying the day number
    public TMP_Text timeText; // Reference to the text displaying the time
    public float secondsPerInGameMinute = 1f; // How many real-time seconds equal one in-game minute
    public Tilemap environmentTilemap; // Reference to the Tilemap component
    public Color dayColor = Color.white; // Color tint for day
    public Color nightColor = new Color(0.2f, 0.2f, 0.5f); // Color tint for night

    private int dayNumber = 1; // Current day number
    private int currentHour = 6; // Start time hour
    private int currentMinute = 0; // Start time minute

    private float timeCounter = 0f; // Counter for updating in-game time

    private void Start()
    {
        UpdateTimeText();
        UpdateDayText();
        UpdateEnvironmentColor(); // Set initial environment color
    }

    private void Update()
    {
        // Increment the time counter
        timeCounter += Time.deltaTime;

        // Check if a minute has passed in the game
        if (timeCounter >= secondsPerInGameMinute)
        {
            // Increase the in-game minute
            currentMinute++;
            timeCounter = 0f;

            // Check if an hour has passed
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                // Check if a new day has started
                if (currentHour >= 24)
                {
                    currentHour = 0;
                    dayNumber++;
                    UpdateDayText();
                }

                // Update environment color if needed
                UpdateEnvironmentColor();
            }

            UpdateTimeText();
        }
    }

    private void UpdateTimeText()
    {
        // Format the time as "HH:MM"
        string hourString = currentHour.ToString("D2"); // D2 ensures two-digit format
        string minuteString = currentMinute.ToString("D2");
        timeText.text = $"Time: {hourString}:{minuteString}";
    }

    private void UpdateDayText()
    {
        // Update the day number text
        dayText.text = $"Day: {dayNumber}";
    }

    private void UpdateEnvironmentColor()
    {
        // Change environment color based on the current hour
        if (currentHour >= 18 || currentHour < 6)
        {
            environmentTilemap.color = nightColor; // Set to night color
        }
        else
        {
            environmentTilemap.color = dayColor; // Set to day color
        }
    }
}
