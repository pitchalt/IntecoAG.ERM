#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

// ��������������� !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

//*****************************************************************************************************//
// �� ������������.
//*****************************************************************************************************//
// CS.CRM.Contract
// Time � ��������� ��� ��������� ��������������� ������� ������� ����������
// ������������� (���������� ����, ������ ������� �..��, ������������ ������� 
// Event, ������ ������������ �������)
//*****************************************************************************************************//
//
// �������������� ������������ ���� �������� �������: ���, ������, ������, ������, ��������, ����
// ����� ��������� ������ �������� ��� : ����������� �������, ����������� �������, ������������� ����
// ��. ������������ IntecoAG.ERM.CS.TimePeriod
//
//*****************************************************************************************************//

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� Time, �������������� ����� � ������� ������
    /// ����� ��� ��������: �������� ����������� ��������, ����������� ������� � ����������� ������� ���������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmTime")]
    public class Time : BaseObject
    {
        #region ������������

        public Time() : base() { }
        public Time(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }

        public Time(DateTimeExt dateTimeExt, Session ses) : this(ses) {
            AbsoluteDateTime = dateTimeExt;
        }

        public Time(DateTimeExt dateTimeStartExt, DateTimeExt dateTimeStopExt, Session ses) : this(ses) {
            DateTimeStart = dateTimeStartExt;
            DateTimeStop = dateTimeStopExt;
        }

        public Time(CS.Work.Event Event, TimeSpanExt daysRelativeEventExt, Session ses) : this(ses) {
            _Event = Event;
            EventTimeShift = daysRelativeEventExt;
        }

        #endregion


        #region ���� ������

        #endregion


        #region �������� ������

        // �������� ��� ���������� ����

        /// <summary>
        /// ���� ������� ����������
        /// </summary>
        private DateTimeExt _AbsoluteDateTime;
        public DateTimeExt AbsoluteDateTime {
            get { return _AbsoluteDateTime; }
            set {
                SetPropertyValue("AbsoluteDateTime", ref _AbsoluteDateTime, value);
            }
        }


        // �������� ��� ���������

        /// <summary>
        /// ���� ������� ���������
        /// </summary>
        private DateTimeExt _DateTimeStart;
        public DateTimeExt DateTimeStart {
            get { return _DateTimeStart; }
            set {
                SetPropertyValue("DateTimeStart", ref _DateTimeStart, value);
            }
        }

        /// <summary>
        /// ���� ������� ��������
        /// </summary>
        private DateTimeExt _DateTimeStop;
        public DateTimeExt DateTimeStop {
            get { return _DateTimeStop; }
            set {
                SetPropertyValue("DateTimeStop", ref _DateTimeStop, value);
            }
        }


        // �������� ��� ������� ������������ ������� Event

        /// <summary>
        /// �������
        /// </summary>
        private CS.Work.Event _Event;
        public CS.Work.Event Event {
            get { return _Event; }
            set {
                SetPropertyValue("Event", ref _Event, value);
            }
        }


        // ����� �� ������� ������������ ������� Event

        /// <summary>
        /// ����� �� ������� ������������ ������� Event. ���� Event ���� � ���� ����������� �����
        /// </summary>
        private TimeSpanExt _EventTimeShift;
        public TimeSpanExt EventTimeShift {
            get { return _EventTimeShift; }
            set {
                SetPropertyValue("EventTimeShift", ref _EventTimeShift, value);
            }
        }

        // ����������������� �������

        /// <summary>
        ///  ����������������� �������, �������������� �������
        /// </summary>
        private TimeSpanExt _PeriodDuration;
        public TimeSpanExt PeriodDuration {
            get { return _PeriodDuration; }
            set {
                SetPropertyValue("PeriodDuration", ref _PeriodDuration, value);
            }
        }

        #endregion


        #region ������ ������

        #endregion

    }


    /// <summary>
    /// ����� ������������ ����� ���������� ���� ���������� ���������� �������, ���������� ������� ����, ������������� ����. 
    /// ������� ���� ���������� ��� �����������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmDateTimeExt")]
    public class DateTimeExt : BaseObject
    {
        #region ������������
        
        public DateTimeExt() : base() { }
        public DateTimeExt(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }

        public DateTimeExt(CS.TimeSingularity timeSingularity, Session ses)
            : this(ses) { 
            TimeSingularity = timeSingularity;
        }

        public DateTimeExt(DateTime datetime, Session ses)
            : this(ses) {
            DateTime = datetime;
        }

        #endregion


        #region ����������

        #endregion


        #region �������� ������

        private DateTime _DateTime;
        /// <summary>
        /// ���� � �����
        /// </summary>
        public DateTime DateTime {
            get { return _DateTime; }
            set {
                SetPropertyValue("DateTime", ref _DateTime, value);
                SetTimeSingularity(value);
            }
        }

        private CS.TimeSingularity _TimeSingularity;
        /// <summary>
        /// ������������� ������ (������������� � �.�.)
        /// </summary>
        public CS.TimeSingularity TimeSingularity {
            get { return _TimeSingularity; }
            set {
                SetPropertyValue("TimeSingularity", ref _TimeSingularity, value);
                SetDateForTimeSingularity(value);
            }
        }

        #endregion

        #region ������: ����������� � ���� � �������� ������������� ����������

        /// <summary>
        /// ������������ ��������� ����������� ���� � ����������� �� � ��������
        /// </summary>
        /// <param name="datetime"></param>
        private void SetTimeSingularity(DateTime datetime) {
            if (datetime == DateTime.MinValue) {
                _TimeSingularity = CS.TimeSingularity.NegativeInfinity;
            }
            else if (datetime == DateTime.MaxValue) {
                _TimeSingularity = CS.TimeSingularity.PositiveInfinity;
            }
            else {
                _TimeSingularity = CS.TimeSingularity.Standart;
            }
        }

        /// <summary>
        /// ������������ ���� � ����������� �� � �����������
        /// </summary>
        /// <param name="datetime"></param>
        private void SetDateForTimeSingularity(CS.TimeSingularity timeSingularity) {
            if (timeSingularity == CS.TimeSingularity.NegativeInfinity) _DateTime = DateTime.MinValue;
            if (timeSingularity == CS.TimeSingularity.PositiveInfinity) _DateTime = DateTime.MaxValue;
        }

        /// <summary>
        /// ���� ������ ������ �������� ����������
        /// </summary>
        /// <param name="dateTimeExt"></param>
        /// <returns></returns>
        public bool IsGreater(DateTimeExt dateTimeExt) {
            switch (_TimeSingularity) {
                case CS.TimeSingularity.NegativeInfinity:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                        case CS.TimeSingularity.PositiveInfinity:
                        case CS.TimeSingularity.Standart:
                            return false;
                    }
                    break;
                case CS.TimeSingularity.PositiveInfinity:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                            return true;
                        case CS.TimeSingularity.PositiveInfinity:
                            return false;
                        case CS.TimeSingularity.Standart:
                            return true;
                    }
                    break;
                case CS.TimeSingularity.Standart:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                            return true;
                        case CS.TimeSingularity.PositiveInfinity:
                            return false;
                        case CS.TimeSingularity.Standart:
                            return this.DateTime > dateTimeExt.DateTime;
                    }
                    break;
            }
            return false;
        }


        /// <summary>
        /// ���� ������ ������ �������� ����������
        /// </summary>
        /// <param name="dateTimeExt"></param>
        /// <returns></returns>
        public bool IsSmaller(DateTimeExt dateTimeExt) {
            switch (_TimeSingularity) {
                case CS.TimeSingularity.NegativeInfinity:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                            return false;
                        case CS.TimeSingularity.PositiveInfinity:
                            return true;
                        case CS.TimeSingularity.Standart:
                            return true;
                    }
                    break;
                case CS.TimeSingularity.PositiveInfinity:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                        case CS.TimeSingularity.PositiveInfinity:
                        case CS.TimeSingularity.Standart:
                            return false;
                    }
                    break;
                case CS.TimeSingularity.Standart:
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity:
                            return true;
                        case CS.TimeSingularity.PositiveInfinity:
                            return false;
                        case CS.TimeSingularity.Standart:
                            return this.DateTime < dateTimeExt.DateTime;
                    }
                    break;
            }
            return false;
        }


        // ����������. "������ ��� �����" � "������ ��� �����" �� �������� � ���������� ������, ������ � ����� (��� ������������� ��������)

        /// <summary>
        /// ���� ������ ������ ��� ����� �������� ����������
        /// </summary>
        /// <param name="dateTimeExt"></param>
        /// <returns></returns>
        public bool IsGreaterOrEqual(DateTimeExt dateTimeExt) {
            switch (_TimeSingularity) {
                default:
                    switch (dateTimeExt.TimeSingularity) {
                        default:
                            return IsGreater(dateTimeExt) | (this == dateTimeExt);
                    }
            }
        }



        /// <summary>
        /// ���� ������ ������ ��� ����� �������� ����������
        /// </summary>
        /// <param name="dateTimeExt"></param>
        /// <returns></returns>
        public bool IsSmallerOrEqual(DateTimeExt dateTimeExt) {
            switch (_TimeSingularity) {
                //case CS.TimeSingularity.Indefinite:
                //            return false;
                default :
                    switch (dateTimeExt.TimeSingularity) {
                        //case CS.TimeSingularity.Indefinite:
                        //    return false;
                        default:
                            return IsSmaller(dateTimeExt) | (this == dateTimeExt);
                    }
            }
        }


        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string Res = "";

            if (TimeSingularity == CS.TimeSingularity.Standart) {
                Res += DateTime.ToString();
            }
            else if (TimeSingularity == CS.TimeSingularity.NegativeInfinity) {
                Res += "-Infinity";
            }
            else if (TimeSingularity == CS.TimeSingularity.PositiveInfinity) {
                Res += "+Infinity";
            }
            else {
                Res += "";
            }

            return Res;
        }

        #endregion

    }

    /// <summary>
    /// ��������� ������������� ��������, ������� ����� ���������� � ������� ������ Time
    /// ����� ��������� TimeSpan � ������ ��� ��������� �� ��� ����, � ������� �� ����� �����������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmTimeSpanExt")]
    public class TimeSpanExt : BaseObject
    {
        #region ������������
        
        public TimeSpanExt() : base() { }
        public TimeSpanExt(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }

        public TimeSpanExt(CS.TimeSingularity timeSingularity, Session ses)
            : this(ses) { 
            TimeSingularity = timeSingularity;
            Year = Quarter = Month = Day = Hour = Minute = Second = 0;
        }


        public TimeSpanExt(int year, int quarter, int month, int day, int hour, int minute, int second, Session ses)
            : this(ses) {
            Year = year;
            Quarter = quarter;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;

            _Duration = new TimeSpan(day, hour, minute, second);
            TimeSingularity = CS.TimeSingularity.Standart;
        }


        public TimeSpanExt(int quarter, int month, int day, int hour, int minute, int second, Session ses)
            : this(0, quarter, month, day, hour, minute, second, ses) {
        }

        public TimeSpanExt(int month, int day, int hour, int minute, int second, Session ses)
            : this(0, month, day, hour, minute, second, ses) {
        }

        public TimeSpanExt(int day, int hour, int minute, int second, Session ses)
            : this(0, day, hour, minute, second, ses) {
        }

        public TimeSpanExt(int hour, int minute, int second, Session ses)
            : this(0, hour, minute, second, ses) {
        }

        public TimeSpanExt(int minute, int second, Session ses)
            : this(0, minute, second, ses) {
        }

        public TimeSpanExt(int second, Session ses)
            : this(0, second, ses) {
        }

        #endregion


        #region ����������

        private TimeSpan _Duration;

        #endregion


        #region �������� ������

        /*
        [PersistentAlias("ToString()")]
        public string UserFriendView {
            get {
                object tempObject = EvaluateAlias("UserFriendView");
                if (tempObject != null) {
                    return (string)tempObject;
                }
                else {
                    return "";
                }
            }
        }
        */


        private string _UserFriendView;
        /// <summary>
        /// UserFriendView
        /// </summary>
        public string UserFriendView {
            get { return _UserFriendView; }
            set {
                SetPropertyValue("UserFriendView", ref _UserFriendView, value);
            }
        }


        private int _Second;
        /// <summary>
        /// �������
        /// </summary>
        public int Second {
            get { return _Second; }
            set {
                SetPropertyValue("Second", ref _Second, value);
            }
        }

        private int _Minute;
        /// <summary>
        /// ������
        /// </summary>
        public int Minute {
            get { return _Minute; }
            set {
                SetPropertyValue("Minute", ref _Minute, value);
            }
        }

        private int _Hour;
        /// <summary>
        /// ����
        /// </summary>
        public int Hour {
            get { return _Hour; }
            set {
                SetPropertyValue("Hour", ref _Hour, value);
            }
        }

        private int _Day;
        /// <summary>
        /// ���
        /// </summary>
        public int Day {
            get { return _Day; }
            set {
                SetPropertyValue("Day", ref _Day, value);
            }
        }

        private int _Month;
        /// <summary>
        /// �����
        /// </summary>
        public int Month {
            get { return _Month; }
            set {
                SetPropertyValue("Month", ref _Month, value);
            }
        }

        private int _Quarter;
        /// <summary>
        /// �������
        /// </summary>
        public int Quarter {
            get { return _Quarter; }
            set {
                SetPropertyValue("Quarter", ref _Quarter, value);
            }
        }

        private int _Year;
        /// <summary>
        /// ���
        /// </summary>
        public int Year {
            get { return _Year; }
            set {
                SetPropertyValue("Year", ref _Year, value);
            }
        }

        private CS.TimeSingularity _TimeSingularity;
        /// <summary>
        /// ������������� ������ (������������� � �.�.)
        /// </summary>
        public CS.TimeSingularity TimeSingularity {
            get { return _TimeSingularity; }
            set {
                if (SetPropertyValue("TimeSingularity", ref _TimeSingularity, value)) {
                    if (_TimeSingularity != CS.TimeSingularity.Standart)
                    Year = Quarter = Month = Day = Hour = Minute = Second = 0;
                }
            }
        }

        #endregion


        #region ������: ����������� � ���� � �������� ������������� ����������

        protected override void OnSaving() {
            base.OnSaving();
            _UserFriendView = this.ToString();
        }


        // ��������� ����������� ��������� � ���� ����� ������� �� ����� ����, ��������, ����������� � 29 ������� ����������� ���� ������ ���� ���� 28 ������� � �.�.
        // ���������� � ������������ ��������� � �����. ������� ������� �� ������������ �������� ��� � .NET

        public void Add(ref DateTimeExt dateTimeExt) {
            switch (_TimeSingularity) {
                case CS.TimeSingularity.NegativeInfinity :
                case CS.TimeSingularity.PositiveInfinity :
                    dateTimeExt.TimeSingularity = _TimeSingularity;
                    break;
                case CS.TimeSingularity.Standart :
                    switch (dateTimeExt.TimeSingularity) {
                        case CS.TimeSingularity.NegativeInfinity :
                        case CS.TimeSingularity.PositiveInfinity :
                            break;
                        case CS.TimeSingularity.Standart :
                            dateTimeExt.DateTime.AddYears(_Year);
                            dateTimeExt.DateTime.AddMonths(_Month);
                            dateTimeExt.DateTime.AddMonths(_Quarter * 3);
                            dateTimeExt.DateTime.AddDays(_Day);
                            dateTimeExt.DateTime.Add(_Duration);
                            break;
                    }
                    break;
            }
        }


        private string Delimiter = "_";


        public string Pack() {
            string Res = "";

            Res = this.Second.ToString() + Delimiter;
            Res = this.Minute.ToString() + Delimiter;
            Res = this.Hour.ToString() + Delimiter;
            Res = this.Day.ToString() + Delimiter;
            Res = this.Month.ToString() + Delimiter;
            Res = this.Quarter.ToString() + Delimiter;
            Res = this.Year.ToString() + Delimiter;

            Res = this.TimeSingularity.ToString();

            return Res;
        }


        public TimeSpanExt UnPack(string packedTimeSpanExt) {
            TimeSpanExt timeSpanExt = new TimeSpanExt(this.Session);

            string[] separator = { Delimiter };

            try {
                string[] mTime = packedTimeSpanExt.Split(separator, StringSplitOptions.None);

                timeSpanExt.Second = Convert.ToInt32(mTime[0]);
                timeSpanExt.Minute = Convert.ToInt32(mTime[1]);
                timeSpanExt.Hour = Convert.ToInt32(mTime[2]);
                timeSpanExt.Day = Convert.ToInt32(mTime[3]);
                timeSpanExt.Month = Convert.ToInt32(mTime[4]);
                timeSpanExt.Quarter = Convert.ToInt32(mTime[5]);
                timeSpanExt.Year = Convert.ToInt32(mTime[6]);

                if (mTime[7] == CS.TimeSingularity.Standart.ToString()) {
                    timeSpanExt.TimeSingularity = CS.TimeSingularity.Standart;
                }
                else if (mTime[7] == CS.TimeSingularity.NegativeInfinity.ToString()) {
                    timeSpanExt.TimeSingularity = CS.TimeSingularity.NegativeInfinity;
                }
                else if (mTime[7] == CS.TimeSingularity.PositiveInfinity.ToString()) {
                    timeSpanExt.TimeSingularity = CS.TimeSingularity.PositiveInfinity;
                }
                else {
                    throw new Exception("Wrong format of TimeSpanExt singularity property");
                }
            }
            catch (Exception ex) {
                throw new Exception("Error unpack TimeSpanExt structure", ex);
            }

            return timeSpanExt;
        }

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string Res = "";

            if (TimeSingularity == CS.TimeSingularity.Standart) {
                Res += TimeSpanElementToString(Year, "year", Res);
                Res += TimeSpanElementToString(Quarter, "quarter", Res);
                Res += TimeSpanElementToString(Month, "month", Res);
                Res += TimeSpanElementToString(Day, "day", Res);
                Res += TimeSpanElementToString(Hour, "hour", Res);
                Res += TimeSpanElementToString(Minute, "minute", Res);
                Res += TimeSpanElementToString(Second, "second", Res);
            }
            else if (TimeSingularity == CS.TimeSingularity.NegativeInfinity) {
                Res += "-Infinity";
            }
            else if (TimeSingularity == CS.TimeSingularity.PositiveInfinity) {
                Res += "+Infinity";
            }
            else {
                Res += "";
            }

            return Res;
        }

        private string TimeSpanElementToString(int timeElem, string timeElemName, string res) {
            return ((timeElem == 0) ? "" : ( ((string.IsNullOrEmpty(res)) ? "" : ", ") + timeElem.ToString() + " " + timeElemName + ((timeElem > 1) ? "s" : "") ) );
        }


        #endregion
    }
}
