using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public List<TextAsset> jsonFile;
    private Recipe[] jData;
    public int step = 0;
    ObjectController objCtrl;

    public GameObject notice;
    public TextMesh noticeText;
    public GameObject gauge;
    public TextMesh time;
    public List<GameObject> noticeBtn = new List<GameObject>();

    private bool isClick = false;
    private float clickTime = 0;
    private float triggerTime = 2.0f;

    bool getReady = false;
    string videoPath;
    string recipeName;
    int recipeCode = 8;
    bool isFirst = true;

    AudioSource _audio;

    private void Start()
    {
        recipeCode = GameObject.Find("DataSave").GetComponent<RecipeCodeSaver>().getCode();

        objCtrl = GetComponent<ObjectController>();
        _audio = GetComponent<AudioSource>();

        LoadJsonData_FromAsset(jsonFile[recipeCode]);
       // Ready();
    }

    private void Update()
    {
        time.text = clickTime.ToString();
    }

    void LoadJsonData_FromAsset(TextAsset pAsset)
    {
        if (pAsset == null)
        {
            Debug.LogError("파일 없음");
            return;
        }

        LoadRecipe(pAsset.text);
    }

    public void Ready()
    {
        if (isFirst)
        {
            notice.SetActive(true);
            notice.GetComponent<Animator>().SetBool("isClick", true);

            //narration
            string nPath = "narration/" + recipeCode + "/popup_start";
            _audio.clip = Resources.Load<AudioClip>(nPath);
            _audio.Play();

            noticeText.text = "잠시 후 " + GetRecipeName(recipeCode) + "의 \n step by step 가이드가 시작됩니다.";

            noticeBtn[0].SetActive(false); //notice '예' 버튼 비활성화
            noticeBtn[1].SetActive(false); //notice '아니오' 버튼 비활성화
            noticeBtn[2].SetActive(true);
            noticeBtn[3].SetActive(false);
            noticeBtn[4].SetActive(false);

            step = 0;

            StartCoroutine(GetStart());

            isFirst = false;
        }

    }

    IEnumerator GetStart()
    {
        noticeBtn[2].SetActive(true);

        for(int i = 10; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            noticeBtn[2].GetComponent<TextMesh>().text = i.ToString();
        }

        yield return new WaitForSeconds(1);
        Debug.Log("getStart()");
        ToNext();
    }

    string GetRecipeName(int recipeNum)
    {
        switch (recipeNum)
        {
            case 0:
                return "티라미수";
            case 1:
                return "아이스박스 케이크";
            case 2:
                return "스모어딥";
            case 3:
                return "바크초콜릿";
            case 4:
                return "퐁당오쇼콜라";
            case 5:
                return "딸기모찌";
            case 6:
                return "구름빵";
            case 7:
                return "커스터드푸딩";
            case 8:
                return "초코칩 쿠키";
            default:
                return "[error]";
        }
    }

    void LoadRecipe(string jsonData) 
    {
        jsonData = "{\"Items\":" + jsonData + "}";
        jData = JsonHelper.FromJson<Recipe>(jsonData);
    }

    public void ToNext()
    {
        step++;
       if(objCtrl != null && jData.Length + 1 > step)
        {
            objCtrl.LoadStep(jData.Length + 1, step, jData[step - 1].content, jData[step - 1].picture, jData[step - 1].guide, jData[step - 1].object3D, jData[step - 1].timer, jData[step - 1].popup, jData[step - 1].tip);

            //objCtrl.LoadStep(testArray.GetLength(0), step, testArray[step-1].content, testArray[step-1].picture, testArray[step-1].guide, testArray[step-1].object3D, testArray[step-1].timer, testArray[step-1].popup, testArray[step-1].tip);
        }
        else
        {
            PlayerPrefs.SetInt(recipeCode + "_end", 1);
        }
        

    }

    public void ToPrev()
    {
        step--;
        objCtrl.LoadStep(jData.Length, step, jData[step-1].content, jData[step-1].picture, jData[step-1].guide, jData[step-1].object3D, jData[step-1].timer, jData[step - 1].popup, jData[step - 1].tip);
    }

    public void ExitGuide() 
    {
        notice.SetActive(true);
        notice.GetComponent<Animator>().SetBool("isClick", true);
        noticeText.text = "튜토리얼을 종료할까요?";
        noticeBtn[0].SetActive(true);
        noticeBtn[1].SetActive(true);
        noticeBtn[2].SetActive(false);
        noticeBtn[3].SetActive(false);
        noticeBtn[4].SetActive(false);
    }

    public void ExitYes()
    {
        isClick = true;
        gauge.SetActive(true);
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        while (isClick)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= triggerTime)
            {
                SceneManager.LoadScene("ListScene");
            }
            yield return null;
        }
    }

    public void ExitYesOff() // 버튼에서 커서 떼면
    {
        gauge.SetActive(false);
        isClick = false;
        clickTime = 0;
    }

    public void ExitNo()
    {
        notice.GetComponent<Animator>().SetBool("isClick", false);
        //notice.SetActive(false);
    }


}

[System.Serializable]
public class Recipe
{
    public string recipeCode;
    public int step;
    public string content;
    public string picture;
    public string timer;
    public string guide;
    public string object3D;
    public float popup;
    public string tip;
}

