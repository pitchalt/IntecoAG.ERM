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
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalEditorState;

namespace IntecoAG.ERM.CRM.Contract
{
    // http://documentation.devexpress.com/#Xaf/CustomDocument3221 - ��� EditorStateRuleAttribute


    //    [EditorStateRuleAttribute("TimeSpanExtIsInfinity", "ExtendedTimeSpanString", EditorState.Disabled,
    //"WorkTimeSingularity == CS.TimeSingularity.NegativeInfinity", ViewType.DetailView)]


    //    [EditorStateRuleAttribute("TimeSpanExtIsInfinity", "ExtendedTimeSpanString", EditorState.Disabled,
    //"IsNullOrEmpty(AAA)", ViewType.DetailView)]


    /// <summary>
    /// ����� Contract, �������������� ������ ��������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmTimeTest")]
    [EditorStateRuleAttribute("TimeSpanExtIsInfinity", "ExtendedTimeSpanString", EditorState.Disabled,
    "WorkTimeSingularity == 2 | WorkTimeSingularity == 3", ViewType.DetailView)] // 2 == IntecoAG.ERM.CS.TimeSingularity.NegativeInfinity, 3 = IntecoAG.ERM.CS.TimeSingularity.PositiveInfinity
    [EditorStateRuleAttribute("DateTimeExtIsInfinity", "WorkDateTime", EditorState.Disabled,
    "WorkDateTimeSingularity == 2 | WorkDateTimeSingularity == 3", ViewType.DetailView)] // 2 == IntecoAG.ERM.CS.TimeSingularity.NegativeInfinity, 3 = IntecoAG.ERM.CS.TimeSingularity.PositiveInfinity
    public partial class TimeTest : BaseObject
    {
        public TimeTest() : base() { }
        public TimeTest(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            _WorkTimeSingularity = CS.TimeSingularity.Standart;
            _WorkDateTimeSingularity = CS.TimeSingularity.Standart;
        }


        #region �������� ������


/*
        #region ����������� TimeSpan

        /// <summary>
        /// TimeSpan
        /// </summary>
        private TimeSpan _SimpleTimeSpan;
        public TimeSpan SimpleTimeSpan {
            get { return _SimpleTimeSpan; }
            set { SetPropertyValue("SimpleTimeSpan", ref _SimpleTimeSpan, value); }
        }

        #endregion
*/


        #region �������, ����������� �������� ����� �������� ���� ����

        /// <summary>
        /// �������
        /// </summary>
        private CRM.Contract.crmEvent _WorkEvent;
        public CRM.Contract.crmEvent WorkEvent {
            get { return _WorkEvent; }
            set { SetPropertyValue("WorkEvent", ref _WorkEvent, value); }
        }

        #endregion



        #region ��� ������������ Time (��������� ����, �������� ���, ������� ����������� �������, ������� ����� ���������, ������� ����������� �������)

        /// <summary>
        /// �������� ��������� �������� CRM.Contract.Time
        /// </summary>
        private CRM.Contract.Time _WorkTime;
        public CRM.Contract.Time WorkTime {
            get { return _WorkTime; }
            set { SetPropertyValue("WorkTime", ref _WorkTime, value); }
        }


        // ��������� ���� �������� ���� (DateTimeExt ��. ����), ������ � ��� ���������� ���, ��� ����.
        
        // ����� � � ���������� ���, ��������� �� ������� �� ���� ������������ ��� ���� �� ����; ��� ���c���� ���������� ������ 
        // � ��������� �����

        // ������ �� ������� � ������ ��������� ��� ������ ���������� �����, ��� � ���� ��� ������ � TimeSpanExt.

        // ��� ������������� ������ ������� TimeSpanExt ���������� ��� ������ � ��������� ���.
 
        // 

        #endregion


        #region ��� ������������ DateTimeExt (������� ������ ��� ����� ���������)

        /// <summary>
        /// �������� ��������� �������� CRM.Contract.DateTimeExt
        /// </summary>
        private CRM.Contract.DateTimeExt _WorkDateTimeExt;
        public CRM.Contract.DateTimeExt WorkDateTimeExt {
            get { return _WorkDateTimeExt; }
            set { SetPropertyValue("WorkDateTimeExt", ref _WorkDateTimeExt, value); }
        }


        // ��������� �� ��������� ������������

        /// <summary>
        /// DateTime ��� DateTimeExt
        /// </summary>
        private DateTime _WorkDateTime;
        public DateTime WorkDateTime {
            get { return _WorkDateTime; }
            set { SetPropertyValue("WorkDateTime", ref _WorkDateTime, value); }
        }

