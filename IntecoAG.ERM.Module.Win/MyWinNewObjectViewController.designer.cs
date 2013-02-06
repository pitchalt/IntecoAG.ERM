// Developer Express Code Central Example:
// How to Customize the New Action's Items List
// 
// When an application contains numerous business classes, the New Action can
// contain most of them in its Items list. In this instance, the use of this Action
// can become cumbersome. To make manipulations with this Action easier, you can
// add to the Action's Items list only those types that are contained in the
// currently selected navigation control group. To group navigation control items,
// use the Application Model's NavigationItems node. To customize the New Action's
// Items list, handle the NewObjectViewController.CollectDescendants and
// NewObjectViewController.CollectRoot events of the NewObjectViewController, which
// contains the New Action. The former event is raised when the current object type
// and its descendants are added to the Action's Items list, and the latter is
// raised when all the remaining types whose CreatableItem attribute is set to true
// in the Application Model are added. This example demonstrates how to do
// this.
// 
// See code in the BusinessClasses.cs and MyController.cs
// (BusinessClasses.vb and MyController.vb) files. For details, refer to the How
// to: Customize the New Action's Items List
// (ms-help://DevExpress.Xaf/CustomDocument2915.htm) topic in XAF documentation.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E238

namespace IntecoAG.ERM.Module {
	partial class MyWinNewObjectViewController {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}

		#endregion
	}
}
