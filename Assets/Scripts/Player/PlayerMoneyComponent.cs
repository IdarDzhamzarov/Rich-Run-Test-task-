using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMoneyComponent: MonoBehaviour
    {
        [SerializeField] private int initialMoney = 40;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI totalMoneyText;
        [SerializeField] private Image moneyBar;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI plusMoney;
        [SerializeField] private TextMeshProUGUI minusMoney;
        [SerializeField] private float accumulationResetTime = 1f;

        [SerializeField] private List<StatusRequirement> statusRequirements;

        private int money;
        private int totalMoney;
        private int moneyForMaxStatus;
        private PlayerStatus playerStatus;

        private int accumulatedPlusMoney;
        private int accumulatedMinusMoney;

        private float lastPlusTime;
        private float lastMinusTime;

        private Animator minusAnim;
        private Animator plusAnim;

        private static readonly int ResetTrigger = Animator.StringToHash("reset");

        private void Start()
        {
            statusRequirements.Sort((a, b) => Math.Sign(a.money - b.money));

            moneyForMaxStatus = statusRequirements[^1].money;
            plusAnim = plusMoney.transform.parent.GetComponent<Animator>();
            minusAnim = minusMoney.transform.parent.GetComponent<Animator>();

            ResetMoney();
        }

        public void ResetMoney(bool isSuccess = false)
        {
            if (isSuccess)
            {
                totalMoney += money;
                totalMoneyText.text = totalMoney.ToString();
            }
            money = initialMoney;
            OnMoneyUpdate();
        }

        public void AddMoney(int value)
        {
            money += value;
            PlayerAudio.Instance.CollectCoins();
            OnMoneyUpdate();

            if (Time.time - lastPlusTime > accumulationResetTime)
            {
                accumulatedPlusMoney = 0;
            }

            accumulatedPlusMoney += value;
            plusMoney.text = $"+{accumulatedPlusMoney}";
            lastPlusTime = Time.time;
            plusAnim.SetTrigger(ResetTrigger);
        }

        public void TakeMoney(int value)
        {
            money -= value;
            if (money <= 0)
            {
                money = 0;
                GameManager.Instance.Fail();
            }
            
            PlayerAudio.Instance.LoseCoins();
            OnMoneyUpdate();

            if (Time.time - lastMinusTime > accumulationResetTime)
            {
                accumulatedMinusMoney = 0;
            }

            accumulatedMinusMoney += value;
            minusMoney.text = $"-{accumulatedMinusMoney}";
            lastMinusTime = Time.time;
            minusAnim.SetTrigger(ResetTrigger);
        }

        private void OnMoneyUpdate()
        {
            var status = CalculateStatus();
            if (status.status != this.playerStatus)
            {
                this.playerStatus = status.status;
                ResetAppearance();

                status.appearance.SetActive(true);
                moneyBar.sprite = status.moneyBar;
                statusText.text = status.statusName;
                statusText.color = status.statusColor;
            }

            moneyText.text = money.ToString();
            moneyBar.fillAmount = money / (float) moneyForMaxStatus;
        }

        private StatusRequirement CalculateStatus() => statusRequirements.Last(statusRequirement => money >= statusRequirement.money);

        private void ResetAppearance()
        {
            foreach (var appearance in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                appearance.gameObject.SetActive(false);
            }
        }
    }
}