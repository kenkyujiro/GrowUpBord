using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleContext
{
    public float allyHpRate;
    public float enemyHpRate;
    public string enemyWeakness;
    public List<string> actions;

    public BattleContext(float allyHp, float enemyHp, List<string> actions)
    {
        this.allyHpRate = allyHp;
        this.enemyHpRate = enemyHp;
        this.actions = actions;
    }
}

public class AllyAI
{
    private Dictionary<string, float> qTable = new Dictionary<string, float>();
    private System.Random rng = new System.Random();

    private float epsilon = 0.2f; // 探索率
    private float alpha = 0.1f;   // 学習率

    //キー値の設定
    private string MakeKey(BattleContext ctx, string action)
    {
        return $"{Mathf.RoundToInt(ctx.allyHpRate * 10)}_" +
               $"{Mathf.RoundToInt(ctx.enemyHpRate * 10)}_" +
               $"{ctx.enemyWeakness}_{action}";
    }

    public int ChooseAction(BattleContext ctx)
    {
        if (ctx.actions.Count == 0) return 0;

        if (rng.NextDouble() < epsilon)
        {
            // ランダム探索
            return rng.Next(ctx.actions.Count);
        }

        // Q値(期待値)最大の行動を選択
        float bestQ = float.MinValue;
        int bestIndex = 0;
        for (int i = 0; i < ctx.actions.Count; i++)
        {
            string key = MakeKey(ctx, ctx.actions[i]);
            if (!qTable.ContainsKey(key)) qTable[key] = 0f;

            if (qTable[key] > bestQ)
            {
                bestQ = qTable[key];
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    //報酬の受け取りと再学習
    public void Learn(BattleContext ctx, string action, float reward)
    {
        string key = MakeKey(ctx, action);
        if (!qTable.ContainsKey(key)) qTable[key] = 0f;
        qTable[key] += alpha * (reward - qTable[key]);
    }
}
