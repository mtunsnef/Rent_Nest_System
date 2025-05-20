using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(MailContent mailContent);
    }
}
