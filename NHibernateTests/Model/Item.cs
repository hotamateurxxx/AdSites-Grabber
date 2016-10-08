using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTests
{

    public class Item
    {

        /// <summary>
        /// Id.
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Name.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

    }
}
