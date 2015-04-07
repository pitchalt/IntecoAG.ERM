














using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Xml;
using System.Xml.Linq;
using IntecoAG.XafExt.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {

	/// <summary>
    /// Сведения из книги покупок об операциях, отражаемых за истекший налоговый период
	/// </summary>
	[Persistent("FmTaxRuVatV504КнигаПокуп")] 
	public  partial class КнигаПокуп  {
		public КнигаПокуп (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Сведения по строке из книги покупок об операциях, отражаемых за истекший налоговый период
		/// </summary>
		[Association("КнигаПокуп-КнПокСтр")]
		public XPCollection<КнПокСтр>КнПокСтр { get { return GetCollection<КнПокСтр>("КнПокСтр");}}
		
		
		
		
		private String _СумНДСВсКПк;
		/// <summary>
		/// Сумманалога всего по книге покупок в рублях и копейках
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СумНДСВсКПк {
			get { return _СумНДСВсКПк; }
			set {
				if (!IsLoading) OnChanging("СумНДСВсКПк", value); 
				SetPropertyValue<String>("СумНДСВсКПк", ref _СумНДСВсКПк, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнигаПокуп");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(КнПокСтр str in КнПокСтр){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						attrib = document.CreateAttribute("СумНДСВсКПк");
						attrib.Value = СумНДСВсКПк.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения по строке из книги покупок об операциях, отражаемых за истекший налоговый период
	/// </summary>
	[Persistent("FmTaxRuVatV504КнПокСтр")] 
	public  partial class КнПокСтр  {
		public КнПокСтр (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Код вида операции
		/// </summary>
		[Association("КнПокСтр-КодВидОпер")]
		public XPCollection<КодВидОпер>КодВидОпер { get { return GetCollection<КодВидОпер>("КодВидОпер");}}
		
		
		
		
		/// <summary>
		/// Сведения о документе, подтверждающем уплату налога
		/// </summary>
		[Association("КнПокСтр-ДокПдтвУпл")]
		public XPCollection<ДокПдтвУпл>ДокПдтвУпл { get { return GetCollection<ДокПдтвУпл>("ДокПдтвУпл");}}
		
		
		
		
		/// <summary>
		/// Дата принятия на учет товаров (работ, услуг), имущественных прав
		/// </summary>
		[Association("КнПокСтр-ДатаУчТов")]
		public XPCollection<ДатаУчТов>ДатаУчТов { get { return GetCollection<ДатаУчТов>("ДатаУчТов");}}
		
		
		
		
		/// <summary>
		/// Сведения о продавце
		/// </summary>
		[Association("КнПокСтр-СвПрод")]
		public XPCollection<СвПрод>СвПрод { get { return GetCollection<СвПрод>("СвПрод");}}
		
		
		
		
		private СвПос _СвПос;
		/// <summary>
		/// Сведения о посреднике (комиссионере, агенте, экспедиторе, застройщике)
		/// </summary>
		
		
		
		public СвПос СвПос {
			get { return _СвПос; }
			set {
				if (!IsLoading) OnChanging("СвПос", value); 
				SetPropertyValue<СвПос>("СвПос", ref _СвПос, value); 
			}
		}
		
		
		
		private String _НомерПор;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомерПор {
			get { return _НомерПор; }
			set {
				if (!IsLoading) OnChanging("НомерПор", value); 
				SetPropertyValue<String>("НомерПор", ref _НомерПор, value); 
			}
		}
		
		
		
		private String _НомСчФПрод;
		/// <summary>
		/// Номер счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомСчФПрод {
			get { return _НомСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомСчФПрод", value); 
				SetPropertyValue<String>("НомСчФПрод", ref _НомСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаСчФПрод;
		/// <summary>
		/// Дата счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаСчФПрод {
			get { return _ДатаСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаСчФПрод", ref _ДатаСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрСчФ;
		/// <summary>
		/// Номер исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрСчФ {
			get { return _НомИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрСчФ", value); 
				SetPropertyValue<String>("НомИспрСчФ", ref _НомИспрСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрСчФ;
		/// <summary>
		/// Дата исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрСчФ {
			get { return _ДатаИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрСчФ", ref _ДатаИспрСчФ, value); 
			}
		}
		
		
		
		private String _НомКСчФПрод;
		/// <summary>
		/// Номер корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомКСчФПрод {
			get { return _НомКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомКСчФПрод", value); 
				SetPropertyValue<String>("НомКСчФПрод", ref _НомКСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаКСчФПрод;
		/// <summary>
		/// Дата корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаКСчФПрод {
			get { return _ДатаКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаКСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаКСчФПрод", ref _ДатаКСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрКСчФ;
		/// <summary>
		/// Номер исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрКСчФ {
			get { return _НомИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрКСчФ", value); 
				SetPropertyValue<String>("НомИспрКСчФ", ref _НомИспрКСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрКСчФ;
		/// <summary>
		/// Дата исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрКСчФ {
			get { return _ДатаИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрКСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрКСчФ", ref _ДатаИспрКСчФ, value); 
			}
		}
		
		
		
		private String _НомТД;
		/// <summary>
		/// Номер таможенной декларации
		/// </summary>
		
		
		
		public String НомТД {
			get { return _НомТД; }
			set {
				if (!IsLoading) OnChanging("НомТД", value); 
				SetPropertyValue<String>("НомТД", ref _НомТД, value); 
			}
		}
		
		
		
		private String _ОКВ;
		/// <summary>
		/// Код валюты по ОКВ
		/// </summary>
		
		
		
		public String ОКВ {
			get { return _ОКВ; }
			set {
				if (!IsLoading) OnChanging("ОКВ", value); 
				SetPropertyValue<String>("ОКВ", ref _ОКВ, value); 
			}
		}
		
		
		
		private String _СтоимПокупВ;
		/// <summary>
		/// Стоимость покупок по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СтоимПокупВ {
			get { return _СтоимПокупВ; }
			set {
				if (!IsLoading) OnChanging("СтоимПокупВ", value); 
				SetPropertyValue<String>("СтоимПокупВ", ref _СтоимПокупВ, value); 
			}
		}
		
		
		
		private String _СумНДСВыч;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре, принимаемая к вычету, в рублях и копейках
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СумНДСВыч {
			get { return _СумНДСВыч; }
			set {
				if (!IsLoading) OnChanging("СумНДСВыч", value); 
				SetPropertyValue<String>("СумНДСВыч", ref _СумНДСВыч, value); 
			}
		}
		
		
		
		private КнигаПокуп _КнигаПокуп;
		/// <summary>
		/// Связь КнигаПокуп-КнПокСтр
		/// </summary>
		
		
		 [Association("КнигаПокуп-КнПокСтр")]
		public КнигаПокуп КнигаПокуп {
			get { return _КнигаПокуп; }
			set {
				if (!IsLoading) OnChanging("КнигаПокуп", value); 
				SetPropertyValue<КнигаПокуп>("КнигаПокуп", ref _КнигаПокуп, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнПокСтр");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(КодВидОпер str in КодВидОпер){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						
						foreach(ДокПдтвУпл str in ДокПдтвУпл){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						
						foreach(ДатаУчТов str in ДатаУчТов){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						
						foreach(СвПрод str in СвПрод){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						
							if (СвПос != null) result.AppendChild(СвПос.ToXmlGenerated(document, result));
						
					
				
					
						attrib = document.CreateAttribute("НомерПор");
						attrib.Value = НомерПор.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомСчФПрод");
						attrib.Value = НомСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаСчФПрод");
						attrib.Value = ДатаСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрСчФ");
						attrib.Value = НомИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрСчФ");
						attrib.Value = ДатаИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомКСчФПрод");
						attrib.Value = НомКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаКСчФПрод");
						attrib.Value = ДатаКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрКСчФ");
						attrib.Value = НомИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрКСчФ");
						attrib.Value = ДатаИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомТД");
						attrib.Value = НомТД.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ОКВ");
						attrib.Value = ОКВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПокупВ");
						attrib.Value = СтоимПокупВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСВыч");
						attrib.Value = СумНДСВыч.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Код вида операции
	/// </summary>
	[Persistent("FmTaxRuVatV504КодВидОпер")] 
	public  partial class КодВидОпер : BaseEntity {
		public КодВидОпер (Session session): base(session) { } 
		
		
		
		
		private КнПокСтр _КнПокСтр;
		/// <summary>
		/// Связь КнПокСтр-КодВидОпер
		/// </summary>
		
		
		 [Association("КнПокСтр-КодВидОпер")]
		public КнПокСтр КнПокСтр {
			get { return _КнПокСтр; }
			set {
				if (!IsLoading) OnChanging("КнПокСтр", value); 
				SetPropertyValue<КнПокСтр>("КнПокСтр", ref _КнПокСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КодВидОпер");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения о документе, подтверждающем уплату налога
	/// </summary>
	[Persistent("FmTaxRuVatV504ДокПдтвУпл")] 
	public  partial class ДокПдтвУпл : BaseEntity {
		public ДокПдтвУпл (Session session): base(session) { } 
		
		
		
		
		private String _НомДокПдтвУпл;
		/// <summary>
		/// Номер документа, подтверждающего уплату налога
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомДокПдтвУпл {
			get { return _НомДокПдтвУпл; }
			set {
				if (!IsLoading) OnChanging("НомДокПдтвУпл", value); 
				SetPropertyValue<String>("НомДокПдтвУпл", ref _НомДокПдтвУпл, value); 
			}
		}
		
		
		
		private DateTime _ДатаДокПдтвУпл;
		/// <summary>
		/// Дата документа, подтверждающего уплату налога
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public DateTime ДатаДокПдтвУпл {
			get { return _ДатаДокПдтвУпл; }
			set {
				if (!IsLoading) OnChanging("ДатаДокПдтвУпл", value); 
				SetPropertyValue<DateTime>("ДатаДокПдтвУпл", ref _ДатаДокПдтвУпл, value); 
			}
		}
		
		
		
		private КнПокСтр _КнПокСтр;
		/// <summary>
		/// Связь КнПокСтр-ДокПдтвУпл
		/// </summary>
		
		
		 [Association("КнПокСтр-ДокПдтвУпл")]
		public КнПокСтр КнПокСтр {
			get { return _КнПокСтр; }
			set {
				if (!IsLoading) OnChanging("КнПокСтр", value); 
				SetPropertyValue<КнПокСтр>("КнПокСтр", ref _КнПокСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("ДокПдтвУпл");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("НомДокПдтвУпл");
						attrib.Value = НомДокПдтвУпл.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаДокПдтвУпл");
						attrib.Value = ДатаДокПдтвУпл.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Дата принятия на учет товаров (работ, услуг), имущественных прав
	/// </summary>
	[Persistent("FmTaxRuVatV504ДатаУчТов")] 
	public  partial class ДатаУчТов : BaseEntity {
		public ДатаУчТов (Session session): base(session) { } 
		
		
		
		
		private КнПокСтр _КнПокСтр;
		/// <summary>
		/// Связь КнПокСтр-ДатаУчТов
		/// </summary>
		
		
		 [Association("КнПокСтр-ДатаУчТов")]
		public КнПокСтр КнПокСтр {
			get { return _КнПокСтр; }
			set {
				if (!IsLoading) OnChanging("КнПокСтр", value); 
				SetPropertyValue<КнПокСтр>("КнПокСтр", ref _КнПокСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("ДатаУчТов");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения о продавце
	/// </summary>
	[Persistent("FmTaxRuVatV504СвПрод")] 
	public  partial class СвПрод : BaseEntity {
		public СвПрод (Session session): base(session) { } 
		
		
		
		
		private КнПокСтр _КнПокСтр;
		/// <summary>
		/// Связь КнПокСтр-СвПрод
		/// </summary>
		
		
		 [Association("КнПокСтр-СвПрод")]
		public КнПокСтр КнПокСтр {
			get { return _КнПокСтр; }
			set {
				if (!IsLoading) OnChanging("КнПокСтр", value); 
				SetPropertyValue<КнПокСтр>("КнПокСтр", ref _КнПокСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("СвПрод");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения о посреднике (комиссионере, агенте, экспедиторе, застройщике)
	/// </summary>
	[Persistent("FmTaxRuVatV504СвПос")] 
	public  partial class СвПос : BaseEntity {
		public СвПос (Session session): base(session) { } 
		
		private String _Значение;
		public String Значение{get {return _Значение;}
					set {
				if (!IsLoading) OnChanging("Значение", value); 
				SetPropertyValue<String>("Значение", ref _Значение, value); 
			}
		}
		
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("СвПос");
				last_node.AppendChild(result);
				 result.InnerText = Значение; 
				XmlAttribute attrib;
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения об организации
	/// </summary>
	[Persistent("FmTaxRuVatV504СведЮЛ")] 
	public  partial class СведЮЛ : BaseEntity {
		public СведЮЛ (Session session): base(session) { } 
		
		
		
		
		private String _ИННЮЛ;
		/// <summary>
		/// ИНН организации
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String ИННЮЛ {
			get { return _ИННЮЛ; }
			set {
				if (!IsLoading) OnChanging("ИННЮЛ", value); 
				SetPropertyValue<String>("ИННЮЛ", ref _ИННЮЛ, value); 
			}
		}
		
		
		
		private String _КПП;
		/// <summary>
		/// КПП
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String КПП {
			get { return _КПП; }
			set {
				if (!IsLoading) OnChanging("КПП", value); 
				SetPropertyValue<String>("КПП", ref _КПП, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("СведЮЛ");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("ИННЮЛ");
						attrib.Value = ИННЮЛ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("КПП");
						attrib.Value = КПП.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения об индивидуальном предпринимателе
	/// </summary>
	[Persistent("FmTaxRuVatV504СведИП")] 
	public  partial class СведИП : BaseEntity {
		public СведИП (Session session): base(session) { } 
		
		
		
		
		private String _ИННФЛ;
		/// <summary>
		/// ИНН физического лица
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String ИННФЛ {
			get { return _ИННФЛ; }
			set {
				if (!IsLoading) OnChanging("ИННФЛ", value); 
				SetPropertyValue<String>("ИННФЛ", ref _ИННФЛ, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("СведИП");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("ИННФЛ");
						attrib.Value = ИННФЛ.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения из дополнительных листов книги покупок
	/// </summary>
	[Persistent("FmTaxRuVatV504КнигаПокупДЛ")] 
	public  partial class КнигаПокупДЛ  {
		public КнигаПокупДЛ (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Сведения по строке из дополнительных листов книги покупок
		/// </summary>
		[Association("КнигаПокупДЛ-КнПокДЛСтр")]
		public XPCollection<КнПокДЛСтр>КнПокДЛСтр { get { return GetCollection<КнПокДЛСтр>("КнПокДЛСтр");}}
		
		
		
		
		private String _СумНДСИтКПк;
		/// <summary>
		/// Итоговая сумма налога по книге покупок
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СумНДСИтКПк {
			get { return _СумНДСИтКПк; }
			set {
				if (!IsLoading) OnChanging("СумНДСИтКПк", value); 
				SetPropertyValue<String>("СумНДСИтКПк", ref _СумНДСИтКПк, value); 
			}
		}
		
		
		
		private String _СумНДСИтП1Р8;
		/// <summary>
		/// Сумма налога всего по приложению 1 к разделу 8 в рублях и копейках
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СумНДСИтП1Р8 {
			get { return _СумНДСИтП1Р8; }
			set {
				if (!IsLoading) OnChanging("СумНДСИтП1Р8", value); 
				SetPropertyValue<String>("СумНДСИтП1Р8", ref _СумНДСИтП1Р8, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнигаПокупДЛ");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(КнПокДЛСтр str in КнПокДЛСтр){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						attrib = document.CreateAttribute("СумНДСИтКПк");
						attrib.Value = СумНДСИтКПк.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСИтП1Р8");
						attrib.Value = СумНДСИтП1Р8.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения по строке из дополнительных листов книги покупок
	/// </summary>
	[Persistent("FmTaxRuVatV504КнПокДЛСтр")] 
	public  partial class КнПокДЛСтр  {
		public КнПокДЛСтр (Session session): base(session) { } 
		
		
		
		
		private String _НомерПор;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомерПор {
			get { return _НомерПор; }
			set {
				if (!IsLoading) OnChanging("НомерПор", value); 
				SetPropertyValue<String>("НомерПор", ref _НомерПор, value); 
			}
		}
		
		
		
		private String _НомСчФПрод;
		/// <summary>
		/// Номер счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомСчФПрод {
			get { return _НомСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомСчФПрод", value); 
				SetPropertyValue<String>("НомСчФПрод", ref _НомСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаСчФПрод;
		/// <summary>
		/// Дата счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаСчФПрод {
			get { return _ДатаСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаСчФПрод", ref _ДатаСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрСчФ;
		/// <summary>
		/// Номер исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрСчФ {
			get { return _НомИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрСчФ", value); 
				SetPropertyValue<String>("НомИспрСчФ", ref _НомИспрСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрСчФ;
		/// <summary>
		/// Дата исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрСчФ {
			get { return _ДатаИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрСчФ", ref _ДатаИспрСчФ, value); 
			}
		}
		
		
		
		private String _НомКСчФПрод;
		/// <summary>
		/// Номер корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомКСчФПрод {
			get { return _НомКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомКСчФПрод", value); 
				SetPropertyValue<String>("НомКСчФПрод", ref _НомКСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаКСчФПрод;
		/// <summary>
		/// Дата корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаКСчФПрод {
			get { return _ДатаКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаКСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаКСчФПрод", ref _ДатаКСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрКСчФ;
		/// <summary>
		/// Номер исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрКСчФ {
			get { return _НомИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрКСчФ", value); 
				SetPropertyValue<String>("НомИспрКСчФ", ref _НомИспрКСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрКСчФ;
		/// <summary>
		/// Дата исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрКСчФ {
			get { return _ДатаИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрКСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрКСчФ", ref _ДатаИспрКСчФ, value); 
			}
		}
		
		
		
		private String _НомТД;
		/// <summary>
		/// Номер таможенной декларации
		/// </summary>
		
		
		
		public String НомТД {
			get { return _НомТД; }
			set {
				if (!IsLoading) OnChanging("НомТД", value); 
				SetPropertyValue<String>("НомТД", ref _НомТД, value); 
			}
		}
		
		
		
		private String _ОКВ;
		/// <summary>
		/// Код валюты по ОКВ
		/// </summary>
		
		
		
		public String ОКВ {
			get { return _ОКВ; }
			set {
				if (!IsLoading) OnChanging("ОКВ", value); 
				SetPropertyValue<String>("ОКВ", ref _ОКВ, value); 
			}
		}
		
		
		
		private String _СтоимПокупВ;
		/// <summary>
		/// Стоимость покупок по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СтоимПокупВ {
			get { return _СтоимПокупВ; }
			set {
				if (!IsLoading) OnChanging("СтоимПокупВ", value); 
				SetPropertyValue<String>("СтоимПокупВ", ref _СтоимПокупВ, value); 
			}
		}
		
		
		
		private String _СумНДС;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре, принимаемая к вычету, в рублях и копейках
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String СумНДС {
			get { return _СумНДС; }
			set {
				if (!IsLoading) OnChanging("СумНДС", value); 
				SetPropertyValue<String>("СумНДС", ref _СумНДС, value); 
			}
		}
		
		
		
		private КнигаПокупДЛ _КнигаПокупДЛ;
		/// <summary>
		/// Связь КнигаПокупДЛ-КнПокДЛСтр
		/// </summary>
		
		
		 [Association("КнигаПокупДЛ-КнПокДЛСтр")]
		public КнигаПокупДЛ КнигаПокупДЛ {
			get { return _КнигаПокупДЛ; }
			set {
				if (!IsLoading) OnChanging("КнигаПокупДЛ", value); 
				SetPropertyValue<КнигаПокупДЛ>("КнигаПокупДЛ", ref _КнигаПокупДЛ, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнПокДЛСтр");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("НомерПор");
						attrib.Value = НомерПор.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомСчФПрод");
						attrib.Value = НомСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаСчФПрод");
						attrib.Value = ДатаСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрСчФ");
						attrib.Value = НомИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрСчФ");
						attrib.Value = ДатаИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомКСчФПрод");
						attrib.Value = НомКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаКСчФПрод");
						attrib.Value = ДатаКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрКСчФ");
						attrib.Value = НомИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрКСчФ");
						attrib.Value = ДатаИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомТД");
						attrib.Value = НомТД.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ОКВ");
						attrib.Value = ОКВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПокупВ");
						attrib.Value = СтоимПокупВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДС");
						attrib.Value = СумНДС.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения из книги продаж об операциях, отражаемых за истекший налоговый период
	/// </summary>
	[Persistent("FmTaxRuVatV504КнигаПрод")] 
	public  partial class КнигаПрод  {
		public КнигаПрод (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Сведения по строке из книги продаж об операциях, отражаемых за истекший налоговый период
		/// </summary>
		[Association("КнигаПрод-КнПродСтр")]
		public XPCollection<КнПродСтр>КнПродСтр { get { return GetCollection<КнПродСтр>("КнПродСтр");}}
		
		
		
		
		private String _СтПродБезНДС18;
		/// <summary>
		/// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 18%
		/// </summary>
		
		
		
		public String СтПродБезНДС18 {
			get { return _СтПродБезНДС18; }
			set {
				if (!IsLoading) OnChanging("СтПродБезНДС18", value); 
				SetPropertyValue<String>("СтПродБезНДС18", ref _СтПродБезНДС18, value); 
			}
		}
		
		
		
		private String _СтПродБезНДС10;
		/// <summary>
		/// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 10%
		/// </summary>
		
		
		
		public String СтПродБезНДС10 {
			get { return _СтПродБезНДС10; }
			set {
				if (!IsLoading) OnChanging("СтПродБезНДС10", value); 
				SetPropertyValue<String>("СтПродБезНДС10", ref _СтПродБезНДС10, value); 
			}
		}
		
		
		
		private String _СтПродБезНДС0;
		/// <summary>
		/// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 0%
		/// </summary>
		
		
		
		public String СтПродБезНДС0 {
			get { return _СтПродБезНДС0; }
			set {
				if (!IsLoading) OnChanging("СтПродБезНДС0", value); 
				SetPropertyValue<String>("СтПродБезНДС0", ref _СтПродБезНДС0, value); 
			}
		}
		
		
		
		private String _СумНДСВсКПр18;
		/// <summary>
		/// Всего сумма налога по книге продаж в рублях и копейках, по ставке 18%
		/// </summary>
		
		
		
		public String СумНДСВсКПр18 {
			get { return _СумНДСВсКПр18; }
			set {
				if (!IsLoading) OnChanging("СумНДСВсКПр18", value); 
				SetPropertyValue<String>("СумНДСВсКПр18", ref _СумНДСВсКПр18, value); 
			}
		}
		
		
		
		private String _СумНДСВсКПр10;
		/// <summary>
		/// Всего сумма налога по книге продаж в рублях и копейках, по ставке 10%
		/// </summary>
		
		
		
		public String СумНДСВсКПр10 {
			get { return _СумНДСВсКПр10; }
			set {
				if (!IsLoading) OnChanging("СумНДСВсКПр10", value); 
				SetPropertyValue<String>("СумНДСВсКПр10", ref _СумНДСВсКПр10, value); 
			}
		}
		
		
		
		private String _СтПродОсвВсКПр;
		/// <summary>
		/// Всего стоимость продаж, освобождаемых от налога, по книге продаж в рублях и копейках
		/// </summary>
		
		
		
		public String СтПродОсвВсКПр {
			get { return _СтПродОсвВсКПр; }
			set {
				if (!IsLoading) OnChanging("СтПродОсвВсКПр", value); 
				SetPropertyValue<String>("СтПродОсвВсКПр", ref _СтПродОсвВсКПр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнигаПрод");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(КнПродСтр str in КнПродСтр){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						attrib = document.CreateAttribute("СтПродБезНДС18");
						attrib.Value = СтПродБезНДС18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродБезНДС10");
						attrib.Value = СтПродБезНДС10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродБезНДС0");
						attrib.Value = СтПродБезНДС0.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСВсКПр18");
						attrib.Value = СумНДСВсКПр18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСВсКПр10");
						attrib.Value = СумНДСВсКПр10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродОсвВсКПр");
						attrib.Value = СтПродОсвВсКПр.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения по строке из книги продаж об операциях, отражаемых за истекший налоговый период
	/// </summary>
	[Persistent("FmTaxRuVatV504КнПродСтр")] 
	public  partial class КнПродСтр  {
		public КнПродСтр (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Сведения о документе, подтверждающем оплату
		/// </summary>
		[Association("КнПродСтр-ДокПдтвОпл")]
		public XPCollection<ДокПдтвОпл>ДокПдтвОпл { get { return GetCollection<ДокПдтвОпл>("ДокПдтвОпл");}}
		
		
		
		
		/// <summary>
		/// Сведения о покупателе
		/// </summary>
		[Association("КнПродСтр-СвПокуп")]
		public XPCollection<СвПокуп>СвПокуп { get { return GetCollection<СвПокуп>("СвПокуп");}}
		
		
		
		
		private String _НомерПор;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомерПор {
			get { return _НомерПор; }
			set {
				if (!IsLoading) OnChanging("НомерПор", value); 
				SetPropertyValue<String>("НомерПор", ref _НомерПор, value); 
			}
		}
		
		
		
		private String _НомСчФПрод;
		/// <summary>
		/// Номер счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомСчФПрод {
			get { return _НомСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомСчФПрод", value); 
				SetPropertyValue<String>("НомСчФПрод", ref _НомСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаСчФПрод;
		/// <summary>
		/// Дата счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public DateTime ДатаСчФПрод {
			get { return _ДатаСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаСчФПрод", ref _ДатаСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрСчФ;
		/// <summary>
		/// Номер исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрСчФ {
			get { return _НомИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрСчФ", value); 
				SetPropertyValue<String>("НомИспрСчФ", ref _НомИспрСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрСчФ;
		/// <summary>
		/// Дата исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрСчФ {
			get { return _ДатаИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрСчФ", ref _ДатаИспрСчФ, value); 
			}
		}
		
		
		
		private String _НомКСчФПрод;
		/// <summary>
		/// Номер корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомКСчФПрод {
			get { return _НомКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомКСчФПрод", value); 
				SetPropertyValue<String>("НомКСчФПрод", ref _НомКСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаКСчФПрод;
		/// <summary>
		/// Дата корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаКСчФПрод {
			get { return _ДатаКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаКСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаКСчФПрод", ref _ДатаКСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрКСчФ;
		/// <summary>
		/// Номер исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрКСчФ {
			get { return _НомИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрКСчФ", value); 
				SetPropertyValue<String>("НомИспрКСчФ", ref _НомИспрКСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрКСчФ;
		/// <summary>
		/// Дата исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрКСчФ {
			get { return _ДатаИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрКСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрКСчФ", ref _ДатаИспрКСчФ, value); 
			}
		}
		
		
		
		private String _ОКВ;
		/// <summary>
		/// Код валюты по ОКВ
		/// </summary>
		
		
		
		public String ОКВ {
			get { return _ОКВ; }
			set {
				if (!IsLoading) OnChanging("ОКВ", value); 
				SetPropertyValue<String>("ОКВ", ref _ОКВ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФВ;
		/// <summary>
		/// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
		/// </summary>
		
		
		
		public String СтоимПродСФВ {
			get { return _СтоимПродСФВ; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФВ", value); 
				SetPropertyValue<String>("СтоимПродСФВ", ref _СтоимПродСФВ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ;
		/// <summary>
		/// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог) в рублях и копейках
		/// </summary>
		
		
		
		public String СтоимПродСФ {
			get { return _СтоимПродСФ; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ", value); 
				SetPropertyValue<String>("СтоимПродСФ", ref _СтоимПродСФ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ18;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 18 %
		/// </summary>
		
		
		
		public String СтоимПродСФ18 {
			get { return _СтоимПродСФ18; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ18", value); 
				SetPropertyValue<String>("СтоимПродСФ18", ref _СтоимПродСФ18, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ10;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 10
		/// </summary>
		
		
		
		public String СтоимПродСФ10 {
			get { return _СтоимПродСФ10; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ10", value); 
				SetPropertyValue<String>("СтоимПродСФ10", ref _СтоимПродСФ10, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ0;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 0
		/// </summary>
		
		
		
		public String СтоимПродСФ0 {
			get { return _СтоимПродСФ0; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ0", value); 
				SetPropertyValue<String>("СтоимПродСФ0", ref _СтоимПродСФ0, value); 
			}
		}
		
		
		
		private String _СумНДССФ18;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках, по ставке 18
		/// </summary>
		
		
		
		public String СумНДССФ18 {
			get { return _СумНДССФ18; }
			set {
				if (!IsLoading) OnChanging("СумНДССФ18", value); 
				SetPropertyValue<String>("СумНДССФ18", ref _СумНДССФ18, value); 
			}
		}
		
		
		
		private String _СумНДССФ10;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках,  по ставке 10
		/// </summary>
		
		
		
		public String СумНДССФ10 {
			get { return _СумНДССФ10; }
			set {
				if (!IsLoading) OnChanging("СумНДССФ10", value); 
				SetPropertyValue<String>("СумНДССФ10", ref _СумНДССФ10, value); 
			}
		}
		
		
		
		private String _СтоимПродОсв;
		/// <summary>
		/// Стоимость продаж, освобождаемых от налога, по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках
		/// </summary>
		
		
		
		public String СтоимПродОсв {
			get { return _СтоимПродОсв; }
			set {
				if (!IsLoading) OnChanging("СтоимПродОсв", value); 
				SetPropertyValue<String>("СтоимПродОсв", ref _СтоимПродОсв, value); 
			}
		}
		
		
		
		private КнигаПрод _КнигаПрод;
		/// <summary>
		/// Связь КнигаПрод-КнПродСтр
		/// </summary>
		
		
		 [Association("КнигаПрод-КнПродСтр")]
		public КнигаПрод КнигаПрод {
			get { return _КнигаПрод; }
			set {
				if (!IsLoading) OnChanging("КнигаПрод", value); 
				SetPropertyValue<КнигаПрод>("КнигаПрод", ref _КнигаПрод, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнПродСтр");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(ДокПдтвОпл str in ДокПдтвОпл){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						
						foreach(СвПокуп str in СвПокуп){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						attrib = document.CreateAttribute("НомерПор");
						attrib.Value = НомерПор.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомСчФПрод");
						attrib.Value = НомСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаСчФПрод");
						attrib.Value = ДатаСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрСчФ");
						attrib.Value = НомИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрСчФ");
						attrib.Value = ДатаИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомКСчФПрод");
						attrib.Value = НомКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаКСчФПрод");
						attrib.Value = ДатаКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрКСчФ");
						attrib.Value = НомИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрКСчФ");
						attrib.Value = ДатаИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ОКВ");
						attrib.Value = ОКВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФВ");
						attrib.Value = СтоимПродСФВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ");
						attrib.Value = СтоимПродСФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ18");
						attrib.Value = СтоимПродСФ18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ10");
						attrib.Value = СтоимПродСФ10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ0");
						attrib.Value = СтоимПродСФ0.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДССФ18");
						attrib.Value = СумНДССФ18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДССФ10");
						attrib.Value = СумНДССФ10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродОсв");
						attrib.Value = СтоимПродОсв.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения о документе, подтверждающем оплату
	/// </summary>
	[Persistent("FmTaxRuVatV504ДокПдтвОпл")] 
	public  partial class ДокПдтвОпл : BaseEntity {
		public ДокПдтвОпл (Session session): base(session) { } 
		
		
		
		
		private String _НомДокПдтвОпл;
		/// <summary>
		/// Номер документа, подтверждающего оплату
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомДокПдтвОпл {
			get { return _НомДокПдтвОпл; }
			set {
				if (!IsLoading) OnChanging("НомДокПдтвОпл", value); 
				SetPropertyValue<String>("НомДокПдтвОпл", ref _НомДокПдтвОпл, value); 
			}
		}
		
		
		
		private DateTime _ДатаДокПдтвОпл;
		/// <summary>
		/// Дата документа, подтверждающего оплату
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public DateTime ДатаДокПдтвОпл {
			get { return _ДатаДокПдтвОпл; }
			set {
				if (!IsLoading) OnChanging("ДатаДокПдтвОпл", value); 
				SetPropertyValue<DateTime>("ДатаДокПдтвОпл", ref _ДатаДокПдтвОпл, value); 
			}
		}
		
		
		
		private КнПродСтр _КнПродСтр;
		/// <summary>
		/// Связь КнПродСтр-ДокПдтвОпл
		/// </summary>
		
		
		 [Association("КнПродСтр-ДокПдтвОпл")]
		public КнПродСтр КнПродСтр {
			get { return _КнПродСтр; }
			set {
				if (!IsLoading) OnChanging("КнПродСтр", value); 
				SetPropertyValue<КнПродСтр>("КнПродСтр", ref _КнПродСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("ДокПдтвОпл");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("НомДокПдтвОпл");
						attrib.Value = НомДокПдтвОпл.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаДокПдтвОпл");
						attrib.Value = ДатаДокПдтвОпл.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения о покупателе
	/// </summary>
	[Persistent("FmTaxRuVatV504СвПокуп")] 
	public  partial class СвПокуп : BaseEntity {
		public СвПокуп (Session session): base(session) { } 
		
		
		
		
		private КнПродСтр _КнПродСтр;
		/// <summary>
		/// Связь КнПродСтр-СвПокуп
		/// </summary>
		
		
		 [Association("КнПродСтр-СвПокуп")]
		public КнПродСтр КнПродСтр {
			get { return _КнПродСтр; }
			set {
				if (!IsLoading) OnChanging("КнПродСтр", value); 
				SetPropertyValue<КнПродСтр>("КнПродСтр", ref _КнПродСтр, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("СвПокуп");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения из дополнительных листов книги продаж
	/// </summary>
	[Persistent("FmTaxRuVatV504КнигаПродДЛ")] 
	public  partial class КнигаПродДЛ  {
		public КнигаПродДЛ (Session session): base(session) { } 
		
		
		
		
		/// <summary>
		/// Сведения по строке из дополнительных листов книги продаж
		/// </summary>
		[Association("КнигаПродДЛ-КнПродДЛСтр")]
		public XPCollection<КнПродДЛСтр>КнПродДЛСтр { get { return GetCollection<КнПродДЛСтр>("КнПродДЛСтр");}}
		
		
		
		
		private String _ИтСтПродКПр18;
		/// <summary>
		/// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 18%
		/// </summary>
		
		
		
		public String ИтСтПродКПр18 {
			get { return _ИтСтПродКПр18; }
			set {
				if (!IsLoading) OnChanging("ИтСтПродКПр18", value); 
				SetPropertyValue<String>("ИтСтПродКПр18", ref _ИтСтПродКПр18, value); 
			}
		}
		
		
		
		private String _ИтСтПродКПр10;
		/// <summary>
		/// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 10%
		/// </summary>
		
		
		
		public String ИтСтПродКПр10 {
			get { return _ИтСтПродКПр10; }
			set {
				if (!IsLoading) OnChanging("ИтСтПродКПр10", value); 
				SetPropertyValue<String>("ИтСтПродКПр10", ref _ИтСтПродКПр10, value); 
			}
		}
		
		
		
		private String _ИтСтПродКПр0;
		/// <summary>
		/// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 0%
		/// </summary>
		
		
		
		public String ИтСтПродКПр0 {
			get { return _ИтСтПродКПр0; }
			set {
				if (!IsLoading) OnChanging("ИтСтПродКПр0", value); 
				SetPropertyValue<String>("ИтСтПродКПр0", ref _ИтСтПродКПр0, value); 
			}
		}
		
		
		
		private String _СумНДСИтКПр18;
		/// <summary>
		/// Итоговая сумма налога по книге продаж в рублях и копейках,  по ставке 18%
		/// </summary>
		
		
		
		public String СумНДСИтКПр18 {
			get { return _СумНДСИтКПр18; }
			set {
				if (!IsLoading) OnChanging("СумНДСИтКПр18", value); 
				SetPropertyValue<String>("СумНДСИтКПр18", ref _СумНДСИтКПр18, value); 
			}
		}
		
		
		
		private String _СумНДСИтКПр10;
		/// <summary>
		/// Итоговая сумма налога по книге продаж в рублях и копейках,  по ставке 10%
		/// </summary>
		
		
		
		public String СумНДСИтКПр10 {
			get { return _СумНДСИтКПр10; }
			set {
				if (!IsLoading) OnChanging("СумНДСИтКПр10", value); 
				SetPropertyValue<String>("СумНДСИтКПр10", ref _СумНДСИтКПр10, value); 
			}
		}
		
		
		
		private String _ИтСтПродОсвКПр;
		/// <summary>
		/// Итоговая стоимость продаж, освобождаемых от налога, по книге продаж в рублях и копейках
		/// </summary>
		
		
		
		public String ИтСтПродОсвКПр {
			get { return _ИтСтПродОсвКПр; }
			set {
				if (!IsLoading) OnChanging("ИтСтПродОсвКПр", value); 
				SetPropertyValue<String>("ИтСтПродОсвКПр", ref _ИтСтПродОсвКПр, value); 
			}
		}
		
		
		
		private String _СтПродВсП1Р9_18;
		/// <summary>
		/// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 18%
		/// </summary>
		
		
		
		public String СтПродВсП1Р9_18 {
			get { return _СтПродВсП1Р9_18; }
			set {
				if (!IsLoading) OnChanging("СтПродВсП1Р9_18", value); 
				SetPropertyValue<String>("СтПродВсП1Р9_18", ref _СтПродВсП1Р9_18, value); 
			}
		}
		
		
		
		private String _СтПродВсП1Р9_10;
		/// <summary>
		/// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 10%
		/// </summary>
		
		
		
		public String СтПродВсП1Р9_10 {
			get { return _СтПродВсП1Р9_10; }
			set {
				if (!IsLoading) OnChanging("СтПродВсП1Р9_10", value); 
				SetPropertyValue<String>("СтПродВсП1Р9_10", ref _СтПродВсП1Р9_10, value); 
			}
		}
		
		
		
		private String _СтПродВсП1Р9_0;
		/// <summary>
		/// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 0%
		/// </summary>
		
		
		
		public String СтПродВсП1Р9_0 {
			get { return _СтПродВсП1Р9_0; }
			set {
				if (!IsLoading) OnChanging("СтПродВсП1Р9_0", value); 
				SetPropertyValue<String>("СтПродВсП1Р9_0", ref _СтПродВсП1Р9_0, value); 
			}
		}
		
		
		
		private String _СумНДСВсП1Р9_18;
		/// <summary>
		/// Всего сумма налога по Приложению 1 к Разделу 9 в рублях и копейках, по ставке  18%
		/// </summary>
		
		
		
		public String СумНДСВсП1Р9_18 {
			get { return _СумНДСВсП1Р9_18; }
			set {
				if (!IsLoading) OnChanging("СумНДСВсП1Р9_18", value); 
				SetPropertyValue<String>("СумНДСВсП1Р9_18", ref _СумНДСВсП1Р9_18, value); 
			}
		}
		
		
		
		private String _СумНДСВсП1Р9_10;
		/// <summary>
		/// Всего сумма налога по Приложению 1 к Разделу 9 в рублях и копейках, по ставке 10%
		/// </summary>
		
		
		
		public String СумНДСВсП1Р9_10 {
			get { return _СумНДСВсП1Р9_10; }
			set {
				if (!IsLoading) OnChanging("СумНДСВсП1Р9_10", value); 
				SetPropertyValue<String>("СумНДСВсП1Р9_10", ref _СумНДСВсП1Р9_10, value); 
			}
		}
		
		
		
		private String _СтПродОсвП1Р9Вс;
		/// <summary>
		/// Всего стоимость продаж, освобождаемых от налога, по приложению 1 к Разделу 9 в рублях и копейках
		/// </summary>
		
		
		
		public String СтПродОсвП1Р9Вс {
			get { return _СтПродОсвП1Р9Вс; }
			set {
				if (!IsLoading) OnChanging("СтПродОсвП1Р9Вс", value); 
				SetPropertyValue<String>("СтПродОсвП1Р9Вс", ref _СтПродОсвП1Р9Вс, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнигаПродДЛ");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						
						foreach(КнПродДЛСтр str in КнПродДЛСтр){
							result.AppendChild(str.ToXmlGenerated(document, result));
						}
						
					
				
					
						attrib = document.CreateAttribute("ИтСтПродКПр18");
						attrib.Value = ИтСтПродКПр18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ИтСтПродКПр10");
						attrib.Value = ИтСтПродКПр10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ИтСтПродКПр0");
						attrib.Value = ИтСтПродКПр0.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСИтКПр18");
						attrib.Value = СумНДСИтКПр18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСИтКПр10");
						attrib.Value = СумНДСИтКПр10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ИтСтПродОсвКПр");
						attrib.Value = ИтСтПродОсвКПр.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродВсП1Р9_18");
						attrib.Value = СтПродВсП1Р9_18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродВсП1Р9_10");
						attrib.Value = СтПродВсП1Р9_10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродВсП1Р9_0");
						attrib.Value = СтПродВсП1Р9_0.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСВсП1Р9_18");
						attrib.Value = СумНДСВсП1Р9_18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДСВсП1Р9_10");
						attrib.Value = СумНДСВсП1Р9_10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтПродОсвП1Р9Вс");
						attrib.Value = СтПродОсвП1Р9Вс.ToString();
						result.Attributes.Append(attrib);
					
				
		return result; }
		
		


	}

	/// <summary>
    /// Сведения по строке из дополнительных листов книги продаж
	/// </summary>
	[Persistent("FmTaxRuVatV504КнПродДЛСтр")] 
	public  partial class КнПродДЛСтр  {
		public КнПродДЛСтр (Session session): base(session) { } 
		
		
		
		
		private String _НомерПор;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомерПор {
			get { return _НомерПор; }
			set {
				if (!IsLoading) OnChanging("НомерПор", value); 
				SetPropertyValue<String>("НомерПор", ref _НомерПор, value); 
			}
		}
		
		
		
		private String _НомСчФПрод;
		/// <summary>
		/// Номер счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public String НомСчФПрод {
			get { return _НомСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомСчФПрод", value); 
				SetPropertyValue<String>("НомСчФПрод", ref _НомСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаСчФПрод;
		/// <summary>
		/// Дата счета-фактуры продавца
		/// </summary>
		
		[RuleRequiredField(DefaultContexts.Save)]
		
		public DateTime ДатаСчФПрод {
			get { return _ДатаСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаСчФПрод", ref _ДатаСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрСчФ;
		/// <summary>
		/// Номер исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрСчФ {
			get { return _НомИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрСчФ", value); 
				SetPropertyValue<String>("НомИспрСчФ", ref _НомИспрСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрСчФ;
		/// <summary>
		/// Дата исправления счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрСчФ {
			get { return _ДатаИспрСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрСчФ", ref _ДатаИспрСчФ, value); 
			}
		}
		
		
		
		private String _НомКСчФПрод;
		/// <summary>
		/// Номер корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомКСчФПрод {
			get { return _НомКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("НомКСчФПрод", value); 
				SetPropertyValue<String>("НомКСчФПрод", ref _НомКСчФПрод, value); 
			}
		}
		
		
		
		private DateTime _ДатаКСчФПрод;
		/// <summary>
		/// Дата корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаКСчФПрод {
			get { return _ДатаКСчФПрод; }
			set {
				if (!IsLoading) OnChanging("ДатаКСчФПрод", value); 
				SetPropertyValue<DateTime>("ДатаКСчФПрод", ref _ДатаКСчФПрод, value); 
			}
		}
		
		
		
		private String _НомИспрКСчФ;
		/// <summary>
		/// Номер исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public String НомИспрКСчФ {
			get { return _НомИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("НомИспрКСчФ", value); 
				SetPropertyValue<String>("НомИспрКСчФ", ref _НомИспрКСчФ, value); 
			}
		}
		
		
		
		private DateTime _ДатаИспрКСчФ;
		/// <summary>
		/// Дата исправления корректировочного счета-фактуры продавца
		/// </summary>
		
		
		
		public DateTime ДатаИспрКСчФ {
			get { return _ДатаИспрКСчФ; }
			set {
				if (!IsLoading) OnChanging("ДатаИспрКСчФ", value); 
				SetPropertyValue<DateTime>("ДатаИспрКСчФ", ref _ДатаИспрКСчФ, value); 
			}
		}
		
		
		
		private String _ОКВ;
		/// <summary>
		/// Код валюты по ОКВ
		/// </summary>
		
		
		
		public String ОКВ {
			get { return _ОКВ; }
			set {
				if (!IsLoading) OnChanging("ОКВ", value); 
				SetPropertyValue<String>("ОКВ", ref _ОКВ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФВ;
		/// <summary>
		/// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
		/// </summary>
		
		
		
		public String СтоимПродСФВ {
			get { return _СтоимПродСФВ; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФВ", value); 
				SetPropertyValue<String>("СтоимПродСФВ", ref _СтоимПродСФВ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ;
		/// <summary>
		/// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в рублях и копейках
		/// </summary>
		
		
		
		public String СтоимПродСФ {
			get { return _СтоимПродСФ; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ", value); 
				SetPropertyValue<String>("СтоимПродСФ", ref _СтоимПродСФ, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ18;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках,  по ставке 18 %
		/// </summary>
		
		
		
		public String СтоимПродСФ18 {
			get { return _СтоимПродСФ18; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ18", value); 
				SetPropertyValue<String>("СтоимПродСФ18", ref _СтоимПродСФ18, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ10;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках,  по ставке 10
		/// </summary>
		
		
		
		public String СтоимПродСФ10 {
			get { return _СтоимПродСФ10; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ10", value); 
				SetPropertyValue<String>("СтоимПродСФ10", ref _СтоимПродСФ10, value); 
			}
		}
		
		
		
		private String _СтоимПродСФ0;
		/// <summary>
		/// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках,  по ставке 0
		/// </summary>
		
		
		
		public String СтоимПродСФ0 {
			get { return _СтоимПродСФ0; }
			set {
				if (!IsLoading) OnChanging("СтоимПродСФ0", value); 
				SetPropertyValue<String>("СтоимПродСФ0", ref _СтоимПродСФ0, value); 
			}
		}
		
		
		
		private String _СумНДССФ18;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках, по ставке 18
		/// </summary>
		
		
		
		public String СумНДССФ18 {
			get { return _СумНДССФ18; }
			set {
				if (!IsLoading) OnChanging("СумНДССФ18", value); 
				SetPropertyValue<String>("СумНДССФ18", ref _СумНДССФ18, value); 
			}
		}
		
		
		
		private String _СумНДССФ10;
		/// <summary>
		/// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках, по ставке 10
		/// </summary>
		
		
		
		public String СумНДССФ10 {
			get { return _СумНДССФ10; }
			set {
				if (!IsLoading) OnChanging("СумНДССФ10", value); 
				SetPropertyValue<String>("СумНДССФ10", ref _СумНДССФ10, value); 
			}
		}
		
		
		
		private String _СтоимПродОсв;
		/// <summary>
		/// Стоимость продаж, освобождаемых от налога, по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках
		/// </summary>
		
		
		
		public String СтоимПродОсв {
			get { return _СтоимПродОсв; }
			set {
				if (!IsLoading) OnChanging("СтоимПродОсв", value); 
				SetPropertyValue<String>("СтоимПродОсв", ref _СтоимПродОсв, value); 
			}
		}
		
		
		
		private КнигаПродДЛ _КнигаПродДЛ;
		/// <summary>
		/// Связь КнигаПродДЛ-КнПродДЛСтр
		/// </summary>
		
		
		 [Association("КнигаПродДЛ-КнПродДЛСтр")]
		public КнигаПродДЛ КнигаПродДЛ {
			get { return _КнигаПродДЛ; }
			set {
				if (!IsLoading) OnChanging("КнигаПродДЛ", value); 
				SetPropertyValue<КнигаПродДЛ>("КнигаПродДЛ", ref _КнигаПродДЛ, value); 
			}
		}
		
		
		public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node){
				XmlNode result = document.CreateElement("КнПродДЛСтр");
				last_node.AppendChild(result);
				
				XmlAttribute attrib;
				
					
						attrib = document.CreateAttribute("НомерПор");
						attrib.Value = НомерПор.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомСчФПрод");
						attrib.Value = НомСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаСчФПрод");
						attrib.Value = ДатаСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрСчФ");
						attrib.Value = НомИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрСчФ");
						attrib.Value = ДатаИспрСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомКСчФПрод");
						attrib.Value = НомКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаКСчФПрод");
						attrib.Value = ДатаКСчФПрод.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("НомИспрКСчФ");
						attrib.Value = НомИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ДатаИспрКСчФ");
						attrib.Value = ДатаИспрКСчФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("ОКВ");
						attrib.Value = ОКВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФВ");
						attrib.Value = СтоимПродСФВ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ");
						attrib.Value = СтоимПродСФ.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ18");
						attrib.Value = СтоимПродСФ18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ10");
						attrib.Value = СтоимПродСФ10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродСФ0");
						attrib.Value = СтоимПродСФ0.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДССФ18");
						attrib.Value = СумНДССФ18.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СумНДССФ10");
						attrib.Value = СумНДССФ10.ToString();
						result.Attributes.Append(attrib);
					
				
					
						attrib = document.CreateAttribute("СтоимПродОсв");
						attrib.Value = СтоимПродОсв.ToString();
						result.Attributes.Append(attrib);
					
				
					
						
					
				
		return result; }
		
		


	}

}
