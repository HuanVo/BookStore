using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Commons;
namespace DAO
{
    public class MailDao
    {
        BookStoreDbContext db = null;
        private static MailDao instance;

        public static MailDao Instance
        {
            get{
                if(instance == null)
                    instance = new MailDao();
                return instance;
            }
           private  set { instance = value; }
        }

        private MailDao()
        {
            db = new BookStoreDbContext();
        }

        public MAIL GetInfo(int mailId)
        {
            MAIL mailInfo = new MAIL();
            try
            {
                mailInfo = db.MAILs.Find(mailId);
            } catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return mailInfo;
        }
    }
}
