using System;
using System.Collections.Generic;
using DevExpress.Xpo;
//using System.ComponentModel; // для IBindingList

namespace IntecoAG.ERM.CS
{
    public interface IHDenormalization<T> {

        // Ассоциация список аналитик нижнего уровня
        T UpLevel {
            get;
            set;
        }

        IList<T> DownLevel {
            get;
        }



        // Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        IList<T> FullDownLevel {
            get;
        }

        IList<T> FullUpLevel {
            get;
        }


        //IHDenormalization<T> HDenormPresentationCommon(T hObj, string fieldPrefix);
    }
}