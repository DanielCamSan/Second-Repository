using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController: ControllerBase
    {
        private static readonly List<Membership> membs = new()
        {
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Basic", StartDate= DateTime.Now, EndDate = DateTime.MaxValue, Status = "Active"  },
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Pro", StartDate= DateTime.MinValue, EndDate = DateTime.Now, Status = "Canceled"  },
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Premium", StartDate= DateTime.MinValue, EndDate = DateTime.Now, Status = "Expired"  },
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Basic", StartDate= DateTime.Now, EndDate = DateTime.MaxValue, Status = "Active"  },
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Pro", StartDate= DateTime.MinValue, EndDate = DateTime.Now, Status = "Canceled"  },
            new Membership{ Id= Guid.NewGuid(), MemberId= Guid.NewGuid(), Plan = "Premium", StartDate= DateTime.MinValue, EndDate = DateTime.Now, Status = "Expired"  }
        };

        private static(int pg, int lim) NormilizePg(int? pg,  int? lim)
        {
            var page = pg.GetValueOrDefault(1);
            if (page<1) page =1 ;
            var it = lim.GetValueOrDefault(10);
            if (it < 1 || it > 100) it= 1;
            return (page, it);
        }

    }
}
