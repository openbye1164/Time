using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject WinUI;
    private const int winCardCouples = 6;
    private int curCardCouples = 0;
    private bool canPlayerClick = true;

    public Sprite BackSprite;
    public Sprite SuccessSprite;
    public Sprite[] FrontSprites;

    public GameObject CardPre;
    public Transform CardsView;
    private List<GameObject> CardObjs;
    private List<Card> FaceCards;

    // Use this for initialization
    void Start()
    {

        CardObjs = new List<GameObject>();
        FaceCards = new List<Card>();

        //��12�ſ���������ɺ���ӵ�CardObjs����
        for (int i = 0; i < 6; i++)
        {
            Sprite FrontSprite = FrontSprites[i];
            for (int j = 0; j < 2; j++)
            {
                //ʵ��������
                GameObject go = (GameObject)Instantiate(CardPre);
                //��ȡCard������г�ʼ��������¼�����Ϸ������ͳһ����
                //���Կ��Ƶĵ���¼��ļ����ڹ�����ָ��
                Card card = go.GetComponent<Card>();
                card.InitCard(i, FrontSprite, BackSprite, SuccessSprite);
                card.cardBtn.onClick.AddListener(() => CardOnClick(card));

                CardObjs.Add(go);
            }
        }

        while (CardObjs.Count > 0)
        {
            //ȡ�����������ҿ�����
            int ran = Random.Range(0, CardObjs.Count);
            GameObject go = CardObjs[ran];
            //������ָ����Panel��Ϊ�����壬�����ͻᱻ���ǵ�����Զ�����
            go.transform.parent = CardsView;
            //local�ͱ�ʾ����ڸ�������������ϵ���˴���У������
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //��CardObjs�б����Ƴ�������ָ������б������������1��
            CardObjs.RemoveAt(ran);
        }
    }

    private void CardOnClick(Card card)
    {
        if (canPlayerClick)
        {
            //���ж��Ƿ���Ե�����ɵ����ֱ�ӷ���
            card.SetFanPai();
            //��ӵ��ȶ�������
            FaceCards.Add(card);
            //������������ˣ��򲻿��ٵ��������Эͬ����
            if (FaceCards.Count == 2)
            {
                canPlayerClick = false;
                StartCoroutine(JugdeTwoCards());
            }
        }
    }

    IEnumerator JugdeTwoCards()
    {
        //��ȡ�����ſ��ƶ���
        Card card1 = FaceCards[0];
        Card card2 = FaceCards[1];
        //��ID���бȶ�
        if (card1.ID == card2.ID)
        {
            Debug.Log("Success......");
            //��ʱ���ڴ˴��ȴ�0.8�����ִ����һ�����
            //Э�̲�Ӱ��������Ľ��У������������Сʵ��
            //�������0.8�ĳ�8�룬��Update�д�ӡTime.time�ᷢ�ֲ�����ͣ�ٵ�ʱ��
            yield return new WaitForSeconds(0.8f);
            card1.SetSuccess();
            card2.SetSuccess();
            curCardCouples++;
            if (curCardCouples == winCardCouples)
            {
                WinUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Failure......");
            //���ʧ�ܵȴ���ʱ��Ҫ��������ΪҪ����Ҽ�������
            yield return new WaitForSeconds(1.5f);
            card1.SetRecover();
            card2.SetRecover();
        }

        FaceCards = new List<Card>();
        canPlayerClick = true;
    }
}