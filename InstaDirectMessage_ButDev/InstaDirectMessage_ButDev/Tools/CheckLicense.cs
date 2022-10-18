using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace InstaDirectMessage_ButDev.Tools
{
    public static class CheckLicense
    {
        public static string tempik = "";
        public static int remaining = 0;
        public static void GetRemainingTime()
        {
            /*string url = B64X.Encrypt("https://instamailchecker.site/InstaDirectMessage/LicenseEnd.php?q=" + MCrypt.Encrypt(MCrypt.Encrypt(ID.NewIDNumber)));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
            request.Method = "GET";
            request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string html = B64X.Encrypt(new StreamReader(response.GetResponseStream()).ReadToEnd());

            DateTime end = DateTime.Parse(MCrypt.Decrypt(MCrypt.Decrypt(B64X.Decrypt(html))));
            TimeSpan ts = end - DateTime.Now;*/

            //remaining = (int)ts.TotalDays;
            remaining = 14;
        }
        public static bool CheckInternetConnection()
        {
            int count = 0;
            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send("google.com");
                if (pr.Status == IPStatus.Success)
                {
                    count++;
                }
            }
            catch { }

            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send("google.ru");
                if (pr.Status == IPStatus.Success)
                {
                    count++;
                }
            }
            catch { }

            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send("google.by");
                if (pr.Status == IPStatus.Success)
                {
                    count++;
                }
            }
            catch { }

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            tempik = B64X.Encrypt(certificate.GetRawCertDataString());
            if (certificate.GetRawCertDataString() == "3082064B30820533A003020102021203F4AA827856B1930459D17AC57FF3A4F273300D06092A864886F70D01010B05003032310B300906035504061302555331163014060355040A130D4C6574277320456E6372797074310B3009060355040313025233301E170D3231303133313131303933375A170D3231303530313131303933375A3020311E301C06035504031315696E7374616D61696C636865636B65722E7369746530820222300D06092A864886F70D01010105000382020F003082020A0282020100DE1CBB12AE9A36C965EF5A3F0ED9964878D849A4C9570E7DEBA052C3F0023667A0E80EC9EC470B5F19E074840956A083A719B370B02D132030B10C95E0AC036C8C69BE4841DFE749DBF5CFC36EE689FF4C267A7900B72703742A6C2539AE04DE3E08E368DF0804FF8CB32916641CBDBE54A14AA4EFC668E323C29637112D4863B4C41F36AA04A99A422D704FE5393DD0D7C17338B84857484E65A67D812AB5C9D6EEC5E3100D9A3E3203C57B67C8539D333C67F492DE45BCA6253785E9501D23B7D31374CA68D61EB5CED76DAC72BACBA00143A7A38C0F39C818AD145913A3C1D67799D1FDF26FE4FC629F03650FC9BF3C8EE567CD7C9134C5B7108A9FBB189F1D472EC4DC79D6BC9A6C861853978EAF1978AB452D4D980379514C9A2DDC2482D5D9A29BAFED56E19CBD84F19C791B402783D3A2290E9B446A770A16AB4C43BAD28CEC9E64BEF7C184C3CCEEA62DAA2953B3A13FF9DCA912115974BD182B7E5C6035099AC1021BA2730B5058D893A4E0126FE804C75858F6041BCED58F542473F2D748EFF2369DBC41D52C70D462D83E8CC503DED8739EE7132DEE5B2476B50C8C7369E25E5FD09095D3D303CEDD01A6550109C7CE3366C0A988A59789A0B919B847DB7835FD9BBE98D36DF9B6F20CD4E60C47A821B9DAB141680D687F5EC278A847118FAAA56DEB09807309E698D9646E43D45066CDCC35B456476F41AB6CEB0203010001A382026B30820267300E0603551D0F0101FF0404030205A0301D0603551D250416301406082B0601050507030106082B06010505070302300C0603551D130101FF04023000301D0603551D0E04160414297CCDE45513CFB3C2CE2F5619FB084D1C66E5A4301F0603551D23041830168014142EB317B75856CBAE500940E61FAF9D8B14C2C6305506082B0601050507010104493047302106082B060105050730018615687474703A2F2F72332E6F2E6C656E63722E6F7267302206082B060105050730028616687474703A2F2F72332E692E6C656E63722E6F72672F303B0603551D11043430328215696E7374616D61696C636865636B65722E7369746582197777772E696E7374616D61696C636865636B65722E73697465304C0603551D20044530433008060667810C0102013037060B2B0601040182DF130101013028302606082B06010505070201161A687474703A2F2F6370732E6C657473656E63727970742E6F726730820104060A2B06010401D6790204020481F50481F200F00076009420BC1E8ED58D6C88731F828B222C0DD1DA4D5E6C4F943D61DB4E2F584DA2C200000177585934490000040300473045022075573493F73DAF3BAA11FD70564A11EF2FD0D7A61A4B828D642E4C0FFF8A9A91022100B0D5ADC09FFAD9FFF457816D00013C06081AB6DDB10ABEA801B4E2EDF52E42010076007D3EF2F88FFF88556824C2C0CA9E5289792BC50E78097F2E6A9768997E22F0D7000001775859348F00000403004730450220617D007BE1F9A8CA43D99229377B201D6E2E96D874312C4F6805ABF892578C80022100DC945A8D40314EFCF10B070EFD159B8B10AA7B3345D91D572B7B18605E236411300D06092A864886F70D01010B050003820101004687A95CC57BDACCFF56CB13A2F5D7B6B0D79A13A6414B9C8B3E66639F85D0C2EBE4E533147CE14E8B8DCFF585CFD1AEB2E1C15911DBED06B07F48F93FBC3242034932ED48C2EE29A4EA6CCF15834B0EE2CCF417A044D202AE4BCB11342D2B2027E51A9C856E9B9EDA51557F64237E05C6678D3A2770361FC698A8555AD6B2C6DA0B651F6B5C639D6759BBD3179359F46649C536A79BC9EE72D463E0B3F888DBE83A76F726C5A46615F9712E7172B6B179B83D211D248BA7F35828A58FA5C4013149A3C1B883972D5CA1E11056AB42D5C951E28B583A1A2F1D80FF5037267999EBB997913C74F46ACAF5EC73B6D89B78223B25FBA9D2E8D512B1AC6A0DC548D7") return true; else return false;
        }

        public class jsonDes
        {
            private string Salt, Result;
            public string salt
            {
                get
                {
                    return Salt;
                }
                set
                {
                    Salt = B64X.Encrypt(value);
                }
            }
            public string result
            {
                get
                {
                    return Result;
                }
                set
                {
                    Result = B64X.Encrypt(value);
                }
            }
        }
        public static BigInteger fun(BigInteger p, BigInteger a, BigInteger b)
        {
            BigInteger s = BigInteger.One;
            for (int i = 1; i <= b.IntValue; i++)
            {
                s = s.Multiply(a).Mod(p);
            }
            return s;
        }
        public static void Check()
        {
            if (!CheckInternetConnection()) { MessageBox.Show(Translate.Tr("Невозможно подключиться к сети!"), Translate.Tr("Ошибка!"), MessageBoxButtons.OK, MessageBoxIcon.Error); Environment.Exit(0); }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;/* | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;*/

                Random myrandom = new Random(DateTime.Now.Millisecond * DateTime.Now.Second * DateTime.Now.Year);
                BigInteger p = BigInteger.ValueOf(myrandom.Next(1000, 10000)), g, A, a, B, k;
                p = p.NextProbablePrime();
                g = BigInteger.ValueOf(FirstLevelRoot.generator(p.IntValue));
                a = BigInteger.ValueOf(myrandom.Next(p.IntValue));//A
                A = fun(p, g, a);

                string url = B64X.Encrypt("https://instamailchecker.site/InstaDirectMessage/InstaDirectMessageTime.php");
                HttpWebRequest requesttime = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                requesttime.Method = "GET";
                requesttime.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                HttpWebResponse responz = (HttpWebResponse)requesttime.GetResponse();
                string htmltime = B64X.Encrypt(new StreamReader(responz.GetResponseStream()).ReadToEnd());
                string hash = B64X.Encrypt(MCrypt.Decrypt(new string(B64X.Decrypt(htmltime).Reverse().ToArray())));
                hash = B64X.Encrypt(new string(B64X.Decrypt(hash).Reverse().ToArray()));

                url = B64X.Encrypt("https://instamailchecker.site/InstaDirectMessage/InstaDirectMessageTime.php?q=" + MCrypt.Encrypt(B64X.Decrypt(hash)));
                HttpWebRequest requesttimereal = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                requesttimereal.Method = "GET";
                requesttimereal.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                requesttimereal.Headers.Add("Cookie", responz.Headers["Set-Cookie"].Remove(responz.Headers["Set-Cookie"].IndexOf(';')));
                HttpWebResponse responze = (HttpWebResponse)requesttimereal.GetResponse();
                string TIME = B64X.Encrypt(MCrypt.Decrypt(new StreamReader(responze.GetResponseStream()).ReadToEnd()));


                HttpWebRequest reqt = (HttpWebRequest)WebRequest.Create("https://instamailchecker.site/InstaDirectMessage/InstaDirectMessageDH.php?q=" + MCrypt.Encrypt("p=" + p.ToString() + "&g=" + g.ToString() + "&A=" + A.ToString() + "&time=" + System.Web.HttpUtility.UrlEncode(B64X.Decrypt(TIME))));
                reqt.Method = "GET";
                reqt.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                HttpWebResponse resp = (HttpWebResponse)reqt.GetResponse();
                string htmlB = B64X.Encrypt(new StreamReader(resp.GetResponseStream()).ReadToEnd());
                B = new BigInteger(MCrypt.Decrypt(B64X.Decrypt(htmlB)));

                string ConstString = B64X.Encrypt("abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_");
                Random random = new Random(DateTime.Now.Second);
                string salt = B64X.Encrypt(new string(Enumerable.Repeat(B64X.Decrypt(ConstString), 10).Select(s => s[random.Next(s.Length)]).ToArray()));
                var outStr = new Dictionary<string, string>
                {
                    {"time",       B64X.Decrypt(TIME)},
                    {"id",         ID.NewIDNumber},
                    {"salt", B64X.Decrypt(salt)}
                };

                string req = B64X.Encrypt(JsonConvert.SerializeObject(outStr));

                string res = "";
                for (int i = 0; i < B64X.Decrypt(req).Length; i++)
                {
                    k = BigInteger.ValueOf(B64X.Decrypt(req)[i]);

                    k = k.Multiply(fun(p, B, a)).Mod(p);
                    res = B64X.Encrypt(B64X.Decrypt(res) + k.ToString() + ".");
                }

                url = B64X.Encrypt("https://instamailchecker.site/InstaDirectMessage/InstaDirectMessage.php?q=" + MCrypt.Encrypt(B64X.Decrypt(res)));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
                request.Method = "GET";
                request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                request.Headers.Add("Cookie", resp.Headers["Set-Cookie"].Remove(resp.Headers["Set-Cookie"].IndexOf(';')));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string html = B64X.Encrypt(new StreamReader(response.GetResponseStream()).ReadToEnd());
                
                string result = "";
                foreach (string value in MCrypt.Decrypt(B64X.Decrypt(html)).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    k = BigInteger.ValueOf(long.Parse(value));

                    k = k.Multiply(fun(p, B, p.Subtract(BigInteger.One.Add(a)))).Mod(p);
                    result = B64X.Encrypt(B64X.Decrypt(result) + (char)k.IntValue);
                }
                jsonDes account = JsonConvert.DeserializeObject<jsonDes>(B64X.Decrypt(result));
                if (MCrypt.Decrypt(new string(MCrypt.Decrypt(B64X.Decrypt(account.salt)).Reverse().ToArray())) != B64X.Decrypt(salt) || B64X.Decrypt(account.result) != "OK") throw new Exception();
            }
            catch (Exception e)
            {
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "InstaDirect-Soft.log"), MCrypt.Encrypt(Environment.NewLine + "saltish" + B64X.Encrypt("shka") + Environment.NewLine + e.ToString() + Environment.NewLine + B64X.Decrypt(tempik)), System.Text.Encoding.UTF8);
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var t = new Thread(() => ShowKeyForm());
                t.Start();
                t.Join();
                Environment.Exit(0);
            }

            Thread.Sleep(1500);
        }

        static void ShowKeyForm()
        {
            ShowKey showKey = new ShowKey();
            showKey.ShowDialog();
        }
    }
}
