using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; //障害物オブジェクト
    public float spawnInterval = 2f;  //生成間隔
    public float spawnHeight = 3f;    //生成の高さ上限

    void Start()
    {
        //spawnIntervalの秒数ごとに生成処理(SpawnObstacle)を行う
        //InvokeRepeating(関数名, n秒後, n秒間隔)
        InvokeRepeating("SpawnObstacle", 1f, spawnInterval);
    }

    void SpawnObstacle()
    {
        //spawnHeightから-spawnHeightの範囲で数値を出力
        float randomY = Random.Range(-spawnHeight, spawnHeight);
        //座標の設定
        Vector3 spawnPosition = new Vector3(10f, randomY, 0f);
        //オブジェクトのインスタンス化
        //Quaternion.identityとは回転なし(常に同じ角度)ということ
        //角度の指定を行いたいときはQuaternion.Euler(0, 0, 45)や
        //float randomY = Random.Range(0f, 360f);Quaternion.Euler(0, randomY, 0)とする
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
