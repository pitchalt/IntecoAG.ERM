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
using DevExpress.Xpo;

namespace IntecoAG.ERM.HRM.Organization
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class hrmDepartment
    {
        [Association("hrmDepartment-Staffs")]
        public XPCollection<hrmStaff> Staffs {
            get {
                return GetCollection<hrmStaff>("Staffs");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class hrmStaff
    {
        private hrmDepartment _Department;
        [Association("hrmDepartment-Staffs")]
        public hrmDepartment Department {
            get { return _Department; }
            set { SetPropertyValue<hrmDepartment>("Department", ref _Department, value); }
        }
    }

}
