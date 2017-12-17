using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BookStore.Commons;
namespace DAO
{
    public class SystemDAO
    {
        BookStoreDbContext db = null;
        private static SystemDAO instance;

        public static SystemDAO Instance
        {
            get
            { if (instance == null)
                    instance = new SystemDAO();
                return instance;
            }
            private set { instance = value; }
        }

        private SystemDAO()
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
