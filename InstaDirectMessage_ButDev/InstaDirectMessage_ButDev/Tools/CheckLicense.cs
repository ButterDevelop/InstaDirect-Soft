﻿using Newtonsoft.Json;
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
            string url = B64X.Encrypt("https://instamailchecker.site/InstaDirectMessage/LicenseEnd.php?q=" + MCrypt.Encrypt(MCrypt.Encrypt(ID.NewIDNumber)));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(B64X.Decrypt(url));
            request.Method = "GET";
            request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string html = B64X.Encrypt(new StreamReader(response.GetResponseStream()).ReadToEnd());

            DateTime end = DateTime.Parse(MCrypt.Decrypt(MCrypt.Decrypt(B64X.Decrypt(html))));
            TimeSpan ts = end - DateTime.Now;

            remaining = (int)ts.TotalDays;
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
            if (certificate.GetRawCertDataString() == "3082070B308205F3A003020102020C46DBF06927A9BFD3280EF887300D06092A864886F70D01010B05003050310B300906035504061302424531193017060355040A1310476C6F62616C5369676E206E762D7361312630240603550403131D476C6F62616C5369676E205253412044562053534C2043412032303138301E170D3230303132323130303732375A170D3231303132323130303732375A30473121301F060355040B1318446F6D61696E20436F6E74726F6C2056616C69646174656431223020060355040313197777772E696E7374616D61696C636865636B65722E7369746530820222300D06092A864886F70D01010105000382020F003082020A0282020100A25E25C610330243B35E0C71A7379FE7E1C915ECDA0746BF3DF4FE681F0A013B12B145E5E7EA9E87B666DE6123A115B802CF79B3CF4AEC3347D71C9E1093E69C2734AE549C280061324978E569D7B4C4CE4A7D499E60B1BD5AC6DF8753CF6CFC0C75A5073E8E7F5EA43CD3E2F40675BBD04B0DD1AD858CDD153384D3F4D1EBFD9BB71C3300BC0C48163D62BB75743421A473CAF0D139E72C877E1B9382956164C6535075E14524F8FD19DA07870417CCC08CA70D5460EBFA5D28E22F81232D47E3C813D5BD1FC29B3F00790E9E2DE5FA061A52AE024C3911CC8E606039BAE8DE5F873B4BF06CD576BD2CA48DABC6BD1C1071E9389EEACB6F5BC0AD185C1BD79B47F7BAC7F9B362AC0CAE00E9E938C83DC00D9CFAB8CA19EBBB06AD61C009FDF61077A83C2959A4A47F15529424165A7CACCE5A0BA1980CD07D2380712ACC2AAD82BB07DAE8A8B1120D91B0A26475CCFD3E552B4AE9E0C90755DD7F0462669529A37F8ABDE4ABA4194A2EA222290508EB2A58F50BE28070793A641035886BE5974DB6C3D3B6008528F4132CDEACD109745EDB6BE20511740A9E6DB2028392BF95ECF91F6C9F013A8B2A12BAD9A9701D758A3120BA1D50FEE6E547C753BDF76B044819A75C77BB5C81033D8AB02E823A28E14C0E0E1DAAB4AC8442821F916C27AD9F67DE913515A829F6858ABD3807468112EE9CB52A9D7495DFB57D410B6CBB310203010001A38202EC308202E8300E0603551D0F0101FF0404030205A030818E06082B06010505070101048181307F304406082B060105050730028638687474703A2F2F7365637572652E676C6F62616C7369676E2E636F6D2F6361636572742F6773727361647673736C6361323031382E637274303706082B06010505073001862B687474703A2F2F6F6373702E676C6F62616C7369676E2E636F6D2F6773727361647673736C63613230313830560603551D20044F304D304106092B06010401A032010A3034303206082B06010505070201162668747470733A2F2F7777772E676C6F62616C7369676E2E636F6D2F7265706F7369746F72792F3008060667810C01020130090603551D1304023000303F0603551D1F043830363034A032A030862E687474703A2F2F63726C2E676C6F62616C7369676E2E636F6D2F6773727361647673736C6361323031382E63726C303B0603551D110434303282197777772E696E7374616D61696C636865636B65722E736974658215696E7374616D61696C636865636B65722E73697465301D0603551D250416301406082B0601050507030106082B06010505070302301F0603551D230418301680148180D62879354A5B793589398F12176E117B2C11301D0603551D0E04160414DC754FEFA576EC582AB6FC740BE2CA9852943DCF30820103060A2B06010401D6790204020481F40481F100EF007500BBD9DFBC1F8A71B593942397AA927B473857950AAB52E81A909664368E1ED1850000016FCCB89F35000004030046304402204C735C1DDD9450A7C0527D323AADECFD19A6FCC0775728E5D111DA45000C1B87022035CE0EC52B7458C97AB123AA3EF9DD9945A2BA9CD945A5E8DB53415D96D7D2B80076008775BFE7597CF88C43995FBDF36EFF568D475636FF4AB560C1B4EAFF5EA0830F0000016FCCB89F6E00000403004730450221008E6BF6103FD5F4E3AC9D48636E79B3A712185DBD2F569860FBB155D59CAFA4C8022021E736F64FFE65E41EEBD3E3034BD461B4ADFD4819F00CB0209B4B93B447012F300D06092A864886F70D01010B050003820101002B8DEF9F36AD761622A1E3513811B805327B3EB292A7CC3DB850CAA85B8D0CE391623C8574F1D2E286E4974524F8D84CD53E1443E2A7D1546B2B20A88E218C0D12481359452031CC96A4A43F08BF24E8D449EE25D3D7D85D39A212D750306D8BA0B4EDC88F47D04C1B903745E244F018483C81623F911D9A0F9E9388DA30E419AC0F4E3BE611C7FBA04FFFE21D0B0AFA0C3BA814448DC47FABE8F4DB47D4B48A13854A1CCA4107B64A25BD0D54174BEA0C23FCF03C5109F7A45A29E33A9198C2EB641EFC217D7E44C4923FB1CDC15C98E0FE85A9EF9287E858B94084BC5F472A7D2D179695DF76DD1DC19A06163C22836710FC958FA6845D563023D5E4858040") return true; else return false;
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
