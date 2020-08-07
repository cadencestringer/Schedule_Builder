using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Manager : MonoBehaviour
{
    //Collections
    public List<GameObject> m_allCardList;
    public static List<Info> m_allInfoList = new List<Info>();
    public static List<int> m_copyIntSchedules = new List<int>();

    //UI Objects
    public InputField m_classInput;
    public InputField m_teacherInput;
    public InputField m_sectionInput;
    public Dropdown m_startHourInput;
    public Dropdown m_startMinuteInput;
    public Dropdown m_startAMPMInput;
    public Dropdown m_endHourInput;
    public Dropdown m_endMinuteInput;
    public Dropdown m_endAMPMInput;
    public Toggle m_requiredInput;
    public Toggle m_mondayInput;
    public Toggle m_tuesdayInput;
    public Toggle m_wednesdayInput;
    public Toggle m_thursdayInput;
    public Toggle m_fridayInput;
    public Text m_error;


    //Scroll View
    public Transform m_cardSpawn;
    public ScrollRect m_scrollView;
    public int m_cardCount;

    //Instance Variables
    public Info m_tempInfo;
    public Info m_infoPrefab;
    public bool m_cardExists = false;
    public static bool m_ready;
    public bool m_timeError = false;
    public static int m_numClasses;


    //Accessor for list of all Info Objects
    public static List<Info> GetInfoList()
    {
        return m_allInfoList;
    }

    //Accessor for the list of index combinations
    public static List<int> GetComboList()
    {
        return m_copyIntSchedules;
    }

    //Runs on start
    public void Start()     {
        m_error.text = string.Empty;
        DontDestroyOnLoad(gameObject);
        m_ready = false;     }

    //Makes sure the class length is more than 50 min and less than 3 hrs
    public void CheckTimes()
    {
        if (m_tempInfo.m_start > m_tempInfo.m_end)
        {
            m_timeError = true;
        }

        if (((m_tempInfo.m_end - m_tempInfo.m_start) > 180) || ((m_tempInfo.m_end - m_tempInfo.m_start) < 50))
        {
            m_timeError = true;
        }
        else
            m_timeError = false;
    }

    //Adds section to list of sections
    public void AddSection()
    {
        StartTimeConverter();
        EndTimeConverter();
        Info newCard;
        m_cardExists = false;
        m_timeError = false;
        //Prevents them from entering incomplete class info
        if (m_classInput.text != "" && m_teacherInput.text != "" && m_sectionInput.text != ""
        && m_startHourInput.value != 0 && m_endHourInput.value != 0 && m_endMinuteInput.value != 0         && m_startAMPMInput.value != 0 && m_endAMPMInput.value != 0)
        {
            //If at least one of the days has been checked
            if (m_mondayInput.isOn == true || m_tuesdayInput.isOn == true
            ||m_wednesdayInput.isOn == true || m_thursdayInput.isOn == true
            || m_fridayInput.isOn == true)
            {
                CheckTimes();
                if (m_timeError == false)
                {
                    foreach (GameObject c in m_allCardList)
                    {
                        Info i = c.GetComponent<Info>();
                        if (m_tempInfo.Equals(i))
                        {
                            m_cardExists = true;
                            m_error.text = "Class already exists!";
                        }
                    }
                    if (!m_cardExists)
                    {
                        newCard = Instantiate(m_infoPrefab, m_cardSpawn.transform);
                        DontDestroyOnLoad(newCard);
                        Info.Copy(m_tempInfo, newCard);
                        newCard.SetCard();
                        newCard.transform.position = newCard.transform.position + new Vector3(0, -(m_cardCount * 125), 0);
                        m_scrollView.content.sizeDelta = m_scrollView.content.sizeDelta + new Vector2(0, 135);
                        m_error.text = string.Empty;

                        m_allCardList.Add(newCard.gameObject);
                        Info copyInfo = new Info();
                        Info.Copy(newCard.GetComponent<Info>(), copyInfo);
                        m_allInfoList.Add(copyInfo);
                        m_numClasses = m_allCardList.Count;

                        ResetInputs();
                        m_tempInfo.Reset();
                    }
                }
                //If there is a time error (longer than 3 hours, shorter than 50 mins)
                else
                {
                    m_error.text = "Time input error.";
                    m_timeError = false;
                }
            }
            //If not at least one day has been checked
            else
            {
                m_error.text = "Add day(s) for class.";
            }
        }
        //If any of those fields are left blank
        else
        {
            m_error.text = "You're missing info!";
        }
        foreach (GameObject c in m_allCardList)
        {
            Info i = c.GetComponent<Info>();
        }
        m_cardCount = m_allCardList.Count;
    }

    //Resets the UI inputs
    public void ResetInputs()
    {
        m_classInput.text = string.Empty;
        m_teacherInput.text = string.Empty;
        m_sectionInput.text = string.Empty;
        m_startHourInput.value = 0;
        m_startMinuteInput.value = 0;
        m_startAMPMInput.value = 0;
        m_endHourInput.value = 0;
        m_endMinuteInput.value = 0;
        m_endAMPMInput.value = 0;
        m_requiredInput.isOn = false;
        m_mondayInput.isOn = false;
        m_tuesdayInput.isOn = false;
        m_wednesdayInput.isOn = false;
        m_thursdayInput.isOn = false;
        m_fridayInput.isOn = false;
    }

    /*
    m_arr[] ---> Input Array
    data[] ---> Temporary array to store current combination
    start & end ---> Indexes in m_arr[]
    index ---> Current index in data[]
    r ---> Size of combinations (5 classes)
    m_numClasses ---> SHOULD (change once its in Generator) count the cardlist
    before calling MakeCombinations()
    m_intSchedules ---> Integer list of all the combinations, in "groups" of 5
    */

    //Error check called when Generate button pressed
    public void GenerateSchedules()
    {
        if (m_numClasses < 5)
        {
            m_error.text = "You need 5 classes.";
        }

        else
        {
            m_error.text = string.Empty;
        }
    }

    //Converts start time to minutes
    public void StartTimeConverter()
    {
        m_tempInfo.m_start = (m_tempInfo.m_startHour * 60) + m_tempInfo.m_startMinute;
        if(!(m_tempInfo.m_startHour == 12 && m_tempInfo.m_startAMPM == "PM"))
        {
            if (m_tempInfo.m_startAMPM == "PM")
            {
                m_tempInfo.m_start += 720;
            }
        }
    }

    //Converts end time to minutes
    public void EndTimeConverter()
    {
        m_tempInfo.m_end = (m_tempInfo.m_endHour * 60) + m_tempInfo.m_endMinute;
        if (!(m_tempInfo.m_endHour == 12 && m_tempInfo.m_endAMPM == "PM"))
        {
            if (m_tempInfo.m_endAMPM == "PM")
            {
                m_tempInfo.m_end += 720;
            }
        }
    }

    //Mutator for class name
    public void SetClassName(string s)
    {
        m_tempInfo.m_className = s.Trim();
    }

    //Mutator for teacher name
    public void SetTeacherName(string s)
    {
        m_tempInfo.m_teacherName = s.Trim();
    }

    //Mutator for section number
    public void SetSectionNumber(string s)
    {
        Regex r = new Regex("^[0-9]+$");
        if (r.IsMatch(s))
        {
            int i = int.Parse(s);
            m_tempInfo.m_section = i;
        }
        else
        {
            m_sectionInput.text = string.Empty;
        }
    }

    //Mutator for start hour
    public void SetStartHour(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_startHour = int.Parse(m_startHourInput.options[i].text); m_startAMPMInput.value = 0;
        }
        else
        {
            m_tempInfo.m_startHour = -1;
        }
    }

    //Mutator for start minute
    public void SetStartMinute(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_startMinute = int.Parse(m_startMinuteInput.options[i].text);
        }
        else
        {
            m_tempInfo.m_startMinute = -1;
        }
    }

    //Mutator for start AM/PM
    public void SetStartAMPM(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_startAMPM = m_startAMPMInput.options[i].text;
        }
        else
        {
            m_tempInfo.m_startAMPM = "AM";
        }
    }

    //Mutator for end hour
    public void SetEndHour(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_endHour = int.Parse(m_endHourInput.options[i].text);
        }
        else
        {
            m_tempInfo.m_endHour = -1;
        }
    }

    //Mutator for end minute
    public void SetEndMinute(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_endMinute = int.Parse(m_endMinuteInput.options[i].text);
        }
        else
        {
            m_tempInfo.m_endMinute = -1;
        }
    }

    //Mutator for end AM/PM
    public void SetEndAMPM(int i)
    {
        if (i > 0)
        {
            m_tempInfo.m_endAMPM = m_endAMPMInput.options[i].text;
        }
        else
        {
            m_tempInfo.m_endAMPM = "AM";
        }
    }

    //Mutator for requisite
    public void SetRequired(bool b)
    {
        m_tempInfo.m_required = b;
    }

    //Mutator for Monday
    public void SetMonday(bool b)
    {
        m_tempInfo.m_monday = b;
    }

    //Mutator for Tuesday
    public void SetTuesday(bool b)
    {
        m_tempInfo.m_tuesday = b;
    }

    //Mutator for Wednesday
    public void SetWednesday(bool b)
    {
        m_tempInfo.m_wednesday = b;
    }

    //Mutator for Thursday
    public void SetThursday(bool b)
    {
        m_tempInfo.m_thursday = b;
    }

    //Mutator for Friday
    public void SetFriday(bool b)
    {
        m_tempInfo.m_friday = b;
    }
}
