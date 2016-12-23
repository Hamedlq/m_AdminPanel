using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public enum TimingOptions
    {
        Now=1,
        Today = 2,
        InDateAndTime=3,
        Weekly=4
    };
    public enum PayingOptions
    {
        NoMatter=1,
        MinMax=2,
        Free=3
    };
    public enum CarTypes
    {
        Praide = 1,
        Peykan = 2,
        Free = 3
    };
    public enum ImageType
    {
        UserPic = 1,
        LicensePic = 2,
        CarPic = 3
    }

}
