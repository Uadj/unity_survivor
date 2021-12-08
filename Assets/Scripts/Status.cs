using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Status : MonoBehaviour
{
    [SerializeField]
    private int sp;
    private int currentSp;

    [SerializeField]
    private int hp;
    private int currentHp;

    [SerializeField]
    private int spIncreaseSpeed;

    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    private bool spUsed;

    [SerializeField]
    private int dp;
    private int currentDp;

    [SerializeField]
    private int hungry;
    private int currentHungry;

    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentthirstyDecreaseTime;

    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    [SerializeField]
    private Image[] images_Gauges;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentSatisfy = satisfy;
        currentThirsty = thirsty;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        GagueUpdate();
        SPRecover();
    }
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }

        }
        else
        {
            Debug.Log("배고픔0");
        }
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentthirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentthirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentthirstyDecreaseTime = 0;
            }

        }
        else
        {
            Debug.Log("목마름0");
        }
    }
    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }
    private void GagueUpdate()
    {
        images_Gauges[HP].fillAmount = (float)currentHp / hp;
        images_Gauges[SP].fillAmount = (float)currentSp / sp;
        images_Gauges[DP].fillAmount = (float)currentDp / dp;
        images_Gauges[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauges[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauges[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;
        if (currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }
        spUsed = false;
    }
}
