using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace onboarding.bll.Kafka
{
    public interface IkafkaSender
    {
        Task SendAsync(string topic, object message);
    }
}
