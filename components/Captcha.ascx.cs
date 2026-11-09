using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class Captcha : UserControl
{
    private const string SessionKey = "CaptchaCode";

    private static readonly Random rnd = new Random();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        string code = NewCode();
        Session[SessionKey] = code;

        imgCaptcha.Src = "data:image/png;base64," + Convert.ToBase64String(RenderPng(code));
        txtCaptcha.Text = "";
    }

    public bool IsValidCode()
    {
        string expected = Session[SessionKey] as string;
        bool ok = expected != null && string.Equals(txtCaptcha.Text.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        Session.Remove(SessionKey);
        return ok;
    }

    private static String NewCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ23456789";
        char[] code = new char[5];
        lock (rnd)
        {
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = chars[rnd.Next(chars.Length)];
            }
        }
        return new string(code);
    }

    private static byte[] RenderPng(string code)
    {
        using (var bmp = new Bitmap(150, 50))
        using (var g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.WhiteSmoke);

            lock (rnd)
            {
                for (int i = 0; i < 6; i++)
                {
                    g.DrawLine(Pens.Gray, rnd.Next(150), rnd.Next(50), rnd.Next(50), rnd.Next(50));
                }
                using (var font = new Font("Arial", 22, FontStyle.Bold))
                {
                    for (int i = 0; i < code.Length; i++)
                    {
                        g.DrawString(code[i].ToString(), font, Brushes.DarkBlue, 10 + i * 27, 6 + rnd.Next(8));
                    }
                }
            }

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
