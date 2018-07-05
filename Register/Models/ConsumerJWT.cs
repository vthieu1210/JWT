using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class ConsumerJWT
    {
        public string key { get; set; }
        public int exp { get; set; }
        public string getSecret()
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "/consumers.txt";
            string data = "";

            if (File.Exists(path))
                data = System.IO.File.ReadAllText(@path);
            string[] consumers = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < consumers.Length; i++) {
                string[] tmp = consumers[i].Split(' ');
                if (tmp[0] == this.key)
                {
                    if (tmp.Length == 3)
                        this.exp = Int32.Parse(tmp[2]);
                    else
                        this.exp = 5;
                    return tmp[1];
                }
            }
            return null;
        }
    }
}