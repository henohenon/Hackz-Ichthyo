using Player.Skill;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public sealed class SelectLernSkillGroup
{
    [SerializeField] private SkillBase[] learnSkills_ = new SkillBase[5];

    /// <summary>
    /// learnSkills_ からランダムに3つのスキルを抽出する責任を持つ関数。
    /// 意味：プレイヤーに提示する候補スキルの選定。
    /// 拡張性：抽出数や条件を変更可能。
    /// </summary>
    public SkillBase[] SelectRandomSkills(int count = 3)
    {
        if (learnSkills_ == null || learnSkills_.Length < count)
        {
            Debug.LogWarning("抽出可能なスキルが不足しています。");
            return Array.Empty<SkillBase>();
        }

        return learnSkills_
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(count)
            .ToArray();
    }
}