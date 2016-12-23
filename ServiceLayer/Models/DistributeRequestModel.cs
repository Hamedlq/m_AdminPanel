using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer.Models
{
    public class DistributeRequestModel
    {
        public DistributeRequestModel()
        {
            DeliverPoints = new List<DeliverPoint>();
            Origin = new ODPoint();
        }
        public ODPoint Origin { set; get; }

        public List<DeliverPoint> DeliverPoints { set; get; }

        public int IsReturn { set; get; }

    }
    public class ODPoint
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Lat { set; get; }
        public string Lng { set; get; }
    }

    public class DeliverPoint
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Lat { set; get; }
        public string Lng { set; get; }
        public double Distance { set; get; }
    }
}
