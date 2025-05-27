using Firebase.Database;
using Firebase;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

public class FirebaseManager
{
    private FirebaseDatabase dbRef;
    public void Init()
    {
        dbRef = FirebaseDatabase.DefaultInstance;
    }

    public async Task<string> LoadData(string key,string id)
    {
        var snapshot = await dbRef.GetReference(key).Child(id).GetValueAsync();

        if (snapshot.Exists)
        {
            return snapshot.GetRawJsonValue();
        }
        else
        {
            return null;
        }
    }

    public async Task<string> SaveData(string key,string id, object data)
    {
        string json = JsonUtility.ToJson(data);

        try
        {
            await dbRef.GetReference(key).Child(id).SetRawJsonValueAsync(json);
            return "저장 완료";
        }
        catch (System.Exception ex)
        {
            return $"저장 실패: {ex.Message}";
        }
    }
}
