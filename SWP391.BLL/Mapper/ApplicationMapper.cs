using SWP391.DAL.Entities.payment;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using SWP391.DAL;

namespace SWP391.BLL.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Payment, PaymentDtos>().ReverseMap();
        }
    }
}
