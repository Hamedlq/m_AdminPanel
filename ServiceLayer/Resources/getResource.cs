using System;

namespace ServiceLayer
{
    public class getResource
    {
        static private System.Resources.ResourceManager stringsResourceMan;
        static private System.Resources.ResourceManager messageResourceMan;

        public static string getString(string value)
        {
            string res="";
            if (object.ReferenceEquals(stringsResourceMan, null))
            {
                var temp = new global::System.Resources.ResourceManager("ServiceLayer.Resources.Strings", typeof(Resources.Strings).Assembly);
                stringsResourceMan = temp;
            }
            try
            {
                res = stringsResourceMan.GetString(value);
            }
            catch (Exception e)
            {
                res = "Resource Error";
            }
            return res;
        }
        public static string getMessage(string value)
        {
            string res = "";
            if (object.ReferenceEquals(messageResourceMan, null))
            {
                System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ServiceLayer.Resources.Messages", typeof(Resources.Messages).Assembly);
                messageResourceMan = temp;
            }
            try
            {
                res = messageResourceMan.GetString(value);
            }
            catch (Exception e)
            {
                res = "Resource Error";
            }
            return res;
        }

       
    }
}
