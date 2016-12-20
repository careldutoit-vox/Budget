using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static  class MailRepository
    {
        public static void Read()
        {
            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
              "dutoit.carel7@gmail.com", "n!con123$%^", AuthMethod.Login, true))
            {
                // Find messages that were sent from abc@def.com and have
                // the string "Hello World" in their subject line.
                IEnumerable<uint> uids = Client.Search(
                    SearchCondition.From("chantal@kibogroup.co.za").And(
                    SearchCondition.Subject("Killarney - Levy Statement"))
                );
                var bla = uids;
            }

        }
    }
}