        private CS.TimeSingularity _WorkDateTimeSingularity;
        /// <summary>
        /// ���� ���� �������� ��������������� � ����� ��������, �������� �� Standart, �� ���� ExtendedTimeSpanString �����������
        /// </summary>
        [ImmediatePostData]
        public CS.TimeSingularity WorkDateTimeSingularity {
            get { return _WorkDateTimeSingularity; }
            set { SetPropertyValue("WorkDateTimeSingularity", ref _WorkDateTimeSingularity, value); }
        }
        
        #endregion


        #region ��� ������������ TimeSpanExt (������� ������ ��� ����� ���������)

        /// <summary>
        /// �������
        /// </summary>
        private string _ExtendedTimeSpanString;
        public string ExtendedTimeSpanString {
            get { return _ExtendedTimeSpanString; }
            set { SetPropertyValue("ExtendedTimeSpanString", ref _ExtendedTimeSpanString, value); }
        }

        private CS.TimeSingularity _WorkTimeSingularity;
        /// <summary>
        /// ���� ���� �������� ��������������� � ����� ��������, �������� �� Standart, �� ���� ExtendedTimeSpanString �����������
        /// </summary>
        [ImmediatePostData]
        public CS.TimeSingularity WorkTimeSingularity {
            get { return _WorkTimeSingularity; }
            set { SetPropertyValue("WorkTimeSingularity", ref _WorkTimeSingularity, value); }
        }

        /// <summary>
        /// �������� ��������� �������� CRM.Contract.TimeSpanExt
        /// </summary>
        private CRM.Contract.TimeSpanExt _WorkTimeSpanExt;
        public CRM.Contract.TimeSpanExt WorkTimeSpanExt {
            get { return _WorkTimeSpanExt; }
            set { SetPropertyValue("WorkTimeSpanExt", ref _WorkTimeSpanExt, value); }
        }

        #endregion


        #endregion


        #region ������

        protected override void OnSaving() {
            base.OnSaving();
            WorkDateTimeExt = SetDateTimeExt();
            WorkTimeSpanExt = ConvertFormatedTimeSpanExtString(ExtendedTimeSpanString);
        }

        /// <summary>
        /// ������ WorkDateTimeExt �� ��������������� �����
        /// </summary>
        /// <param name="formatedTimeSpanExt"></param>
        /// <returns></returns>
        public DateTimeExt SetDateTimeExt() {
            DateTimeExt dateTimeExt = new DateTimeExt(this.Session);

            try {
                // �������������
                dateTimeExt.TimeSingularity = WorkDateTimeSingularity;

                if (WorkDateTimeSingularity == CS.TimeSingularity.Standart) {
                    dateTimeExt.DateTime = WorkDateTime;
                }
            }
            catch (Exception ex) {
                throw new Exception("Error parsing TimeSpanExt formated string", ex);
            }

            return dateTimeExt;
        }

        /// <summary>
        /// ������ TimeSpanExt �� ��������������� �����
        /// </summary>
        /// <param name="formatedTimeSpanExt"></param>
        /// <returns></returns>
        public TimeSpanExt ConvertFormatedTimeSpanExtString(string formatedTimeSpanExt) {
            TimeSpanExt timeSpanExt = new TimeSpanExt(this.Session);

            try {
                // �������������
                timeSpanExt.TimeSingularity = WorkTimeSingularity;

                if (WorkTimeSingularity == CS.TimeSingularity.Standart) {
                    string Delimiter = " ";
                    string TimeDelimiter = ":";

                    string[] separator = { Delimiter };
                    string[] timeSeparator = { TimeDelimiter };

                    string[] mTimeExt = formatedTimeSpanExt.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    string[] mTime = mTimeExt[6].Split(timeSeparator, StringSplitOptions.RemoveEmptyEntries);

                    timeSpanExt.Second = 0;
                    timeSpanExt.Minute = Convert.ToInt32(mTime[1]);
                    timeSpanExt.Hour = Convert.ToInt32(mTime[0]);

                    timeSpanExt.Day = Convert.ToInt32(mTimeExt[4]);
                    timeSpanExt.Month = Convert.ToInt32(mTimeExt[2]);
                    timeSpanExt.Quarter = 0;
                    timeSpanExt.Year = Convert.ToInt32(mTimeExt[0]);
                }
                else {
                    timeSpanExt.Second = 0;
                    timeSpanExt.Minute = 0;
                    timeSpanExt.Hour = 0;
                    timeSpanExt.Day = 0;
                    timeSpanExt.Month = 0;
                    timeSpanExt.Quarter = 0;
                    timeSpanExt.Year = 0;
                }
            }
            catch (Exception ex) {
                throw new Exception("Error parsing TimeSpanExt formated string", ex);
            }

            return timeSpanExt;
        }


        #endregion

    }

}