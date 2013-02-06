using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp;

namespace IntecoAG.ERM.Module {

    // См. http://community.devexpress.com/blogs/garyshort/archive/2010/04/13/xaf-application-model-and-model-editor-improvements-v2010-vol1.aspx
    // по поводу атрибута [DataSourceProperty("Application.Views")]

    // http://documentation.devexpress.com/#Xaf/CustomDocument3169/2
    //  http://www.devexpress.com/Support/Center/p/K18252.aspx
    // http://documentation.devexpress.com/#Xaf/CustomDocument3169
    // 

    [KeyProperty("Name")]
    public interface IModelMiniNavigationItem : IModelNode {
        
        string Name { get; set; }

        string NavigationPath { get; set; }

        [Localizable(true)]
        string NavigationCaption { get; set; }

        [DataSourceProperty("Application.Views")]
        //[DataSourceCriteria("this.Parent is IModelDetailView")]
        IModelView View { get; set; }

        [DataSourceProperty("TargetWindow")]
        TargetWindow TargetWindow { get; set; }

        //public int Order { get; set; } // Порядковый номер НЕ НУЖНО, т.к. АВТОМАТИЧЕСКИ ПОЯВЛЯЕТСЯ Index
        
        /*
        string StateId { get; set; }

        [DataSourceProperty("AB.Contract.VersionStates")]
        //[DataSourceCriteria("View is IModelDetailView")]
        AB.Contract.VersionStates VersionState { get; set; }
        */
    }


    public interface IModelMiniNavigations : IModelNode, IModelList<IModelMiniNavigationItem> {
        //[DisplayName("DefaultMiniNavigationNode")]
        [DataSourceProperty("this")]
        IModelMiniNavigationItem DefaultMiniNavigationNode { get; set; }
    }

    public interface IModelMiniNavigationExtension : IModelNode {
        // ... 
        //IModelMiniNavigationItem MyNodeViewForStateNode { get; }
        IModelMiniNavigations MiniNavigations { get; }
    }

}
