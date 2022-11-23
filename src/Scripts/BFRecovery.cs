using System;
using System.IO;
using UnityEngine;

namespace BalloonFloater.Scripts
{
    public class BFRecovery
    {
        public BFData GetData()
        {
            BFData data = new BFData();
            string dataPath = Application.persistentDataPath + "\\BalloonFloaterData.txt";

            data.playerGain = 0.5f;
            data.playerMaxGain = 2f;
            data.balloonGain = 1.5f;
            data.destroyTime = 0.5f;
            data.movementMode = 0;

            if (File.Exists(dataPath)) data = JsonUtility.FromJson<BFData>(File.ReadAllText(dataPath));
            else SetData(data);

            return data;
        }

        public void SetData(BFData data)
        {
            string dataPath = Application.persistentDataPath + "\\BalloonFloaterData.txt";
            File.WriteAllText(dataPath, JsonUtility.ToJson(data));
        }
    }

    [Serializable]
    public class BFData
    {
        public float playerGain;
        public float playerMaxGain;
        public float balloonGain;
        public float destroyTime;
        public int movementMode;
    }
}
