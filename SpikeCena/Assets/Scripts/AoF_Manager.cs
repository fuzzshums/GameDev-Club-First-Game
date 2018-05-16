using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoF_Manager : MonoBehaviour {

    // Use this for initialization
    public GameObject AoF_GO;

    public static List<GameObject> AoF_List;
    public static GameObject Choosen_AoF;

    float timeSwitch;
    float interval;
    int choice;

    int num_splits;
    public static bool others_active;

	void Start () {
        AoF_List = new List<GameObject>();

        interval = 9;
        timeSwitch = 0;
        choice = 0;
        others_active = false;

        num_splits = 2;

        Init_AoFs();
	}
	
	// Update is called once per frame
	void Update () {
        make_choice();
        decide_action(choice);
	}

    void Init_AoFs()
    {
        for (int i = 0; i < num_splits; i++)
        {
            GameObject AreaOfFocus = Instantiate(AoF_GO);
            AoF_List.Add(AreaOfFocus);
        }
    }

    void make_choice()
    {
        timeSwitch += Time.deltaTime;
        if (timeSwitch > interval)
        {
            Debug.Log("Switching AoF action!");
            timeSwitch = 0;
            choice = UnityEngine.Random.Range(0, 4);
        }
        decide_action(choice);
    }

    void deactivate_others()
    {
        for (int i = 1; i < AoF_List.Count; i++)
        {
            AoF_List[i].SetActive(false);
            AoF_List[i].GetComponent<Renderer>().enabled = false;
        }
        others_active = false;
    }

    void reactivate_others()
    {
        for (int i = 1; i < AoF_List.Count; i++)
        {
            AoF_List[i].SetActive(true);
            AoF_List[i].GetComponent<Renderer>().enabled = true;
        }
        others_active = true;
    }

    void decide_action(int choice)
    {
        if (choice == 0)
        {
            AoF_List[0].GetComponent<Area_Of_Focus>().follow_player();
            deactivate_others();
        }
        else if (choice == 1)
        {
            AoF_List[0].GetComponent<Area_Of_Focus>().move_freely(4.5f);
            deactivate_others();
        }
        else if (choice == 2)
        {
            AoF_List[0].GetComponent<Area_Of_Focus>().full_screen();
            deactivate_others();
        }
        else if (choice == 3)
        {
            reactivate_others();
            for (int i = 0; i < num_splits; i++)
            {
                AoF_List[i].GetComponent<Area_Of_Focus>().move_freely(1.5f);
            }
        }
    }

    public float chooseAOF()
    {
        if (others_active == false)
        {
            return AoF_List[0].transform.position.x;
        }
        else
        {
            Choosen_AoF = AoF_List[UnityEngine.Random.Range(0, AoF_List.Count)];
            return Choosen_AoF.transform.position.x;
        }

    }

    public float chooseAOF2()
    {
        if (others_active == false)
        {
            return AoF_List[0].transform.localScale.x/2; //calling function needs half bounds!
        }
        else
        {
            Choosen_AoF = AoF_List[UnityEngine.Random.Range(0, AoF_List.Count)];
            return Choosen_AoF.transform.localScale.x/2;
        }

    }
}
