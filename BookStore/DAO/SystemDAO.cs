using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BookStore.Commons;
namespace DAO
{
    public class SystemDao
    {
        BookStoreDbContext db = null;
        private static SystemDao instance;

        public static SystemDao Instance
        {
            get
            { if (instance == null)
                    instance = new SystemDao();
                return instance;
            }
            private set { instance = value; }
        }

        private SystemDao()
        {
            db = new BookStoreDbContext();
        }
        public String GetValueConfigById(int configId)
        {
            var result = db.SYSTEMS.Find(configId);
            if(result !=null)
                return result.config_value;
            return "";
        }
    }
}
