using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISmsService
    {
        bool IsActiveNumber();
        bool HasCredit(int smsQuantity);
        long SendSms(List<string> reciever,  string message);
        int DeliveryStatus(long recId);
        decimal GetBaseAmount();

        int SendToList(List<string> reciever, string message);
       int SendToOne(string reciever, string message);
    }
}
