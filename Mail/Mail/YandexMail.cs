using ImapX;
using ImapX.Authentication;
using ImapX.Collections;
using ImapX.Enums;
using ImapX.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mail
{
    public class YandexMail : IDisposable
    {
        private IYandexMailConfig _config;
        private ImapClient _client;
        // "admin@meoga.ga"
        //"eskmmqxunzbynnis"
        //InstagramTVCu
        public YandexMail(IYandexMailConfig config)
        {
            this._config = config;
        }
        public bool Connect()
        {
            _client = new ImapClient("imap.yandex.ru", ImapClient.DefaultImapSslPort, true, false);
            return _client.Connect() && _client.Login(_config.Username, _config.Password);
        }
        public MessageCollection ReadMailbox(string folderName, int count = -1)
        {
            _client = new ImapClient("imap.yandex.ru", ImapClient.DefaultImapSslPort, true, false);
            if (_client.Connect())
            {

                if (_client.Login(_config.Username, _config.Password))
                {
                    _client.Folders[folderName].Messages.Download(count: count);
                    return _client.Folders[folderName].Messages;
                }
            }
            return null;
        }
        public Message[] Search(string query, string folderName)
        {
            return _client.Folders[folderName].Search(query);
        }

        public IEnumerable<Message> FilterMessagesBy(string folderName, string emailAddress = null, string fromEmailAddress = null, string subject = null)
        {
            var messages = ReadMailbox(folderName, 10);
            return messages.Where((message) =>
            {
                bool result = false;
                if (emailAddress != null)
                    result = message.To.Select<MailAddress, string>((mailAddress) => mailAddress.Address).Contains(emailAddress);
                if (fromEmailAddress != null)
                    result = result && message.From.Address == fromEmailAddress;
                if (subject != null)
                    return result && message.Subject == subject;
                return result;
            });
        }
        public Message GetLinkConfirm(string folderName, string emailAddress, string fromEmailAddress, string subject, string pattern)
        {
            var messages = FilterMessagesBy(folderName, emailAddress, fromEmailAddress, subject);//SearchMailbox($"(TO \"{emailAddress}\" FROM \"{fromEmailAddress}\" SUBJECT \"{subject}\")", folderName);
            foreach (var item in messages)
            {
                string body = item.Body.HasHtml ? item.Body.Html : item.Body.Text;

                Regex rx = new Regex(pattern);
                MatchCollection matches = rx.Matches(body);
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count > 0)
                    {
                        item.Seen = true;
                        item.Subject = groups[0].Value;
                        return item;
                    }

                }
            }
            return null;
        }

        public string GetCode(string folderName, string emailAddress)
        {
            foreach (var item in FilterMessagesBy(folderName, emailAddress))
            {
                string body = item.Body.HasHtml ? item.Body.Html : item.Body.Text;

                Regex rx = new Regex(@"[0-9]{6}");
                MatchCollection matches = rx.Matches(body);
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count > 0)
                        return groups[0].Value;
                }
            }
            return null;
        }

        public bool MoveToTrash(Message message)
        {

            if (_client.IsConnected && _client.IsAuthenticated)
                return message.MoveTo(_client.Folders.Trash);
            else
                throw new Exception("client not Authentication");
        }

        public bool MoveToFolder(Message message, string folderName)
        {

            if (_client.IsConnected && _client.IsAuthenticated)
                return message.MoveTo(_client.Folders[folderName]);
            else
                throw new Exception("client not Authentication");
        }

        public void Dispose()
        {
            _client.Disconnect();
        }
    }
}
