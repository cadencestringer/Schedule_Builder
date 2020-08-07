using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    //Collections
    public List<int> m_comboList;
    public List<int> m_intSchedules = new List<int>();
    public List<Info> m_listOfInfo;
    public List<Schedule> m_listOfSchedules;
    public List<Schedule> m_newListOfSchedules = new List<Schedule>();

    //UI Objects
    public Text m_scheduleText;
    public Text m_errorText;
    public Button m_nextButton;
    public Button m_prevButton;

    //Instance Variables
    public Info m_infoPrefab;
    public Schedule m_schedulePrefab;
    public int m_currentSchedule;
    public Manager m_manager;

    //Scroll View
    public Transform m_cardSpawn;
    public ScrollRect m_scrollView;

    //Generates combinations
    static void Combinator(int[] m_arr, int[] data, int start, int end, int index,
                       int r, List<int> m_intSchedules)
    {
        if (index == r)
        {
            for (int j = 0; j < r; j++)
                m_intSchedules.Add(data[j]);
            return;
        }

        for (int i = start; i <= end && end - i + 1 >= r - index; i++)
        {
            data[index] = m_arr[i];
            Combinator(m_arr, data, i + 1, end, index + 1, r, m_intSchedules);
        }
    }

    //Uses Combinator to make and store combinations
    static void FinishCombinations(int[] m_arr, int n, int r, List<int> m_intSchedules)
    {
        //Data is a temporary array that stores combinations
        int[] data = new int[r];
        Combinator(m_arr, data, 0, n - 1, 0, r, m_intSchedules);
    }

    //Finalizes combinations
    static public void MakeCombinations(int m_numClasses, List<int> m_intSchedules)
    {
        //Creates array based on length of classes input list
        int[] m_arr = new int[m_numClasses];
        for (int i = 0; i < m_numClasses; i++)
            m_arr[i] = i;
        //Combinations are a length of 5
        int r = 5;
        int n = m_arr.Length;
        FinishCombinations(m_arr, n, r, m_intSchedules);
    }

    //Loads the specified schedule into the Scroll View
    public void LoadSchedule(int i)
    {
        //If there are schedules to display
        if (m_listOfSchedules.Count > 0)
        {
            m_errorText.text = "";
            Info newCard;
            int current = 0;
            m_scrollView.content.sizeDelta = new Vector2(0, 0);
            foreach (Info x in m_listOfSchedules[i].m_cardList)
            {
                newCard = Instantiate(m_infoPrefab, m_cardSpawn);
                Info.Copy(x, newCard);
                newCard.SetFullCard();

                newCard.transform.position = newCard.transform.position + new Vector3(0, -(current * 165), 0);
                m_scrollView.content.sizeDelta = m_scrollView.content.sizeDelta + new Vector2(0, 170);
                current++;
            }
        }
        //If there aren't any schedules to display
        else
        {
            m_errorText.text = "NO POSSIBLE COMBINATIONS.";
        }
    }

    //Cycles through schedules with prev/next buttons
    public void ChangeSchedule(string s)
    {
        m_nextButton.interactable = true;
        m_nextButton.interactable = true;

        if (s.Equals("n"))
        {
            m_currentSchedule++;
            m_scheduleText.text = "Schedule # " + (m_currentSchedule + 1);
            LoadSchedule(m_currentSchedule);
        }
        else
        {
            m_currentSchedule--;
            m_scheduleText.text = "Schedule # " + (m_currentSchedule + 1);
            LoadSchedule(m_currentSchedule);
        }
    }

    //Determines if the prev/next buttons are active
    public void Update()
    {
        //Doesn't let the user click back if its the first schedule
        if (m_currentSchedule <= 0)
            m_prevButton.interactable = false;
        //Lets user click back if it's not the first schedule
        else
            m_prevButton.interactable = true;
        //Doesn't let the user click forward if its the last schedule
        if (m_currentSchedule >= m_listOfSchedules.Count - 1)
            m_nextButton.interactable = false;
        //Lets the user click forward if its not the last schedule
        else
            m_nextButton.interactable = true;
    }

    //Checks if a schedule combination contains conflicts     public void CheckConflict()     {         //Makes a duplicate list that we iterate through         foreach (Schedule i in m_listOfSchedules)             m_newListOfSchedules.Add(i);          //Looks through every schedule in all schedules         foreach (Schedule sched in m_newListOfSchedules)         {             //Looks through every class "c" in the schedule             foreach (Info c in sched.m_cardList)             {                 //Gets another class from the same schedule to compare to                 foreach (Info d in sched.m_cardList)                 {                     //If the comparison class d isn't the same as class c                     if(!c.Equals(d))                     {                         //If class name is the same, delete schedule because you can't have the same class twice                         if (c.m_className.Equals(d.m_className))                         {                             m_listOfSchedules.Remove(sched);                         }                         //If days are the same                         else if (d.DaysEqual(c))                         {                             //If the start times are the same                             if (c.m_start == d.m_start)                             {                                 //Remove the schedule                                 m_listOfSchedules.Remove(sched);                             }                             //If class names and start times are different                             else                             {                                 //If the start of c is between start and end of d                                 if (c.m_start <= d.m_end && c.m_start >= d.m_start)                                 {                                     m_listOfSchedules.Remove(sched);                                 }                             }                         }                     }                 }             }
        }     } 
    //Runs on start
    public void Start()
    {
        m_errorText.text = "";
        m_nextButton.interactable = true;
        m_prevButton.interactable = false;
        m_manager = FindObjectOfType<Manager>();
        m_listOfInfo = Manager.GetInfoList();

        MakeCombinations(Manager.m_numClasses, m_intSchedules);

        //Iterates through the integer schedules and creates new schedule objects every 5 classes
        int inc = 0;
        int scheds = 0;
        foreach (int i in m_intSchedules)
        {
            switch (inc)
            {
                case 0:
                    Schedule newSchedule = Instantiate(m_schedulePrefab);
                    m_listOfSchedules.Add(newSchedule);
                    inc++;
                    scheds++;
                    break;
                case 4:
                    inc = 0;
                    break;
                default:
                    inc++;
                    break;
            }
            m_listOfSchedules[scheds-1].m_cardList.Add(m_listOfInfo[i]);
        }

        //Sorts them from earliest start time to latest start time
        foreach (Schedule s in m_listOfSchedules)
        {
            s.m_cardList.Sort();
        }

        //Removes schedules that have any conflicts
        CheckConflict();

        //Displays first schedule and loads display
        m_scheduleText.text = "Schedule # " + (m_currentSchedule + 1);
        m_currentSchedule = 0;
        LoadSchedule(m_currentSchedule);
    }
}
