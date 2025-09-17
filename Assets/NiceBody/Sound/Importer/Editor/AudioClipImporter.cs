using UnityEditor;
using UnityEngine;
using System.IO;
using GameCore.Asset;

namespace EditorExtension.Importer
{
    public class AudioClipImporter : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___)
        {
            string[] audioPaths = System.Array.FindAll(importedAssets, path =>
                path.EndsWith(".wav") || path.EndsWith(".mp3"));

            if (audioPaths.Length == 0)
                return;

            EditorApplication.delayCall += () =>
            {
                for (int i = 0; i < audioPaths.Length; i++)
                {
                    string importAssetPath = audioPaths[i];
                    float progress = (float)i / audioPaths.Length;

                    EditorUtility.DisplayProgressBar("Audio Importing", $"進行具合: {Path.GetFileName(importAssetPath)}", progress);

                    var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(importAssetPath);
                    if (audioClip == null)
                        continue;

                    string directoryFilePath = Path.GetDirectoryName(importAssetPath);
                    string audioSourceFileName = Path.GetFileNameWithoutExtension(importAssetPath);
                    string soundDataPath = Path.Combine(directoryFilePath, $"{audioSourceFileName}_SoundData.asset");

                    if (File.Exists(soundDataPath))
                        continue;

                    var soundData = ScriptableObject.CreateInstance<SoundData>();
                    var serialized = new SerializedObject(soundData);
                    serialized.FindProperty("audioClip").objectReferenceValue = audioClip;
                    serialized.ApplyModifiedProperties();

                    AssetDatabase.CreateAsset(soundData, soundDataPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    audioClip.hideFlags = HideFlags.HideInHierarchy;
                    EditorUtility.SetDirty(audioClip);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorUtility.ClearProgressBar();
            };
        }
    }
}