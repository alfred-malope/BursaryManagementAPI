using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UniversityBLL
    {
        private readonly UniversityDAL _universityDAL;
        public UniversityBLL(UniversityDAL universityDAL)
        {
            _universityDAL = universityDAL;
        }


    }
}
