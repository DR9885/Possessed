using UnityEngine;
using System.Collections;
using StateMachine;

public class QuicktimeController : MonoBehaviour
{

    [SerializeField]
    private float buttonDelay = 3, initialDelay = 4;

    private int buttonWins = 0, buttonFailure = 0;

    [SerializeField]
    private int possessionStep = 0;

	[SerializeField]
    private float timeStart, timeEnd;

    [SerializeField]
    private Size ButtonSize = new Size(128, 64), SpaceBetween = new Size(20, 20);

    [SerializeField]
    private KeyCode randomKey1, randomKey2, randomKey3, randomKey4, pressedKey;

    private Texture2D key1Img, key2Img, key3Img, key4Img;

    [SerializeField]
    private Texture2D leftKey, rightKey, upKey, downKey, successTexture, failureTexture;

    private int guiSet = 0;
    private int a = 0;


    private StateMachine<PossessionRemote, AbstractLivingBeing> stateMachine;

    KeyCode GenerateRandomKey()
    {
        return ContextActions.QuickActions[Random.Range(0, ContextActions.QuickActions.Count)].KeyCode;
    }

    public void Enable(StateMachine<PossessionRemote, AbstractLivingBeing> theState)
    {
        stateMachine = theState;
        timeStart = Time.time + initialDelay;
        timeEnd = Time.time + buttonDelay + initialDelay;
        this.enabled = true;
    }

    public void Disable()
    {
		possessionStep = 0;
		a = 0;
		guiSet = 0;
		buttonWins = 0;
		buttonFailure = 0;
        timeStart = 0;
        timeEnd = 0;
        randomKey1 = KeyCode.None;
        randomKey2 = KeyCode.None;
        randomKey3 = KeyCode.None;
        randomKey4 = KeyCode.None;
        key1Img = null;
        key2Img = null;
        key3Img = null;
        key4Img = null;
        this.enabled = false;
    }

    public int GetStep()
    {
        return possessionStep;
    }

    Texture2D WhichTexture(KeyCode key)
    {
        if (key == KeyCode.LeftArrow)
            return leftKey;
        else if (key == KeyCode.RightArrow)
            return rightKey;
        else if (key == KeyCode.UpArrow)
            return upKey;
        else
            return downKey;
    }

    bool HandleInput(KeyCode key, int a)
    {
        if (a == 1)
        {
            if (key == randomKey1)
                return true;
        }
        else if (a == 2)
        {
            if (key == randomKey2)
                return true;
        }

        return false;
    }


    void OnGUI()
    {
        Vector2 ghost2D = Camera.main.WorldToScreenPoint(MainPlayerGhost.Instance.Renderer.bounds.center);
        ghost2D.y = Screen.height - ghost2D.y;
        ghost2D.x = Screen.width / 2f;
        ghost2D.y = Screen.height / 2f;

        float leftX = ghost2D.x - ButtonSize.Width - SpaceBetween.Width;
        float rightX = ghost2D.x + SpaceBetween.Width;
        float upperY = ghost2D.y - ButtonSize.Height - SpaceBetween.Height;
        float lowerY = ghost2D.y + SpaceBetween.Height;

		GUI.color = new Color(1, 1, 1, 0.5f);
		
        /// Draw UPPER LEFT
        if(key3Img != null)
            GUI.Label(new Rect(leftX, upperY, ButtonSize.Width, ButtonSize.Height), key3Img);

        /// Draw UPPER RIGHT
        if (key4Img != null)
            GUI.Label(new Rect(rightX, upperY, ButtonSize.Width, ButtonSize.Height), key4Img);

        /// Draw LOWER LEFT
        if (key1Img != null)
            GUI.Label(new Rect(leftX, lowerY, ButtonSize.Width, ButtonSize.Height), key1Img);

        /// Draw LOWER RIGHT
        if (key2Img != null)
            GUI.Label(new Rect(rightX, lowerY, ButtonSize.Width, ButtonSize.Height), key2Img);

    
		GUI.color = new Color(1, 1, 1, 1);
	}

    // Update is called once per frame
    void Update()
    {
        //Quicktime event is going
        if (Time.time > timeStart && Time.time < timeEnd)
        {
            if (guiSet == 0)
            {
                randomKey1 = GenerateRandomKey();
                randomKey2 = GenerateRandomKey();
                //randomKey3 = GenerateRandomKey();
                //randomKey4 = GenerateRandomKey();

                key1Img = ResourceManager.TextureResources[randomKey1];
                key2Img = ResourceManager.TextureResources[randomKey2];
                key3Img = ResourceManager.TextureResources[randomKey3];
                key4Img = ResourceManager.TextureResources[randomKey4];

                guiSet = 1;
            }

            KeyCode f;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                f = KeyCode.LeftArrow;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                f = KeyCode.RightArrow;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                f = KeyCode.DownArrow;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                f = KeyCode.UpArrow;
            else
                f = KeyCode.None;
            
            if (a == 0 && f != KeyCode.None)
            {
                if (HandleInput(f, 1))
                {
                    a++;
                    key1Img = successTexture;
                    //key1Img.LoadImage(successTexture.EncodeToPNG());
                }
                else
                {
                    buttonFailure = 1;
                    key1Img = failureTexture;
                    a++;
                }
            }
            else if (a == 1 && f != KeyCode.None && buttonFailure != 1)
            {
                if (HandleInput(f, 2))
                {
                    buttonWins = 1;
                    key2Img = successTexture;
                }
                else
                {
                    buttonFailure = 1;
                    key2Img = failureTexture;
                }
            }

            if (buttonWins == 1)
            {
                possessionStep++;
                buttonWins = 0;
                buttonFailure = 0;
				guiSet = 0;
				timeStart = Time.time + buttonDelay;
				timeEnd = Time.time + buttonDelay + buttonDelay;
                a = 0;
            }
        }
        // Player has failed the quicktime event due to taking to long or pressing the wrong button
        else if (Time.time > timeEnd || buttonFailure == 1)
        {
            guiSet = 0;
            a = 0;
			possessionStep = possessionStep - 1;
            timeStart = Time.time + buttonDelay;
            timeEnd = Time.time + buttonDelay + buttonDelay;
            randomKey1 = GenerateRandomKey();
            randomKey2 = GenerateRandomKey();
            //quickTime1.texture = new Texture2D(0,0);
            //quickTime2.texture = new Texture2D(0, 0);
            //quickTime3.texture = new Texture2D(0, 0);
            //quickTime4.texture = new Texture2D(0, 0);
            buttonFailure = 0;
        }

    }

}