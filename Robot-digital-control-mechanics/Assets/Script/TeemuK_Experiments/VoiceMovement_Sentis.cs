using UnityEngine;
using Unity.Sentis;
using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class VoiceMovement_Sentis : MonoBehaviour {
    Worker decoderEngine, encoderEngine, spectroEngine;

    const BackendType backend = BackendType.GPUCompute;

    //Link your audioclipo here. Format must be 16Hz mono non-compressed.
    public AudioClip audioClip;

    //This is how many tokens you want. It can be adjusted.
    const int maxTokens = 100;

    //Special tokens see added tokens file for details
    const int END_OF_TEXT = 50257;
    const int START_OF_TRANSCRIPT = 50258;
    const int ENGLISH = 50259;
    const int GERMAN = 50261;
    const int FRENCH = 50265;
    const int TRANSCRIBE = 50359; //for speech-to-text in specified language
    const int TRANSLATE = 50358;  //for speech-to-text then translate to English
    const int NO_TIME_STAMPS = 50363;
    const int START_TIME = 50364;

    int numSamples;
    float[] data;
    string[] tokens;

    int currentToken = 0;
    int[] outputTokens = new int[maxTokens];

    //Used for special character decoding
    int[] whiteSpaceCharacters = new int[256];

    Tensor<float> encodedAudio;

    bool transcribe = false;
    string outputString = "";

    //Maximum size of audioclip (30s at 16kHz)
    const int maxSamples = 30 * 16000;

    void Start() {
        SetupWhiteSpaceShifts();

        GetTokens();

        Model decoder = ModelLoader.Load(Application.streamingAssetsPath + "/AudioDecoder_Tiny.sentis");

        //Model decoderWithArgMax = Functional.... en tiedä onko tämä tarpeen?

        Model encoder = ModelLoader.Load(Application.streamingAssetsPath + "/AudioEncoder_Tiny.sentis");
        Model spectro = ModelLoader.Load(Application.streamingAssetsPath + "/LogMelSepctro.sentis");

        decoderEngine = new Worker(decoder, backend);
        encoderEngine = new Worker(encoder, backend);
        spectroEngine = new Worker(spectro, backend);

        outputTokens[0] = START_OF_TRANSCRIPT;
        outputTokens[1] = ENGLISH;// GERMAN;//FRENCH;//
        outputTokens[2] = TRANSCRIBE; //TRANSLATE;//
        outputTokens[3] = NO_TIME_STAMPS;// START_TIME;//
        currentToken = 3;

        LoadAudio();
        EncodeAudio();
        transcribe = true;

    }

    void LoadAudio() {
        if (audioClip.frequency != 16000) {
            Debug.Log($"The audio clip should have frequency 16kHz. It has frequency {audioClip.frequency / 1000f}kHz");
            return;
        }

        numSamples = audioClip.samples;

        if (numSamples > maxSamples) {
            Debug.Log($"The AudioClip is too long. It must be less than 30 seconds. This clip is {numSamples / audioClip.frequency} seconds.");
            return;
        }

        data = new float[maxSamples];
        numSamples = maxSamples;
        //We will get a warning here if data.length is larger than audio length but that is OK
        audioClip.GetData(data, 0);
    }

    void GetTokens() {
        var jsonText = File.ReadAllText(Application.streamingAssetsPath + "/vocab.json");
        var vocab = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonText);
        tokens = new string[vocab.Count];
        foreach (var item in vocab) {
            tokens[item.Value] = item.Key;
        }
    }

    void EncodeAudio() {
        using var input = new Tensor<float>(new TensorShape(1, numSamples), data);

        //spectroEngine.Execute(input); original, Execute ei ole olemassa
        spectroEngine.Schedule(input);
        var spectroOutput = spectroEngine.PeekOutput() as Tensor<float>;

        //encoderEngine.Execute(spectroOutput); original
        encoderEngine.Schedule(spectroOutput);
        encodedAudio = encoderEngine.PeekOutput() as Tensor<float>;
    }

    void Update() {
        if (transcribe && currentToken < outputTokens.Length - 1) {
            using var tokensSoFar = new Tensor<int>(new TensorShape(1, outputTokens.Length), outputTokens);

            var inputs = new Dictionary<string, Tensor> {
                {"input_0", tokensSoFar},
                {"input_1", encodedAudio}
            };

            decoderEngine.Schedule(tokensSoFar); //Oletettavasti vanhasta versiosta vastaava Execute
            var tokenPredictions = decoderEngine.PeekOutput() as Tensor<int>;

            int ID = tokenPredictions[currentToken];

            outputTokens[++currentToken] = ID;

            if (ID == END_OF_TEXT) transcribe = false;
            else if (ID >= tokens.Length) outputString += $"(time={(ID - START_TIME) * 0.02f})";
            else outputString += GetUnicodeText(tokens[ID]);

            Debug.Log(outputString);
        }
    }

    // Translates encoded special characters to Unicode
    string GetUnicodeText(string text) {
        var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(ShiftCharacterDown(text));
        return Encoding.UTF8.GetString(bytes);
    }

    string ShiftCharacterDown(string text) {
        string outText = "";
        foreach (char letter in text) {
            outText += ((int)letter <= 256) ? letter :
                (char)whiteSpaceCharacters[(int)(letter - 256)];
        }
        return outText;
    }

    void SetupWhiteSpaceShifts() {
        for (int i = 0, n = 0; i < 256; i++) {
            if (IsWhiteSpace((char)i)) whiteSpaceCharacters[n++] = i;
        }
    }

    bool IsWhiteSpace(char c) {
        return !(('!' <= c && c <= '~') || ('�' <= c && c <= '�') || ('�' <= c && c <= '�'));
    }

    private void OnApplicationQuit() {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void OnDestroy() {
        decoderEngine?.Dispose();
        encoderEngine?.Dispose();
        spectroEngine?.Dispose();
    }
}
