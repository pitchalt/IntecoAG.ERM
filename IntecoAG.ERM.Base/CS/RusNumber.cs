// A set of C# classes for spelling Russian numerics 
// Copyright (c) 2002 RSDN Group

using System;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;

namespace RSDN
{
    public class RusNumber
    {
        private static string[] hunds =
        {
            "", "��� ", "������ ", "������ ", "��������� ",
            "������� ", "�������� ", "������� ", "��������� ", "��������� "
        };

        private static string[] tens =
        {
            "", "������ ", "�������� ", "�������� ", "����� ", "��������� ",
            "���������� ", "��������� ", "����������� ", "��������� "
        };

        public static string Str(int val, bool male, string one, string two, string five)
        {
            string[] frac20 =
            {
                "", "���� ", "��� ", "��� ", "������ ", "���� ", "����� ",
                "���� ", "������ ", "������ ", "������ ", "����������� ",
                "���������� ", "���������� ", "������������ ", "���������� ",
                "����������� ", "���������� ", "������������ ", "������������ "
            };

            int num = val % 1000;
            if(0 == num) return "";
            if(num < 0) throw new ArgumentOutOfRangeException("val", "�������� �� ����� ���� �������������");
            if(!male)
            {
                frac20[1] = "���� ";
                frac20[2] = "��� ";
            }

            StringBuilder r = new StringBuilder(hunds[num / 100]);

            if(num % 100 < 20)
            {
                r.Append(frac20[num % 100]);
            }
            else
            {
                r.Append(tens[num % 100 / 10]);
                r.Append(frac20[num % 10]);
            }
            
            r.Append(Case(num, one, two, five));

            if(r.Length != 0) r.Append(" ");
            return r.ToString();
        }

        public static string Case(int val, string one, string two, string five)
        {
            int t=(val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2: case 3: case 4: return two;
                default: return five;
            }
        }

        public static string Str(Int64 val, Boolean IsUpper = false, String seniorOne = "", String seniorTwo = "", String seniorFive = "") {

            bool minus = false;
            if (val < 0) { val = -val; minus = true; }

            int n = (int)val;
            int remainder = (int)((val - n + 0.005) * 100);

            StringBuilder r = new StringBuilder();

            if (0 == n) r.Append("0 ");
            if (n % 1000 != 0)
                r.Append(RusNumber.Str(n, true, seniorOne, seniorTwo, seniorFive));
            else
                r.Append(seniorFive);

            n /= 1000;

            r.Insert(0, RusNumber.Str(n, false, "������", "������", "�����"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "�������", "��������", "���������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "���������", "����������", "�����������"));
            if (minus) r.Insert(0, "����� ");

//            r.Append(remainder.ToString("00 "));
//            r.Append(RusNumber.Case(remainder, seniorOne, seniorTwo, seniorFive));

            //������ ������ ����� ���������
            if (IsUpper) 
                r[0] = char.ToUpper(r[0]);

            return r.ToString();
        }
    };

    struct CurrencyInfo
    {
        public bool male;
        public string seniorOne, seniorTwo, seniorFive;
        public string juniorOne, juniorTwo, juniorFive;
    };

    public class RusCurrencySectionHandler:IConfigurationSectionHandler
    {
        public object Create( object parent, object configContext, XmlNode section )
        {
            foreach(XmlNode curr in section.ChildNodes)
            {
                if(curr.Name=="currency")
                {
                    XmlNode senior=curr["senior"];
                    XmlNode junior=curr["junior"];
                    RusCurrency.Register(   
                        curr.Attributes["code"].InnerText,
                        (curr.Attributes["male"].InnerText == "1"),
                        senior.Attributes["one"].InnerText,
                        senior.Attributes["two"].InnerText,
                        senior.Attributes["five"].InnerText,
                        junior.Attributes["one"].InnerText,
                        junior.Attributes["two"].InnerText,
                        junior.Attributes["five"].InnerText);
                }
            }
            return null;
        }
    };
            
    public class RusCurrency
    {
        private static HybridDictionary currencies = new HybridDictionary();

        static RusCurrency()
        {
            Register("RUR", true, "�����", "�����", "������", "�������", "�������", "������");          
            Register("EUR", true, "����", "����", "����", "��������", "���������", "����������");           
            Register("USD", true, "������", "�������", "��������", "����", "�����", "������");          
//            ConfigurationSettings.GetConfig("currency-names");
        }

        public static void Register(string currency, bool male, 
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            CurrencyInfo info;
            info.male = male;
            info.seniorOne = seniorOne; info.seniorTwo = seniorTwo; info.seniorFive = seniorFive; 
            info.juniorOne = juniorOne; info.juniorTwo = juniorTwo; info.juniorFive = juniorFive;
            currencies.Add(currency, info);
        }

        public static string Str(double val)
        {
            return Str(val, "RUR");
        }

        public static string Str(double val, string currency)
        {
            if(!currencies.Contains(currency)) 
                throw new ArgumentOutOfRangeException("currency", "������ \""+currency+"\" �� ����������������");
            
            CurrencyInfo info = (CurrencyInfo)currencies[currency];
            return Str(val, info.male, 
                info.seniorOne, info.seniorTwo, info.seniorFive,
                info.juniorOne, info.juniorTwo, info.juniorFive);
        }

        public static string Str(double val, bool male, 
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            bool minus = false;
            if(val < 0) { val = - val; minus = true; }

            int n = (int) val;
            int remainder = (int) (( val - n + 0.005 ) * 100);

            StringBuilder r = new StringBuilder();

            if(0 == n) r.Append("0 ");
            if(n % 1000 != 0)
                r.Append(RusNumber.Str(n, male, seniorOne, seniorTwo, seniorFive));
            else
                r.Append(seniorFive);

            n /= 1000;
         
            r.Insert(0, RusNumber.Str(n, false, "������", "������", "�����"));
            n /= 1000;
         
            r.Insert(0, RusNumber.Str(n, true, "�������", "��������", "���������"));
            n /= 1000;
         
            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;
         
            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;
         
            r.Insert(0, RusNumber.Str(n, true, "���������", "����������", "�����������"));
            if(minus) r.Insert(0, "����� ");

            r.Append(remainder.ToString("00 "));
            r.Append(RusNumber.Case(remainder, juniorOne, juniorTwo, juniorFive));
         
            //������ ������ ����� ���������
            r[0] = char.ToUpper(r[0]);

            return r.ToString();
        }
    };
};