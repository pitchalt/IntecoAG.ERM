using System;
using System.Collections.Generic;
using System.Text;

namespace IntecoAG.ERM.CS
{
    /// <summary>
    /// секунды, минуты, часы, дни, недели, декады, месяцы, кварталы, годы
    /// </summary>
    public enum TimePeriod
    {
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
        Days = 4,
        Weeks = 5,
        Decades = 6,
        Months = 7,
        Quarter = 8,
        Years = 9
    }

    /// <summary>
    /// Перечень особых дат для временной шкалы
    /// </summary>
    public enum TimeSingularity
    {
        Standart = 1,
        NegativeInfinity = 2,
        PositiveInfinity = 3  //,
        //Indefinite = 4
    }

    /// <summary>
    /// Состояния для статусов версий версионируемых объектов
    /// </summary>
    public enum VersionStates {
        VERSION_NEW = 0,
        CURRENT = 1,
        VERSION_CURRENT = 2,
        VERSION_PROJECT = 3,
        VERSION_DECLINE = 4,
        VERSION_OLD = 5,
        VERSION_CONSTRACT = 6
    }

    public enum PartyRole {
        CUSTOMER = 1,
        SUPPLIER = 2
    }
    public enum ContractType {
        SIMPLE_CONTRACT = 1,
        COMPLEX_CONTRACT = 2,
        WORK_PLAN = 3
    }

    /// <summary>
    /// Кварталы
    /// </summary>
    public enum Quarter {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    /// <summary>
    /// Месяцы
    /// </summary>
    public enum Month {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

}
