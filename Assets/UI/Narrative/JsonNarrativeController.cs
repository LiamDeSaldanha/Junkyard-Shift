// NarrativeController.cs
// -------------------------------------------------------------------
// Drives the intro narration UI built in NarrativeUI.uxml / .uss.
// Attach to a GameObject that also has a UIDocument component
// pointing at NarrativeUI.uxml.
// -------------------------------------------------------------------
// Inspector fields to fill in Unity:
//   jsonFile      → drag intro_data.json (TextAsset) here
//   narrationSource → drag an AudioSource component here
//   nextSceneName → type the exact Build-Settings scene name to load
//                   after the last slide (e.g. "Tutorial")
// -------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class JsonNarrativeController : MonoBehaviour
{
    [Header("Data")]
    public TextAsset jsonFile;

    [Header("Audio")]
    public AudioSource narrationSource;

    [Header("Scene Transition")]
    public string nextSceneName;

    // ── UI references ──────────────────────────────────────────────
    private VisualElement _background;
    private Label _caption;
    private Button _continueButton;
    private Button _skipButton;

    // ── State ──────────────────────────────────────────────────────
    private NarrativeSlide[] _slides;
    private int _slideIndex = 0;
    private int _lineIndex = 0;

    // ── Unity lifecycle ────────────────────────────────────────────

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Query elements by the names set in NarrativeUI.uxml
        _background = root.Q<VisualElement>("Background");
        _caption = root.Q<Label>("CaptionText");
        _continueButton = root.Q<Button>("ContinueButton");
        _skipButton = root.Q<Button>("SkipButton");

        // Wire up buttons
        _continueButton.clicked += OnNextClicked;
        _skipButton.clicked += OnSkipClicked;

        LoadJsonData();
        UpdateUI();
    }

    void OnDisable()
    {
        // Always unsubscribe to avoid memory leaks when the scene reloads
        if (_continueButton != null) _continueButton.clicked -= OnNextClicked;
        if (_skipButton != null) _skipButton.clicked -= OnSkipClicked;
    }

    // ── Button handlers ────────────────────────────────────────────

    /// <summary>Advance one line; if all lines exhausted, advance one slide.</summary>
    private void OnNextClicked()
    {
        _lineIndex++;

        // Exhausted all lines in the current slide → move to next slide
        if (_lineIndex >= _slides[_slideIndex].lines.Length)
        {
            _slideIndex++;
            _lineIndex = 0;

            // Exhausted all slides → load the next scene
            if (_slideIndex >= _slides.Length)
            {
                LoadNextScene();
                return;
            }
        }

        UpdateUI();
    }

    /// <summary>Skip directly to the next scene, bypassing remaining slides.</summary>
    private void OnSkipClicked()
    {
        // Stop any playing narration audio before jumping away
        if (narrationSource != null && narrationSource.isPlaying)
            narrationSource.Stop();

        LoadNextScene();
    }

    // ── UI update ──────────────────────────────────────────────────

    private void UpdateUI()
    {
        NarrativeSlide currentSlide = _slides[_slideIndex];
        NarrativeLine currentLine = currentSlide.lines[_lineIndex];

        // ── Background image ──────────────────────────────────────
        // Only reload on the first line of each slide (perf).
        // Load as Texture2D — works with any Texture Type import setting,
        // unlike Resources.Load<Sprite> which requires "Sprite (2D and UI)".
        if (_lineIndex == 0)
        {
            Texture2D bg = Resources.Load<Texture2D>(currentSlide.imagePath);
            if (bg != null)
                _background.style.backgroundImage = new StyleBackground(bg);
            else
                Debug.LogWarning($"[NarrativeController] Texture not found at: {currentSlide.imagePath}");
        }

        // ── Button text ───────────────────────────────────────────
        // Show the slide's custom buttonText (e.g. "Start Tutorial") only on
        // the very last line of that slide; otherwise use a plain "Continue ›"
        bool isLastLine = (_lineIndex == currentSlide.lines.Length - 1);
        _continueButton.text = isLastLine ? currentSlide.buttonText + "  ›" : "Continue  ›";

        // ── Caption ───────────────────────────────────────────────
        _caption.text = currentLine.caption;

        // ── Audio ─────────────────────────────────────────────────
        // Strip any file extension from the path before calling Resources.Load —
        // it does not accept extensions and returns null silently if one is present.
        string audioPath = System.IO.Path.ChangeExtension(currentLine.audioPath, null);
        AudioClip clip = Resources.Load<AudioClip>(audioPath);
        if (clip != null)
        {
            narrationSource.Stop();
            narrationSource.clip = clip;
            narrationSource.Play();
        }
        else
        {
            Debug.LogWarning($"[NarrativeController] AudioClip not found at: {audioPath}");
        }
    }

    // ── Data loading ───────────────────────────────────────────────

    private void LoadJsonData()
    {
        if (jsonFile == null)
        {
            Debug.LogError("[NarrativeController] jsonFile is not assigned in the Inspector!");
            return;
        }

        NarrativeWrapper wrapper = JsonUtility.FromJson<NarrativeWrapper>(jsonFile.text);
        _slides = wrapper.slides;
    }

    // ── Scene transition ───────────────────────────────────────────

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("[NarrativeController] nextSceneName is empty — set it in the Inspector.");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}