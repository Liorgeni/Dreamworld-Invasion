using System.Collections;
using UnityEngine;
using TMPro; // Be sure to import this if you're using TextMeshPro
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshPro component
    public TextMeshProUGUI skipText; // Reference to your TextMeshProUGUI component (or use Text if it's a regular UI Text)

    public float typingSpeed = 0.05f; // Speed of the typewriter effect
    public string nextSceneName = "Game"; // Name of the next scene to load
    private string fullStory;
    public float fadeSpeed = 1f;      // Speed of fade effect
    private bool isFadingOut = true;  // Start by fading out

    void Start()
    {
        fullStory = textComponent.text; // Store the full text
        textComponent.text = ""; // Clear the text to start typing
        StartCoroutine(TypeText());


        if (skipText == null)
            skipText = GetComponent<TextMeshProUGUI>(); // Get the TextMeshPro component if not set
        StartCoroutine(FadeText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // You can change the key if needed
        {
            StopAllCoroutines(); // Stop typing effect
            textComponent.text = fullStory; // Show full text immediately
            SceneManager.LoadScene(nextSceneName); // Transition to the game scene
        }
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullStory.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for a few seconds before transitioning to the next scene
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName);
    }


    IEnumerator FadeText()
    {
        while (true) // Continuous loop for fading
        {
            float targetAlpha = isFadingOut ? 0f : 1f; // Fade to 0 (transparent) or 1 (fully visible)
            float currentAlpha = skipText.alpha;

            // Smoothly interpolate alpha value
            while (!Mathf.Approximately(currentAlpha, targetAlpha))
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
                skipText.alpha = currentAlpha; // Apply the alpha value to the text
                yield return null;
            }

            // Toggle fade direction
            isFadingOut = !isFadingOut;

            // Optionally, you can add a small delay between fades
            yield return new WaitForSeconds(0.5f);
        }
    }
}
