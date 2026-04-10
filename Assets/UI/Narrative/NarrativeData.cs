// NarrativeData.cs
// -------------------------------------------------------------------
// Plain serialisable data classes that mirror the shape of intro_data.json.
// JsonUtility.FromJson<NarrativeWrapper>() in NarrativeController.cs
// depends on these exact field names matching the JSON keys.
// -------------------------------------------------------------------
// JSON shape reminder:
//
// {
//   "slides": [
//     {
//       "id": 0,
//       "imagePath": "IntroImages/approaching_planet",
//       "buttonText": "Continue",
//       "lines": [
//         {
//           "audioPath": "IntroAudio/approaching_planet",
//           "caption": "..."
//         }
//       ]
//     }
//   ]
// }
// -------------------------------------------------------------------

using System;

/// <summary>Top-level wrapper so JsonUtility can deserialise the root object.</summary>
[Serializable]
public class NarrativeWrapper
{
    public NarrativeSlide[] slides;
}

/// <summary>One full slide: a background image, a button label, and one or more spoken lines.</summary>
[Serializable]
public class NarrativeSlide
{
    public int    id;
    public string imagePath;
    public string buttonText;
    public NarrativeLine[] lines;
}

/// <summary>One line of dialogue: an audio clip path and the matching caption string.</summary>
[Serializable]
public class NarrativeLine
{
    public string audioPath;
    public string caption;
}
