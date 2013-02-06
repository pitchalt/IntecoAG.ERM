using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors;
using System.ComponentModel;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils;
using DevExpress.Data.Mask;
using System.Globalization;
using DevExpress.XtraEditors.Mask;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;

namespace TimeSpanEditor
{
    //[DetailViewItem(typeof(Object))]
    [UserRepositoryItem("RegisterTimeSpanEdit")]
    public class RepositoryItemTimeSpanEdit : RepositoryItemTimeEdit
    {
        bool allowDayInput;
        static RepositoryItemTimeSpanEdit() { RegisterTimeSpanEdit(); }
        public RepositoryItemTimeSpanEdit() {
            allowDayInput = false;
            UpdateFormats();
        }
        public const string TimeSpanEditName = "TimeSpanEdit";
        public override string EditorTypeName { get { return TimeSpanEditName; } }
        public static void RegisterTimeSpanEdit() {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(TimeSpanEditName,
              typeof(TimeSpanEdit), typeof(RepositoryItemTimeSpanEdit),
              typeof(BaseSpinEditViewInfo), new ButtonEditPainter(), true));
        }
        [Browsable(false)]
        public override FormatInfo EditFormat { get { return base.EditFormat; } }
        [Browsable(false)]
        public override FormatInfo DisplayFormat { get { return base.DisplayFormat; } }
        [Browsable(false)]
        public override MaskProperties Mask { get { return base.Mask; } }
        [Browsable(false)]
        public new virtual string EditMask {
            get {
                string mask = "HH:mm:ss";
                if (AllowDayInput) mask = "d." + mask;
                return mask;
            }
        }
        protected internal virtual char TimeSeparator { get { return TimeSpanHelper.TimeSeparator; } }
        protected internal virtual char DaySeparator { get { return TimeSpanHelper.DaySeparator; } }
        [Category(CategoryName.Behavior), DefaultValue(false)]
        public virtual bool AllowDayInput {
            get { return allowDayInput; }
            set {
                if (allowDayInput == value) return;
                allowDayInput = value;
                UpdateFormats();
            }
        }
        protected virtual void UpdateFormats() {
            EditFormat.FormatString = EditMask;
            DisplayFormat.FormatString = EditMask;
            Mask.EditMask = EditMask;
        }
        public override void Assign(RepositoryItem item) {
            BeginUpdate();
            try {
                base.Assign(item);
                RepositoryItemTimeSpanEdit source = item as RepositoryItemTimeSpanEdit;
                if (source == null) return;
                this.AllowDayInput = source.AllowDayInput;
            }
            finally {
                EndUpdate();
            }
        }

        public override string GetDisplayText(FormatInfo format, object editValue) {
            if (editValue is TimeSpan)
                return TimeSpanHelper.TimeSpanToString(((TimeSpan)editValue), AllowDayInput);
            if (editValue is string)
                return editValue.ToString();
            return GetDisplayText(null, new TimeSpan(0));
        }

