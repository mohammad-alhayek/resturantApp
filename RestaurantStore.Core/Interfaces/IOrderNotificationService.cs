using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantStore.Core.Interfaces
{
    public interface IOrderNotificationService
    {
        
            Task NewOrderCreated(object order);
        
    }
}
