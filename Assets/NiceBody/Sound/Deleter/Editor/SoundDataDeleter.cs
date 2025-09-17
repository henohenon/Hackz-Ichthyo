using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using GameCore.Asset;

namespace EditorExtension.Deleter
{
    public class SoundDataDeleter : AssetModificationProcessor
    {
        private static List<string> pendingAudioClipDeletions = new();

        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            var soundData = AssetDatabase.LoadAssetAtPath<SoundData>(assetPath);
            if (soundData == null || soundData.AudioClip == null)
                return AssetDeleteResult.DidNotDelete;

            string clipPath = AssetDatabase.GetAssetPath(soundData.AudioClip);
            if (!string.IsNullOrEmpty(clipPath) && AssetDatabase.IsMainAsset(soundData.AudioClip))
            {
                pendingAudioClipDeletions.Add(clipPath);
            }

            return AssetDeleteResult.DidNotDelete;
        }

        [InitializeOnLoadMethod]
        static void SetupDeletionCallback()
        {
            EditorApplication.projectChanged += TryDeletePendingClips;
        }

        static void TryDeletePendingClips()
        {
            if (pendingAudioClipDeletions.Count == 0) return;

            foreach (var audioClipFilePath in pendingAudioClipDeletions)
            {
                if (AssetDatabase.DeleteAsset(audioClipFilePath))
                {
                    Debug.Log("<color=green>関連するaudioClipを破棄しました: " + audioClipFilePath + "</color>");
                }
            }

            pendingAudioClipDeletions.Clear();
        }
    }
}