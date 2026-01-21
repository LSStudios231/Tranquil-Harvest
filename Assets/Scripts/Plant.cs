using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject[] growthStages; // Array of plant prefabs representing different growth stages
    public float[] growthStageTimers; // Array of timers for each growth stage

    private GameObject currentPlant;

    private void Start()
    {
        // Start the plant growth process
        StartCoroutine(GrowPlant());
    }

    private IEnumerator GrowPlant()
    {
        // The initial plant is the one the script is attached to
        currentPlant = gameObject;

        // Iterate over each growth stage
        for (int i = 0; i < growthStages.Length; i++)
        {
            // Wait for the specified time for this growth stage
            yield return new WaitForSeconds(growthStageTimers[i]);

            // Instantiate the next growth stage prefab
            GameObject newPlant = Instantiate(growthStages[i], transform.position, Quaternion.identity);

            // Destroy the previous growth stage
            if (currentPlant != null && currentPlant != gameObject)
            {
                Destroy(currentPlant);
            }

            // Set the new plant as the current plant
            currentPlant = newPlant;
        }

        // Destroy the initial plant (which has this script) after the first stage
        Destroy(gameObject);

        // Plant has reached full growth
        // The final stage stays without being destroyed
        currentPlant = null; // Optional: Clear reference to indicate growth is complete
    }
}
