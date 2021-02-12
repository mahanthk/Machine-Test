using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public const int rows = 3;
    public const int cols = 7;
    public const float colOffset = 2.5f;
    public const float rowOffset = 3f;
    public bool isGamePaused = false;
    public bool canFlip;

    private int remainingMatches = 7;
    [SerializeField]
    private Text remainingMatchesText;
    [SerializeField]
    private GameObject panelWin;
    [SerializeField]
    private GameObject panelPause;
    [SerializeField]
    private CardController mainCard;
    [SerializeField]
    private Sprite[] images;

    // Start is called before the first frame update
    void Start()
    {
        canFlip = true;
        panelWin.SetActive(false);
        panelPause.SetActive(false);
        remainingMatchesText.text = "Remaining Matches: " + remainingMatches;
        Vector2 startPos = mainCard.gameObject.transform.position;
        int[] numbers = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 };

        numbers = Shuffle(numbers);
        for (int i = 0; i < cols; i++)
        {
            for(int j=0; j < rows; j++)
            {
                CardController card;
                if(i == 0 && j == 0)
                {
                    card = mainCard;
                }
                else
                {
                    card = Instantiate(mainCard) as CardController;
                }

                int index = j * cols + i;
                //Debug.Log("number:" + numbers[index]);
                int id = numbers[index];
                mainCard.ChangeSprite(id, images[id]);
                Debug.Log(card._cardId);
                float posX = (colOffset * i) + startPos.x;
                float posY = (rowOffset * j) + startPos.y;
                card.transform.position = new Vector2(posX, posY);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGamePaused)
        {
            panelPause.SetActive(true);
            isGamePaused = true;
        }else if(Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
        {
            panelPause.SetActive(false);
            isGamePaused = false;
        }
    }

    private int[] Shuffle(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];

        for(int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int random = Random.Range(i, newArray.Length);
            newArray[i] = newArray[random];
            newArray[random] = temp;
        }
        return newArray;
    }



    CardController firstCard;
    CardController secondCard;
    CardController thirdCard;
    private bool secondCardMatch;
    private bool thirdCardMatch;

    public void CardRevealed(CardController card)
    {
        if(firstCard == null)
        {
            Debug.Log("First card opened");
            firstCard = card;
        }
        else if(secondCard == null)
        {
            Debug.Log("Second card opened");
            secondCard = card;
            StartCoroutine(CoroutineSecondCheckMatch());
        }else if(thirdCard == null && secondCardMatch)
        {
            Debug.Log("Third card opened");
            thirdCard = card;
            StartCoroutine(CoroutineThirdCheckMatch());
        }
    }

    private IEnumerator CoroutineSecondCheckMatch()
    {
        Debug.Log("First card id: " + firstCard._cardId + ", Second card id: " + secondCard._cardId);
        if(firstCard._cardId == secondCard._cardId)
        {
            Debug.Log("Second card matched");
            secondCardMatch = true;
        }
        else
        {
            Debug.Log("Second card is not matched");
            secondCardMatch = false;
            thirdCardMatch = false;
            canFlip = false;
            yield return new WaitForSeconds(0.3f);
            firstCard.UnrevealCard();
            secondCard.UnrevealCard();
            canFlip = true;
            firstCard = null;
            secondCard = null;
        }
    }

    private IEnumerator CoroutineThirdCheckMatch()
    {
        if(secondCard._cardId == thirdCard._cardId)
        {
            Debug.Log("Third card matched");
            thirdCardMatch = true;
            remainingMatches -= 1;
            remainingMatchesText.text = "Remaining Matches: " + remainingMatches;
            if (remainingMatches.Equals(0))
            {
                panelWin.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Third card is not matched");
            secondCardMatch = false;
            thirdCardMatch = false;
            canFlip = false;
            yield return new WaitForSeconds(0.3f);
            secondCard.UnrevealCard();
            thirdCard.UnrevealCard();
            firstCard.UnrevealCard();
            canFlip = true;
            /*firstCard = null;
            secondCard = null;
            thirdCard = null;*/
        }
        firstCard = null;
        secondCard = null;
        thirdCard = null;
    }

    //==================================================================================================================
    //Button functions

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadHomePage()
    {
        SceneManager.LoadScene("Home");
    }
}
