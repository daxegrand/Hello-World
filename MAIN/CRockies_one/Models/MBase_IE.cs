using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRockies_one.Models
{
    public class MBase_IE
    {
        private InsideEdEntities ie_entities;

        public MBase_IE()
        {
            ie_entities = new InsideEdEntities();
        }
        public InsideEdEntities IE_Entities
        {
            get { return ie_entities; }
        }
    }
}