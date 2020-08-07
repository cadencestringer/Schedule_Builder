using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Info : MonoBehaviour, IComparable<Info>
{
    //Instance Variables
    public bool m_required;
    public bool m_monday;
    public bool m_tuesday;
    public bool m_wednesday;
    public bool m_thursday;
    public bool m_friday;
    public int m_section;
    public int m_start;
    public int m_end;
    public int m_startHour;
    public int m_startMinute;
    public int m_endHour;
    public int m_endMinute;
    public string m_startAMPM;
    public string m_endAMPM;
    public string m_className;
    public string m_teacherName;
     //Variables for displaying info on screen
    public Text m_cardName;
    public Text m_cardSection;
    public Text m_cardRequired;     public Text m_cardTeacher;     public Text m_cardTime;     public Text m_cardDays;
     //Runs on start
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
     //Sets info on the card for input page
    public void SetCard()
    {
        m_cardName.text = m_className;
        m_cardSection.text = "Section: " + m_section;
        if (m_required)
        {
            m_cardRequired.text = "Required: Yes";
        }
        else
        {
            m_cardRequired.text = "Required: No";
        }
    }      //Sets info on the card for schedule page     public void SetFullCard()
    {         m_cardName.text = m_className;         m_cardSection.text = "Section: " + m_section;         if (m_required)         {             m_cardRequired.text = "Required: Yes";         }         else         {             m_cardRequired.text = "Required: No";         }         m_cardTeacher.text = "Teacher: " + m_teacherName;          String sh = "";         String sm = "";         String eh = "";         String em = "";          if (m_startHour == 0)
        {
            sh += "00";
        }         else
        {
            sh += m_startHour;
        }

        if (m_startMinute == 0)         {             sm += "00";         }         else         {             sm += m_startMinute;         } 
        if (m_endHour == 0)         {             eh += "00";         }         else         {             eh += m_endHour;         } 
        if (m_endMinute == 0)         {             em += "00";         }         else         {             em += m_endMinute;         }          m_cardTime.text = "Time: " + sh + ":" + sm + " " + m_startAMPM + " to " + eh + ":" + em + " " + m_endAMPM;          String days = "";          if (m_monday == true)
        {
            days += "M. ";
        }         if (m_tuesday == true)
        {
            days += "T. ";
        }
        if (m_wednesday == true)         {             days += "W. ";         }
        if (m_thursday == true)         {             days += "TH. ";         }
        if (m_friday == true)         {             days += "F. ";         }         m_cardDays.text = "Days: " + days;
    }
     //Compares the start times of two Info objects     public int CompareTo(Info i)
    {         int ret = 0;         if (m_start > i.m_start)
        {
            ret = 1;
        }         else if (m_start < i.m_start)
        {
            ret = -1;
        }         else
        {
            ret = 0;
        }
        return ret;
    }      //Resets Info attributes
    public void Reset()
    {
        m_required = false;
        m_monday = false;
        m_tuesday = false;
        m_wednesday = false;
        m_thursday = false;
        m_friday = false;
        m_section = 0;
        m_start = 0;
        m_end = 0;
        m_startHour = 0;
        m_startMinute = 0;
        m_endHour = 0;
        m_endMinute = 0;
        m_startAMPM = "";
        m_endAMPM = "";
        m_className = "";
        m_teacherName = "";
    }
     //Copies the Info attributes from one Info object to another
    public static void Copy(Info f, Info t)
    {
        t.m_required = f.m_required;
        t.m_monday = f.m_monday;
        t.m_tuesday = f.m_tuesday;
        t.m_wednesday = f.m_wednesday;
        t.m_thursday = f.m_thursday;
        t.m_friday = f.m_friday;
        t.m_section = f.m_section;
        t.m_start = f.m_start;
        t.m_end = f.m_end;
        t.m_startHour = f.m_startHour;
        t.m_startMinute = f.m_startMinute;
        t.m_endHour = f.m_endHour;
        t.m_endMinute = f.m_endMinute;
        t.m_startAMPM = f.m_startAMPM;
        t.m_endAMPM = f.m_endAMPM;
        t.m_className = f.m_className;
        t.m_teacherName = f.m_teacherName;
    }
     //Returns true if the compared Info objects are identical
    public bool Equals(Info i)
    {
        return m_required == i.m_required && m_monday == i.m_monday && m_tuesday == i.m_tuesday && m_wednesday == i.m_wednesday && m_thursday == i.m_thursday && m_friday == i.m_friday && m_start == i.m_start && m_end == i.m_end && m_startHour == i.m_startHour && m_startMinute == i.m_startMinute && m_endHour == i.m_endHour && m_endMinute == i.m_endMinute && m_startAMPM.Equals(i.m_startAMPM) && m_endAMPM.Equals(i.m_endAMPM) && m_className.Equals(i.m_className) && m_teacherName.Equals(i.m_teacherName);
    }      //Returns true if any of the days of the compared Info objects is the same     public bool DaysEqual(Info i)
    {
        return m_monday == i.m_monday || m_tuesday == i.m_tuesday || m_wednesday == i.m_wednesday || m_thursday == i.m_thursday || m_friday == i.m_friday;
    }
}
