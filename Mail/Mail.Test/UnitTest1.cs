
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Mail.Test
{
    public class UnitTest1 : IYandexMailConfig
    {
        string auth = null;
        [Fact]
        public void Test1()
        {
            // "admin@meoga.ga"
            //"eskmmqxunzbynnis"
            //InstagramTVCu
            YandexMail yandexMail = new YandexMail(this);
            var a = yandexMail.GetLinkConfirm("ConfirmEtsy", "NeilOgdenneil.ogden@vandeso1la.site", "emails@mail.etsy.com", "Confirm your Etsy account", "href=\"https://www.etsy.com/confirm+.+&link_clicked=1");
            var b = a.Subject.Replace("href=\"", string.Empty);
            //yandexMail.MoveToTrash(a);
            yandexMail.Dispose();
        }
        public string Username => "admin@meoga.ga";
        public string Password => "eskmmqxunzbynnis";
        public string Token => "3c053abb47122b18637d3eed6ae9c436";


    }
}
