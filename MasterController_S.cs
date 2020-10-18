using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController_S : MonoBehaviour
{
    public static GameObject[] storedGameObject;
    public static MasterController_S self;
    public static Canvas_S _Canvas_S;
    public static Player_S player_S;
    public static Inventory_S inventory_S;
    public static SceneLoader_S SceneLoader_S;
    public static ChatterBox_S chatterBox_S;
    public static CanvasMessageText_S canvasMessageText_S;
    public static int[] scoreKeeper_Correct;
    public static int[] scoreKeeper_Incorrect;

    public List<QuestionList_S> questionRepository;

    public float money;
    public float health;
    public float HEALTH_STUN_TIME;
    public float levelTotal;
    public int lives = 4;
    public int ammo;
    private ArrayList lastQuestionArray;
    public int QUESTION_COOLDOWN = 5;
    public bool hasLumin;
    public bool hasClimb;
    public bool hasAmmo;

    private static bool loadSave = false;
    private static int lastSceneIndex;
    private bool healthStun;

    void Awake()
    {
        transform.parent.DetachChildren();


        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
        }
        else Destroy(gameObject);

        if (loadSave)
        {
            loadSave = false;
            LoadSaved();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastQuestionArray = new ArrayList();
        scoreKeeper_Correct = new int[9];
        scoreKeeper_Incorrect = new int[9];
        //if (inventory_S != null)
        //    storedGameObject = new GameObject[inventory_S.slots.Length];
        // This is a singleton, so the Start method is called ONCE forever, when untouched.
    }

    // Update is called once per frame
    void Update()
    {
        


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }


    public Question_S RandomQuestion() // Returns a question, but one that is not the same as the last 5 questions
    {
        int arrayNum = 0;
        string sceneSpecial = SceneManager.GetActiveScene().name;
        switch (sceneSpecial)
        {
            case "Level 0":
                break;
            case "Level Tutorial":
                arrayNum = 1;
                break;
            case "Level 1.0":
                arrayNum = 2;
                break;
            case "Level 1.5":
                arrayNum = 3;
                break;
            case "Level 1.8":
                arrayNum = 4;
                break;
            case "Level 2":
                arrayNum = 5;
                break;
            default:
                break;
        }

        int num = 0;
        bool isTheSame = false; // Throw out any questions that match the last QUESTION_COOLDOWN of previous array element indexes
        do
        {
            isTheSame = false;
            num = Random.Range(0, questionRepository[arrayNum].questions.Count);
            if (lastQuestionArray.Count > 0)
            {
                foreach (int arrayElement in lastQuestionArray)
                {
                    if (num == arrayElement)
                        isTheSame = true;
                }
            }
        } while (isTheSame);
        if (lastQuestionArray.Count >= QUESTION_COOLDOWN)
            lastQuestionArray.RemoveAt(QUESTION_COOLDOWN-1);
        lastQuestionArray.Add(num);

        Question_S question = questionRepository[arrayNum].questions[num];
        
        return question;
    }


    public void SetActivateLumin(bool newActivate) // Activates the Follow Object, lots of redunency here to ensure this works with singleton objs or otherwise
    {
        hasLumin = newActivate;
    }

    public void ChangeMoneyAmount(float amount) // Controls all money that goes through the system, is a singleton so this is saved after each level
    {
        money += amount;
        Canvas_S._UIMoney_S.ChangeMoneyUI(amount);

        if (amount >= 2)
        {
            canvasMessageText_S.SendCanvasMessage("Collected " + amount + " Pins");
        }
        else if (amount <= 0)
        {
            canvasMessageText_S.SendCanvasMessage("Traded " + amount + " Pins");

            if (money < 0)
            {
                Debug.LogError("Money Set Below 0!!");
            }
        }
    }

    public void ChangeSpecificHealth(bool increase,bool stun,int order)
    {
        if ((increase && UIHealth_S.roleCall[order]) || (!increase && !UIHealth_S.roleCall[order]))
        {
            Debug.LogError("Role call " + order + " is already true.");
            return;
        }

        int healthChange = -1;
        if (increase)
        {
            healthChange = 1;
        }

        if (!healthStun)
        {
            if (health + healthChange < 1)
            {
                PlayerDeath();
            }
            else
            {
                if (stun)
                {
                    SoundManager_S.PlaySound("hurt"); // hurt sound
                    healthStun = true;
                    StartCoroutine(PreventDamage(HEALTH_STUN_TIME));
                    player_S.TakeStunDamage();
                }
                health += healthChange;
                Canvas_S._UIHealth_S.SetHealth(increase,order);
            }
        }
    }

    public void ChangeHealth(int healthChange,bool stun)
    {
        if (!healthStun)
        {
            if (health + healthChange < 1)
            {
                PlayerDeath();
            }
            else
            {
                if (stun)
                {
                    SoundManager_S.PlaySound("hurt"); // hurt sound
                    healthStun = true;
                    StartCoroutine(PreventDamage(HEALTH_STUN_TIME));
                    player_S.TakeStunDamage();
                }
                health += healthChange;
                Canvas_S._UIHealth_S.SetHealth();
            }
        }
    }

    public void ChangeHealth(int healthChange, bool stun, Vector2 stunDirection)
    {
        if (!healthStun)
        {
            if (health + healthChange < 1)
            {
                PlayerDeath();
            }
            else
            {
                if (stun)
                {
                    SoundManager_S.PlaySound("hurt"); //hurt sound
                    healthStun = true;
                    StartCoroutine(PreventDamage(HEALTH_STUN_TIME));
                    player_S.TempAnimStunActivate(stunDirection);
                    player_S.TakeStunDamage();
                }
                health += healthChange;
                Canvas_S._UIHealth_S.SetHealth();
            }
        }
    }

    public void ChangeAmmo(int newAmmo)
    {
        ammo = newAmmo;
        Ammo_S.ammoFloat = ammo;
        Ammo_S.ammoText = ammo.ToString();
        if (ammo <= 0)
        {
            ammo = 0;
            hasAmmo = false;
        }

        else if (ammo > 0)
        {
            hasAmmo = true;
        }
    }

    public void ChangeScore(bool gotCorrect)
    {
        if (gotCorrect)
        {
            scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex] += 1;
            SoundManager_S.PlaySound("correct");
        }
        else
        {
            scoreKeeper_Incorrect[SceneManager.GetActiveScene().buildIndex] += 1;
            SoundManager_S.PlaySound("incorrect");
        }
    }

    public void KillAmmoCanvasObject(Ammo_S ammoElement)
    {
        for (int i = 0; i < storedGameObject.Length; i++)
        {
            if (storedGameObject[i] != null && storedGameObject[i].name.Equals("Projectile Button"))
            {
                Destroy(ammoElement.gameObject);
                storedGameObject[i] = null;
                inventory_S.isFull[i] = false;
            }
        }
    }


    public void WinScreenActivate(WinScreen_S winScreen)
    {
        PlayerPrefs.SetFloat("HasSave", 0f);

        ammo = 0;
        hasClimb = false;
        hasLumin = false;

        int numCorrect = 0;
        int numIncorrect = 0;
        if (scoreKeeper_Correct != null)
        {
            foreach (int score in scoreKeeper_Correct)
            {
                numCorrect += score;
            }
            foreach (int score in scoreKeeper_Incorrect)
            {
                numIncorrect += score;
            }
        }
        winScreen.qcText.text = "Total Correct: " + (numCorrect);
        winScreen.qiText.text = "Total Incorrect: " + (numIncorrect);
        winScreen.qaText.text = "Total Questions Answered: " + (numCorrect + numIncorrect);
        winScreen.piText.text = "Total FBLA State Pins Aquired: " + (money);
    }

    public void PlayerDeath()
    {

        if (lives > 0)
        {
            lives -= 1;
            lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(lastSceneIndex);
            SceneManager.LoadScene("Life Over");
        }
        else
        {
            health = 8;
            money = 0;
            ChangeAmmo(-15);
            scoreKeeper_Correct = new int[9];
            scoreKeeper_Incorrect = new int[9];
            storedGameObject = null;
            GameRestart();
        }
    }

    public void GameRestart()
    {
        PlayerPrefs.SetFloat("HasSave", 0f); // Prevents the player from accessing save
        SceneManager.LoadScene("Game Over");
    }

    public void ReloadPreviousScene()
    {
        int temp = lastSceneIndex;
        Debug.Log(temp);
        health = 8;
        health = 8;
        money = 0;
        ChangeAmmo(-15);
        scoreKeeper_Correct[temp] = 0;
        scoreKeeper_Incorrect[temp] = 0;
        storedGameObject = null;
        LoadScene(temp);
    }

    public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }


    public void ResetVariables()
    {
        health = 8;
        money = 0;
        ChangeAmmo(-15);
        scoreKeeper_Correct = new int[9];
        scoreKeeper_Incorrect = new int[9];
        storedGameObject = null;
    }


    public void ResetScene()
    {
        Canvas_S._UIMoney_S.ChangeMoneyUI(MasterController_S.self.money);
        int num = SceneManager.GetActiveScene().buildIndex;
        health = 8;
        ChangeAmmo(-15);
        scoreKeeper_Correct[num] = 0;
        scoreKeeper_Incorrect[num] = 0;
        storedGameObject = null;
        SceneManager.LoadScene(num);
    }


    public static void StoreInventoryGameObject(GameObject inventoryGameObject, int inventorySlot)
    {
        storedGameObject[inventorySlot] = inventoryGameObject;
    }

    public static void LoadInventory()
    {
        bool createPickaxe = false;
        bool createPamplets = false;

        Canvas_S._UIMoney_S.ChangeMoneyUI(MasterController_S.self.money);

        if (self.hasClimb)
        {
            player_S.SetCanClimb(true);
            if (storedGameObject == null)
            {
                createPickaxe = true;
            }
        }
        if (self.hasAmmo)
        {
            player_S.SetCanAttack(true);
            if (storedGameObject == null)
            {
                createPamplets = true;
            }
        }

        if (storedGameObject == null)
        {
            storedGameObject = new GameObject[inventory_S.slots.Length];
            if (createPickaxe)
            {
                Debug.Log("Lets Go");
                Instantiate(Resources.Load("Savable/Pickaxe Button") as GameObject, inventory_S.slots[0].transform, false);
                inventory_S.isFull[0] = true;
            }
            if (createPamplets)
            {
                Instantiate(Resources.Load("Savable/Pamplets Button") as GameObject, inventory_S.slots[1].transform, false);
                inventory_S.isFull[1] = true;
            }
        }
        else
        {
            for (int i = 0; i < storedGameObject.Length; i++)
            {
                if (storedGameObject[i] != null)
                {
                    inventory_S.isFull[i] = true;
                    Instantiate(storedGameObject[i], inventory_S.slots[i].transform, false);
                }
            }
        }
        SaveSystem_S.SavePlayer(self);
        PlayerPrefs.SetFloat("HasSave", 1.0f);
    }
    

    public void LoadSaved()
    {
        PlayerData_S data = SaveSystem_S.LoadPlayer();
        hasLumin = data.hasLumin;
        hasClimb = data.hasClimb;
        health = data.health;
        money = data.money;
        SceneManager.LoadScene(data.level);
    }

    public static int GetLocalScore(bool askingForCorrect) // returns the score of a specific level
    {
        if (askingForCorrect)
            return scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex];
        else
            return scoreKeeper_Incorrect[SceneManager.GetActiveScene().buildIndex];
    }


    public static int GetLevelIndex() //returns scene index
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    public static int GetLevelScore(bool askingForCorrect, int levelNum) // returns the score of a specific level
    {
        if (askingForCorrect)
            return scoreKeeper_Correct[levelNum];
        else
            return scoreKeeper_Incorrect[levelNum];
    }

    IEnumerator PreventDamage(float time)
    {
        yield return new WaitForSeconds(time);
        healthStun = false;
    }
}
