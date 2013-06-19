using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.FinIndex {

    public interface fmIFinIndexStructure {
        IList<fmIFinIndexStructureItem> FinIndexes { get; }
        fmIFinIndexStructureItem FinIndexesCreateItem(fmCFinIndex fin_index);

        void UpdateIndexStructure(IList<fmCFinIndex> index_col);
        void Copy(fmIFinIndexStructure from);
    }

    #region fmIFinIndexStructureLogic
    /// <summary>
    /// Паша!!! Пока с передадим коллекцию index_col явно, потом возможно нужно будет ее получить из ObjectSpace
    /// </summary>
    public static class fmIFinIndexStructureLogic {
        public static void UpdateIndexStructure(fmIFinIndexStructure comp, IList<fmCFinIndex> index_col) {
//            IList<fmCFinIndex> index_col = os.GetObjects<fmCFinIndex>();
            IList<fmIFinIndexStructureItem> removes = new List<fmIFinIndexStructureItem>();
            IDictionary<fmCFinIndex, fmIFinIndexStructureItem> index_dic =
                    new Dictionary<fmCFinIndex, fmIFinIndexStructureItem>(index_col.Count);
            foreach (fmCFinIndex index in index_col)
                index_dic.Add(index, null);
            foreach (fmIFinIndexStructureItem item in comp.FinIndexes) {
                if (item.FinIndex != null)
                    if (index_dic[item.FinIndex] == null)
                        index_dic[item.FinIndex] = item;
                    else
                        removes.Add(item);
                else
                    removes.Add(item);
            }
            foreach (var pair in index_dic) {
                if (pair.Key.IsClosed) {
                    if (pair.Value != null) {
                        comp.FinIndexes.Remove(pair.Value);
                    }
                } else {
                    if (pair.Value == null) {
                        comp.FinIndexesCreateItem(pair.Key);
                    }
                }
            }
            foreach (var item in removes) {
                comp.FinIndexes.Remove(item);
            }
        }

        public static void Copy(fmIFinIndexStructure to, fmIFinIndexStructure from) {
            IList<fmIFinIndexStructureItem> to_list = new List<fmIFinIndexStructureItem>(to.FinIndexes);
            foreach (fmIFinIndexStructureItem item_from in from.FinIndexes) {
                fmIFinIndexStructureItem item_to = to.FinIndexes.FirstOrDefault(
                        (fmIFinIndexStructureItem x) => x.FinIndex == item_from.FinIndex);
                if (item_to == null)
                    item_to = to.FinIndexesCreateItem(item_from.FinIndex);
                else
                    to_list.Remove(item_to);
                item_to.SummKB = item_from.SummKB;
                item_to.SummOZM = item_from.SummOZM;
                item_to.SummOrion = item_from.SummOrion;
                item_to.SummPersonalContract = item_from.SummPersonalContract;
                item_to.SummOther = item_from.SummOther;
            }
            foreach (fmIFinIndexStructureItem item_to in to_list) {
                to.FinIndexes.Remove(item_to);
            }
        }
    }
    #endregion

}