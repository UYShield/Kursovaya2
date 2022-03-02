using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyKurs_2
{
    internal class DBEntities
    {
        private static KEntities _context;
        public static KEntities GetContext()
        {
            if (_context == null)
                _context = new KEntities();
            return _context;
        }
    }
}
