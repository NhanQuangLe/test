using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Model;

namespace TestProject.Helpers
{
    public class FlightAPIService
    {
        // TODO: Define your api service here. Ref: https://github.com/reactiveui/refit
        //Example
        [Get("/flights")]
        Task<List<Flight>> GetAllFlight(string user)
        {
            // Em không làm phần backend call api trên trường
            // Lúc học wpf em lấy data từ Local SQL server thôi ạ

            // khi call api sẽ lấy dữ liệu và chuyển sang lớp FlightDTO, nhưng vì phải lấy fullName của flight nên
            // phải thực hiện call api thứ 2 để lấy full name
        }
    }
}