        protected internal virtual string GetFormatMaskAccessFunction(string editMask, CultureInfo managerCultureInfo) {
            return GetFormatMask(editMask, managerCultureInfo);
        }
    }

    public class TimeSpanEdit : TimeEdit
    {
        static TimeSpanEdit() { RepositoryItemTimeSpanEdit.RegisterTimeSpanEdit(); }
        public TimeSpanEdit()
            : base() {
            this.fOldEditValue = this.fEditValue = new TimeSpan(0);
        }
        public override string EditorTypeName { get { return RepositoryItemTimeSpanEdit.TimeSpanEditName; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemTimeSpanEdit Properties {
            get {
                return base.Properties as
                    RepositoryItemTimeSpanEdit;
            }
        }
        public override object EditValue {
            get {
                if (Properties.ExportMode == ExportMode.DisplayText)
                    return Properties.GetDisplayText(null, base.EditValue);
                return base.EditValue;
            }
            set {
                if (value is DateTime) {
                    DateTime time = ((DateTime)value);
                    base.EditValue = new TimeSpan(time.Ticks);

                }
                else if (value is TimeSpan)
                    base.EditValue = value;
                else if (value is string)
                    base.EditValue = TimeSpanHelper.Parse((string)value);
                else
                    base.EditValue = new TimeSpan(0, 0, 0);
            }
        }


        protected override MaskManager CreateMaskManager(MaskProperties mask) {
            CustomTimeEditMaskProperties patchedMask = new CustomTimeEditMaskProperties();
            patchedMask.Assign(mask);
            patchedMask.EditMask = Properties.GetFormatMaskAccessFunction(mask.EditMask, mask.Culture);
            return patchedMask.CreatePatchedMaskManager();
        }
    }
    public class CustomTimeEditMaskProperties : TimeEditMaskProperties
    {
        public CustomTimeEditMaskProperties() : base() { }
        public virtual MaskManager CreatePatchedMaskManager() {
            CultureInfo managerCultureInfo = this.Culture;
            if (managerCultureInfo == null)
                managerCultureInfo = CultureInfo.CurrentCulture;
            string editMask = this.EditMask;
            if (editMask == null)
                editMask = string.Empty;
            return new CustomDateTimeMaskManager(editMask, false, managerCultureInfo, true);
        }
    }

    public class CustomDateTimeMaskManager : DateTimeMaskManager
    {
        public CustomDateTimeMaskManager(string mask, bool isOperatorMask, CultureInfo culture, bool allowNull)
            : base(mask, isOperatorMask, culture, allowNull) {
            fFormatInfo = new CustomDateTimeMaskFormatInfo(mask, this.fInitialDateTimeFormatInfo);
        }
        public override void SetInitialEditText(string initialEditText) {
            KillCurrentElementEditor();
            DateTime? initialEditValue = DateTime.MinValue;
            if (!string.IsNullOrEmpty(initialEditText)) {
                try {
                    initialEditValue = new DateTime(TimeSpanHelper.Parse(initialEditText).Ticks);
                }
                catch { }
            }
            SetInitialEditValue(initialEditValue);
        }
    }
    public class CustomDateTimeMaskFormatInfo : DateTimeMaskFormatInfo
    {
        public CustomDateTimeMaskFormatInfo(string mask, DateTimeFormatInfo dateTimeFormatInfo)
            : base(mask, dateTimeFormatInfo) {
            for (int i = 0; i < Count; i++) {
                if (innerList[i] is DateTimeMaskFormatElement_d || innerList[i] is DateTimeMaskFormatElement_d) {
                    innerList[i] = new DateTimeMaskFormatElement_Dxxx("H", dateTimeFormatInfo);
                    return;
                }
                if (innerList[i] is DateTimeMaskFormatElement_H24 || innerList[i] is DateTimeMaskFormatElement_h12) {
                    innerList[i] = new DateTimeMaskFormatElement_Hxxx("H", dateTimeFormatInfo);
                    return;
                }
            }
        }
    }

    public class DateTimeMaskFormatElement_Hxxx : DateTimeNumericRangeFormatElementEditable
    {
        public DateTimeMaskFormatElement_Hxxx(string mask, DateTimeFormatInfo dateTimeFormatInfo)
            : base(mask,
                dateTimeFormatInfo, DateTimePart.Time) { }
        public override DateTimeElementEditor CreateElementEditor(DateTime editedDateTime) {
            return new DateTimeNumericRangeElementEditor(GetHours(editedDateTime), 0, 24000000, 1, 9);
        }
        public override DateTime ApplyElement(int result, DateTime editedDateTime) {
            TimeSpan value = new TimeSpan(result, editedDateTime.Minute, editedDateTime.Second);
            return new DateTime(value.Ticks);
        }
        public override string Format(DateTime formattedDateTime) {
            return GetHours(formattedDateTime).ToString();
        }
        protected virtual int GetHours(DateTime dt) {
            TimeSpan internalValue = new TimeSpan(dt.Ticks);
            return System.Convert.ToInt32(Math.Floor(internalValue.TotalHours));
        }
    }

    public class DateTimeMaskFormatElement_Dxxx : DateTimeNumericRangeFormatElementEditable
    {
        public DateTimeMaskFormatElement_Dxxx(string mask, DateTimeFormatInfo dateTimeFormatInfo)
            : base(mask,
                dateTimeFormatInfo, DateTimePart.Time) { }
        public override DateTimeElementEditor CreateElementEditor(DateTime editedDateTime) {
            TimeSpan internalValue = new TimeSpan(editedDateTime.Ticks);
            return new DateTimeNumericRangeElementEditor(internalValue.Days, 0, 1000000, 1, 7);
        }
        public override DateTime ApplyElement(int result, DateTime editedDateTime) {
            TimeSpan internalValue = new TimeSpan(result, editedDateTime.Hour, editedDateTime.Minute, editedDateTime.Second);
            return new DateTime(internalValue.Ticks);
        }
        public override string Format(DateTime formattedDateTime) {
            TimeSpan internalValue = new TimeSpan(formattedDateTime.Ticks);
            return internalValue.Days.ToString();
        }
    }
    public class TimeSpanHelper
    {
        const char timeSeparator = ':';
        const char daySeparator = '.';
        public TimeSpanHelper() { }
        public static TimeSpan Parse(string str) {
            TimeSpan ts;
            try { ts = TimeSpan.Parse(str); }
            catch (System.OverflowException) {
                int hours, index = str.IndexOf(TimeSeparator);
                string HoursStr = str.Substring(0, index);
                str = str.Remove(0, index);
                str = str.Insert(0, "00");
                try { hours = int.Parse(HoursStr); }
                catch { return new TimeSpan(0); }
                try { ts = TimeSpan.Parse(str); }
                catch { return new TimeSpan(0); }
                ts = new TimeSpan(hours, ts.Minutes, ts.Seconds);
            }
            catch { ts = new TimeSpan(0, 0, 0); }
            return ts;
        }
        public static string TimeSpanToString(TimeSpan value, bool alloDayInput) {
            if (alloDayInput)
                return value.Days.ToString() + DaySeparator + value.Hours.ToString("00") + TimeSeparator + value.Minutes.ToString("00") + TimeSeparator + value.Seconds.ToString("00");
            string hoursStr;
            hoursStr = Math.Floor(value.TotalHours).ToString("0");
            return hoursStr + TimeSeparator + value.Minutes.ToString("00") + TimeSeparator + value.Seconds.ToString("00");

        }
        public static char TimeSeparator { get { return timeSeparator; } }
        public static char DaySeparator { get { return daySeparator; } }
    }
}
