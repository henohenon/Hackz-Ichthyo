using UnityEngine;

/// <summary>
/// 指定された Transform に対して offset を加えた位置に自身を移動させる責任を持つコンポーネント。
/// 意味：空間的な追従と相対位置の維持。
/// 拡張性：offset の動的変更や条件付き追従にも対応可能。
/// </summary>
public sealed class FollowTargetWithOffset : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        // 意味：target の位置に offset を加えた空間的意味位置へ自身を移動
        transform.position = target.position + offset;
    }
}