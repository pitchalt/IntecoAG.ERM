using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
//using DevExpress.Persistent;
//using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS {
    //
    public interface csIVersioned<To, Tv> 
        where To : csIVersioned<To, Tv>
        where Tv : csIVersion<To, Tv> 
    {
        Tv Current { get; }
        IList<Tv> Versions { get; }
        Tv Approve(Tv ver);
    }
    //
    public interface csIVersion<To, Tv> 
        where To : csIVersioned<To, Tv>
        where Tv : csIVersion<To, Tv> 
    {
        VersionStates State { get; }
        To VersionedObject { get; }

        Tv Copy();

        void Approve(IObjectSpace os);
        void Decline(IObjectSpace os);
    }
    //
    public static class csCVersionLogic<To, Tv>
        where To : csIVersioned<To, Tv>
        where Tv : csIVersion<To, Tv> 
    {
        static void Approve(Tv ver) { 

        }
    }
}
