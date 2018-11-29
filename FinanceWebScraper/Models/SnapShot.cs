using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceWebScraper.Models
{
    public class SnapShot
    {
        public int ID { get; set; }
        public string Userid { get; set; }
        public DateTime SnapshotTime { get; set; }
        public List<Stock> Stocks { get; set; }

        public SnapShot()
        {

        }

        public SnapShot(string userid, DateTime snaptime)
        {
            this.Userid = userid;
            this.SnapshotTime = snaptime;
        }
    }
}
