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
using System.Data;
using System.IO;
//
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Reports;
//
using IntecoAG.ERM.XAFExt;

namespace IntecoAG.ERM.Module.ReportHelper
{

    /// <summary>
    /// Класс ReportHelper класс для методов загрузки отчётов
    /// </summary>
    [NonPersistent]
    public static class ReportHelper
    {
        //public ReportHelper(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion


        #region МЕТОДЫ

        #region Загрузка отчётов
        /// <summary>
        /// http://www.devexpress.com/Support/Center/e/E1160.aspx How to import additional reports into the database
        /// </summary>
        /// <param name="Path"></param>
        public static void GetAllReportsFromDirectory(string Path, IObjectSpace ObjSpace) {
            DirectoryInfo di = new DirectoryInfo(Path);
            if (!di.Exists) return;
            foreach (FileInfo fi in di.GetFiles("*.repx", SearchOption.TopDirectoryOnly)) {
                using (IObjectSpace os = ObjSpace.CreateNestedObjectSpace()) {
                    CreateReport(os, fi);
                    os.CommitChanges();
                }
            }
        }

        public static void CreateReport(IObjectSpace os, String name, Stream stream) {
            XafExtReportData reportdata = os.FindObject<XafExtReportData>(new BinaryOperator("FileName", name), true);
            if (reportdata == null) {
                reportdata = os.CreateObject<XafExtReportData>();
                XafReport rep = new XafReport() { ReportName = name, ObjectSpace = os };
                rep.LoadLayout(stream);
                reportdata.SaveXtraReport(rep);
                reportdata.FileName = name;
                //                reportdata.Save();
            }
            else {
                XafReport rep = new XafReport() { ReportName = name, ObjectSpace = os };
                rep.LoadLayout(stream);
                reportdata.SaveXtraReport(rep);
            }
        }

        public static void CreateReport(IObjectSpace os, FileInfo fi) {
            Stream fs = fi.OpenRead();
            String rep_name = fi.Name.TrimEnd(fi.Extension.ToCharArray());
            CreateReport(os, rep_name, fs);
        }

        public static void CreateReport(IObjectSpace os, String name, Type module) {
            Stream stream = module.Assembly.GetManifestResourceStream(module, name + ".repx");
            CreateReport(os, name, stream);
        }
        #endregion

        #endregion
    }
}