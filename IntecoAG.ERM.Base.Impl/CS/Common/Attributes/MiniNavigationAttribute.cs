using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace IntecoAG.ERM.CS.Common {


    /// <summary>
    /// Содержит список путей и их названий на человеческом языке к свойствам класса и входящих в него по ссылкам классов
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MiniNavigationAttribute : Attribute {

        //public LikeSearchPathListAttribute(params string[] prms) { }
        // Тогда применение: [LikeSearchPathList("string1", "string2", "string3", "string4")]


        /*
        //...
        // Можно и так:
        public string[] NavigationPaths { get; set; }
        public string[] NavigationCaptins { get; set; }
        public TargetWindow[] TargetWindows { get; set; }

        public MiniNavigationAttribute(string[] navigationPaths, string[] navigationCaptins, TargetWindow[] targetWindows) {
            this.NavigationPaths = navigationPaths;
            this.NavigationCaptins = navigationCaptins;
            this.TargetWindows = targetWindows;
        }

        // Тогда применение: [MiniNavigation(new string[] { "Путь1", "ИмяДляПути1", "Путь2", "ИмяДляПути2", "Путь3", "ИмяДляПути3", "Путь4", "ИмяДляПути4" })]
        */


        public string NavigationPath { get; set; }
        public string NavigationCaptin { get; set; }
        public TargetWindow TargetWindow { get; set; }
        public int Order { get; set; } // Порядковый номер

        public MiniNavigationAttribute(string navigationPath, string navigationCaptin, TargetWindow targetWindow, int order) {
            this.NavigationPath = navigationPath;
            this.NavigationCaptin = navigationCaptin;
            this.TargetWindow = targetWindow;
            this.Order = order;
        }




    }



/*
    public struct PathStruct {
        public string pathString;
        public string pathCaption;
        public PathStruct(string prmPathString, string prmPathCaption) {
            this.pathString = prmPathString;
            this.pathCaption = prmPathCaption;
        }
    }

    /// <summary>
    /// Содержит список путей и их названий на человеческом языке к свойствам класса и входящих в него по ссылкам классов
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MiniNavigationAttribute : Attribute {

        //public LikeSearchPathListAttribute(params string[] prms) { }
        // Тогда применение: [LikeSearchPathList("string1", "string2", "string3", "string4")]

        //...
        // Можно и так:
        public PathStruct[] NavigationPaths { get; set; }

        public MiniNavigationAttribute(PathStruct[] navigationPaths) {
            this.NavigationPaths = navigationPaths;
        }

        // Тогда применение: [MiniNavigation(new string[] { "Путь1", "ИмяДляПути1", "Путь2", "ИмяДляПути2", "Путь3", "ИмяДляПути3", "Путь4", "ИмяДляПути4" })]


    }
*/
}