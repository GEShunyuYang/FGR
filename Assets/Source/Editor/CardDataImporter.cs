#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CardCsvImporter
{
    private const string CardCsvPath = "Assets/Data/CSV/cards.csv";
    private const string CardOutputDir = "Assets/Data/Cards";
    private const string EffectOutputDir = "Assets/Data/CardEffects";
    private const string ConditionOutputDir = "Assets/Data/CardConditions";

    [MenuItem("FGR/Import Cards From CSV")]
    public static void Import()
    {
        EnsureFolder(CardOutputDir);
        EnsureFolder(EffectOutputDir);
        EnsureFolder(ConditionOutputDir);

        string[] lines = File.ReadAllLines(CardCsvPath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] columns = lines[i].Split(',');

            string id = columns[0].Trim();
            string nameKey = columns[1].Trim();
            int cost = int.Parse(columns[2].Trim());
            string descriptionKey = columns[3].Trim();
            string effectsRaw = columns[4].Trim();
            string conditionsRaw = columns[5].Trim();

            CardData card = LoadOrCreateCard(id);

            SerializedObject so = new SerializedObject(card);

            so.FindProperty("CardId").stringValue = id;
            so.FindProperty("CardNameKey").stringValue = nameKey;
            so.FindProperty("BaseCost").intValue = cost;
            so.FindProperty("DescriptionKey").stringValue = string.IsNullOrWhiteSpace(descriptionKey)
                ? $"card.{id}.desc"
                : descriptionKey;

            SetObjectReferenceList(so.FindProperty("Effects"), ParseEffects(effectsRaw));
            SetObjectReferenceList(so.FindProperty("Conditions"), ParseConditions(conditionsRaw));

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(card);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Card CSV import complete.");
    }
    private static void SetObjectReferenceList<T>(SerializedProperty property, List<T> values)
    where T : UnityEngine.Object
    {
        property.ClearArray();

        for (int i = 0; i < values.Count; i++)
        {
            property.InsertArrayElementAtIndex(i);
            property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }
    }

    private static CardData LoadOrCreateCard(string id)
    {
        string path = $"{CardOutputDir}/{id}.asset";
        CardData card = AssetDatabase.LoadAssetAtPath<CardData>(path);

        if (card == null)
        {
            card = ScriptableObject.CreateInstance<CardData>();
            AssetDatabase.CreateAsset(card, path);
        }

        return card;
    }

    private static List<CardEffect> ParseEffects(string raw)
    {
        List<CardEffect> effects = new();

        foreach (string token in raw.Split('|'))
        {
            if (string.IsNullOrWhiteSpace(token)) continue;

            string[] parts = token.Split(':');
            string type = parts[0].Trim();

            if (type == "Damage")
            {
                float damage = float.Parse(parts[1].Trim());
                effects.Add(GetOrCreateDamageEffect(damage));
            }
            else
            {
                Debug.LogWarning($"Unknown effect: {token}");
            }
        }

        return effects;
    }

    private static List<CardCondition> ParseConditions(string raw)
    {
        List<CardCondition> conditions = new();

        foreach (string token in raw.Split('|'))
        {
            if (string.IsNullOrWhiteSpace(token)) continue;

            string[] parts = token.Split(':');
            string type = parts[0].Trim();

            if (type == "Range")
            {
                int range = int.Parse(parts[1].Trim());
                conditions.Add(GetOrCreateRangeCondition(range));
            }
            else if (type == "EnemyTarget")
            {
                conditions.Add(GetOrCreateEnemyTargetCondition());
            }
            else
            {
                Debug.LogWarning($"Unknown condition: {token}");
            }
        }

        return conditions;
    }

    private static DamageEffect GetOrCreateDamageEffect(float damage)
    {
        string path = $"{EffectOutputDir}/Damage_{damage}.asset";
        DamageEffect effect = AssetDatabase.LoadAssetAtPath<DamageEffect>(path);

        if (effect == null)
        {
            effect = ScriptableObject.CreateInstance<DamageEffect>();
            AssetDatabase.CreateAsset(effect, path);
        }

        SerializedObject so = new SerializedObject(effect);
        so.FindProperty("Damage").floatValue = damage;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(effect);
        return effect;
    }

    private static RangeCondition GetOrCreateRangeCondition(int range)
    {
        string path = $"{ConditionOutputDir}/Range_{range}.asset";
        RangeCondition condition = AssetDatabase.LoadAssetAtPath<RangeCondition>(path);

        if (condition == null)
        {
            condition = ScriptableObject.CreateInstance<RangeCondition>();
            AssetDatabase.CreateAsset(condition, path);
        }

        SerializedObject so = new SerializedObject(condition);
        so.FindProperty("MaxRange").intValue = range;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(condition);
        return condition;
    }

    private static EnemyTargetCondition GetOrCreateEnemyTargetCondition()
    {
        string path = $"{ConditionOutputDir}/EnemyTarget.asset";
        EnemyTargetCondition condition = AssetDatabase.LoadAssetAtPath<EnemyTargetCondition>(path);

        if (condition == null)
        {
            condition = ScriptableObject.CreateInstance<EnemyTargetCondition>();
            AssetDatabase.CreateAsset(condition, path);
        }

        return condition;
    }

    private static void EnsureFolder(string folder)
    {
        if (AssetDatabase.IsValidFolder(folder)) return;

        string parent = Path.GetDirectoryName(folder).Replace("\\", "/");
        string name = Path.GetFileName(folder);

        if (!AssetDatabase.IsValidFolder(parent))
        {
            EnsureFolder(parent);
        }

        AssetDatabase.CreateFolder(parent, name);
    }
}
#endif