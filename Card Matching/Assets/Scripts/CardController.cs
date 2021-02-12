using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    [SerializeField]
    private GameObject backFace;
    [SerializeField]
    private GameController gameController;

    private int cardId;
    public int _cardId;
    /*{
        get { return cardId; }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        backFace.SetActive(false);
        StartCoroutine(CoroutineRevealedForFiveSec());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (backFace.activeSelf && !gameController.isGamePaused)
        {
            backFace.SetActive(false);
            gameController.CardRevealed(this);
        }
    }

    public void ChangeSprite(int id, Sprite cardFront)
    {
        //Debug.Log("id: "+id);
        cardId = id;
        _cardId = id;
        GetComponent<SpriteRenderer>().sprite = cardFront;
    }

    public void UnrevealCard()
    {
        backFace.SetActive(true);
    }

    IEnumerator CoroutineRevealedForFiveSec()
    {
        yield return new WaitForSeconds(5);
        backFace.SetActive(true);
    }
}
